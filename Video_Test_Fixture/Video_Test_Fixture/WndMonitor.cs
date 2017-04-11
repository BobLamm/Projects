using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Utility.VgvUtility;

using AlaxInfoIpVideoSource;
using DirectShowLib;

namespace Video_Test_Fixture
{
    public partial class WndMonitor : Form
    {
        private GlobalConfig cfg = null;
        IGraphBuilder graphPreview = null;
        IVideoWindow videoWindow = null;
        IMediaControl mediaControl = null;
        IMediaEventEx mediaEventEx = null;

        DsROTEntry rot = null;

        IBaseFilter pVideoRenderer = null;
        IJpegVideoSourceFilter rawFilter = null;

        /// <summary>
        /// camera preview window
        /// </summary>
        /// <param name="cam"></param>
        public WndMonitor(/*IBaseFilter srcFilter,*/CameraObject cam)
        {
            IBaseFilter srcFilter = null;
            int hr = 0;

            cfg = GlobalConfig.Instance;

            InitializeComponent();

            try
            {
                // An exception is thrown if cast fail
                graphPreview = (IGraphBuilder)new FilterGraph();
                //graph builder
                ICaptureGraphBuilder2 pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
                hr = pBuilder.SetFiltergraph(graphPreview);
                VgvUtil.checkHR(hr,"Can't SetFiltergraph");
                {
                    string userPass = (cam.userName != null && cam.password != null)
                                    ? cam.userName + ":" + cam.password + "@"
                                    : "";
                    string location = "http://" + userPass + cam.ipAddrPort + "/axis-cgi/mjpg/video.cgi?resolution="
                                    + cam.scanWidth + "x" + cam.scanLines;

                    //add Alax.Info JPEG Video Source
                    Guid CLSID_VideoSource = new Guid("{A8DA2ECB-DEF6-414D-8CE2-E651640DBA4F}");    // IpVideoSource.dll
                    rawFilter = (IJpegVideoSourceFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoSource));
                    srcFilter = rawFilter as IBaseFilter;
                    hr = (graphPreview as IFilterGraph2).AddFilter(srcFilter,"Alax.Info JPEG Video Source");
                    VgvUtil.checkHR(hr,"Can't add Alax.Info JPEG Video Source to graph");

                    rawFilter.Location = location;
                    rawFilter.Width = cam.scanWidth;
                    rawFilter.Height = cam.scanLines;
                }
                // Render the preview pin on the video capture filter
                // Use this instead of pGraph.RenderFile
                Guid CLSID_VideoRenderer = new Guid("{B87BEB7B-8D29-423F-AE4D-6582C10175AC}"); //quartz.dll
                pVideoRenderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoRenderer));
                hr = (graphPreview as IFilterGraph2).AddFilter(pVideoRenderer,"Video Renderer");
                VgvUtil.checkHR(hr,"Can't add Video Renderer to graph");
                hr = pBuilder.RenderStream(PinCategory.Preview,MediaType.Video,srcFilter,null,pVideoRenderer);
                VgvUtil.checkHR(hr,"cannot render preview pin");

                // Get DirectShow interfaces
                mediaControl = (IMediaControl)graphPreview;
                videoWindow = (IVideoWindow)graphPreview;
                mediaEventEx = (IMediaEventEx)graphPreview;

//              VgvUtil.EnumFilters(graphPreview);

                hr = mediaEventEx.SetNotifyWindow(Handle,GlobalConfig.WM_GRAPHNOTIFY,IntPtr.Zero);
                VgvUtil.checkHR(hr,"SetNotifyWindow failed");

                // Set video window style and position
                SetupPreviewWindow();

                // Add our graph to the running object table, which will allow
                // the GraphEdit application to "spy" on our graph
                rot = new DsROTEntry(graphPreview);

                // Start previewing video data
                hr = mediaControl.Run();
                VgvUtil.checkHR(hr,"cannot start video rendering");
            }
            catch (COMException ex)
            {
                MessageBox.Show("COM error: " + ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                // Stop capturing and release interfaces
                CloseInterfaces();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void CloseInterfaces()
        {
            // Stop previewing data
            if (mediaControl != null)
                mediaControl.StopWhenReady();

            // Stop receiving events
            if (mediaEventEx != null)
                mediaEventEx.SetNotifyWindow(IntPtr.Zero,GlobalConfig.WM_GRAPHNOTIFY,IntPtr.Zero);

            // Relinquish ownership (IMPORTANT!) of the video window.
            // Failing to call put_Owner can lead to assert failures within
            // the video renderer, as it still assumes that it has a valid
            // parent window.
            if (videoWindow != null)
            {   videoWindow.put_Visible(OABool.False);
                videoWindow.put_Owner(IntPtr.Zero);
            }

            // Remove filter graph from the running object table
            if (rot != null)
            {   rot.Dispose();
                rot = null;
            }

            // Release DirectShow interfaces
            if (mediaControl != null)
            {   Marshal.ReleaseComObject(mediaControl);
                mediaControl = null;
            }
            if (mediaEventEx != null)
            {   Marshal.ReleaseComObject(mediaEventEx);
                mediaEventEx = null;
            }
            if (videoWindow != null)
            {   Marshal.ReleaseComObject(videoWindow);
                videoWindow = null;
            }
            if (graphPreview != null)
            {
//              VgvUtil.ReleaseFilters(graphPreview);
                Marshal.ReleaseComObject(graphPreview);
                graphPreview = null;
            }
        }

        /// <summary>
        /// sets the video window to be a child of the main window
        /// </summary>
        public void SetupPreviewWindow()
        {
            int hr = 0;

            // Set the video window to be a child of the main window
            hr = videoWindow.put_Owner(Handle);
            DsError.ThrowExceptionForHR(hr);

            hr = videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(hr);

            // Use helper function to position video window in client rect 
            // of main application window
            ResizePreviewWindow();

            // Make the video window visible, now that it is properly positioned
            hr = videoWindow.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// resizes the video preview window to match owner window size
        /// </summary>
        public void ResizePreviewWindow()
        {
            // Resize the video preview window to match owner window size
            if (videoWindow != null)
            {
                videoWindow.SetWindowPosition(0,0,ClientSize.Width,ClientSize.Height);
            }
        }

        private void HandleGraphEvent()
        {
            int hr = 0;
            EventCode evCode;
            IntPtr evParam1, evParam2;

            if (mediaEventEx == null)
                return;

            while (mediaEventEx.GetEvent(out evCode,out evParam1,out evParam2,0) == 0)
            {
                // Free event parameters to prevent memory leaks associated with
                // event parameter data.  While this application is not interested
                // in the received events, applications should always process them.
                hr = mediaEventEx.FreeEventParams(evCode,evParam1,evParam2);
                DsError.ThrowExceptionForHR(hr);

                // Insert event processing code here, if desired
            }
        }

        /// <summary>
        /// WndProc override
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case GlobalConfig.WM_GRAPHNOTIFY:
                {
                    HandleGraphEvent();
                    break;
                }
            }

            // Pass this message to the video window for notification of system changes
            if (videoWindow != null)
                videoWindow.NotifyOwnerMessage(m.HWnd,m.Msg,m.WParam,m.LParam);

            base.WndProc(ref m);
        }

        private void WndPreview_FormClosing(object sender,FormClosingEventArgs e)
        {
//          MessageBox.Show(e.ToString());

            CloseInterfaces();

            if (rawFilter != null)
                rawFilter = null;
            if (pVideoRenderer != null)
                pVideoRenderer = null;
        }
    }
}
