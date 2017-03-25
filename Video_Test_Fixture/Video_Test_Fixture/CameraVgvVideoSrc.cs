/**
 * File: CameraVgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/CameraVgvVideoSrc.cs,v $
 */
/**
*	Implements a camera video source
*
*	TODO: finish implementation
*
*	Author:			Fred Koschara
*	Creation Date:	January eighth, 2017
*	Last Modified:	March 22, 2017 @ 9:21 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/03/22	wfredk	add documentation
*	2017/03/19	wfredk	add preview() method
*	2017/01/09	wfredk	original development
*		|						|
*	2017/01/08	wfredk	original development
*/
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Utility.VgvUtility;

using AlaxInfoIpVideoSource;
using DirectShowLib;

namespace Video_Test_Fixture
{
    /// <summary>
    /// implements a camera video source
    /// </summary>
    public class CameraVgvVideoSrc : BaseVgvVideoSrc
    {
        /// <summary>
        /// bool, set if the camera is in a paused state
        /// </summary>
        protected bool bPaused = false;
        /// <summary>
        /// CameraObject, information about the camera
        /// </summary>
        protected CameraObject inputCam;

        IJpegVideoSourceFilter rawFilter = null;
        IBaseFilter srcFilter = null;
        /// <summary>
        /// property (accessor) for this camera's source filter object
        /// </summary>
        public IBaseFilter SrcFilter
        {
            get
            {
                if (srcFilter == null)
                {
                    GlobalConfig cfg = GlobalConfig.Instance;
                    IGraphBuilder graph = cfg.Graph;
                    if (graph != null)
                    {
                        int hr = 0;
                        string userPass = (inputCam.userName != null && inputCam.password != null)
                                        ? inputCam.userName + ":" + inputCam.password + "@"
                                        : "";
                        string location = "http://" + userPass + inputCam.ipAddrPort + "/axis-cgi/mjpg/video.cgi?resolution="
                                        + inputCam.scanWidth + "x" + inputCam.scanLines;

                        //graph builder
                        ICaptureGraphBuilder2 pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
                        hr = pBuilder.SetFiltergraph(graph);
                        VgvUtil.checkHR(hr,"Can't SetFiltergraph");

                        //add Alax.Info JPEG Video Source
                        Guid CLSID_VideoSource = new Guid("{A8DA2ECB-DEF6-414D-8CE2-E651640DBA4F}");    // IpVideoSource.dll
                        rawFilter = (IJpegVideoSourceFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoSource));
                        srcFilter = rawFilter as IBaseFilter;
                        hr = (graph as IFilterGraph2).AddFilter(srcFilter,"Alax.Info JPEG Video Source");
                        VgvUtil.checkHR(hr,"Can't add Alax.Info JPEG Video Source to graph");

                        rawFilter.Location = location;
                        rawFilter.Width = inputCam.scanWidth;
                        rawFilter.Height = inputCam.scanLines;
                    }
                }
                return srcFilter;
            }
        }

        WndPreview wndPreview = null;

        // --------------------------------------------------------------------
        // constructor

        /// <summary>
        /// construct a camera source
        /// </summary>
        /// <param name="cam">CameraObject instance</param>
        public CameraVgvVideoSrc(CameraObject cam)
        {
            inputCam = cam;

            // TODO
            // MessageBox.Show("camera source object created");
        }

        // --------------------------------------------------------------------
        // query configuration

        /// <summary>
        /// returns the type of input that is configured
        /// </summary>
        /// <returns>string, "camera"</returns>
        public override string getType()
        {
            return "camera";
        }
        /// <summary>
        /// returns the camera name and its IP address/port
        /// 
        /// If no input is configured, it will be "no camera source"
        /// </summary>
        /// <returns>string, input name</returns>
        public override string getId()
        {
            return (inputCam == null)
                ? "no camera source"
                : inputCam.cameraName + " @ " + inputCam.ipAddrPort;
        }

        // --------------------------------------------------------------------
        // operational controls

        /// <summary>
        /// opens a preview window for this video source
        /// 
        /// If a preview window was already opened for this video source, it is
        /// made visible and brought to the top of the Z stack.
        /// 
        /// This method should be overridden in derived classes.  The derived method
        /// should call this base implementation for the core window functionality.
        /// </summary>
        /// <returns>bool, true=preview opened successfully</returns>
        public override bool preview()
        {
            // TODO: open the camera in a preview window
            wndPreview = new WndPreview(/*SrcFilter,*/inputCam);
            wndPreview.Show();

            return base.preview();     // bring visible window to top of Z stack
        }

        /// <summary>
        /// shows or hides the output of this video source when it's an overlay
        /// If this video source is NOT an overlay, the returned value will always
        /// be the same as the passed one:  "show" succeeds, "hide" fails
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <param name="show">bool, true=show the overlay, false=hide it</param>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool show(bool show)
        {
            if (!bOverlay)
                return show;

            // TODO

            return false;
        }

        /// <summary>
        /// starts the output from this video source
        /// 
        /// If the video source was previously paused, it will resume from where
        /// it left off.  Otherwise, output starts from the beginning of the stream.
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool start()
        {
            if (bPaused)
            {
                // TODO
                bPaused = false;
            }
            else
            {
                // TODO
            }

            return false;
        }

        /// <summary>
        /// temporarily stops the video output from this source
        /// 
        /// If start() is called after the output has been paused by this method,
        /// output will resume from where it was stopped.
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool pause()
        {
            // TODO
            bPaused = true;

            return false;
        }

        /// <summary>
        /// stops output from this video source, closes any associated files or handles
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool stop()
        {
            // TODO

            return false;
        }
    }
}
//
// EOF: CameraVgvVideoSrc.cs
