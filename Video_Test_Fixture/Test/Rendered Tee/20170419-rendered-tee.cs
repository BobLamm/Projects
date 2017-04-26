//Don't forget to add reference to DirectShowLib in your project.
using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using AlaxInfoIpVideoSource;
using DirectShowLib;

namespace graphcode
{
    class Program
    {
        IGraphBuilder graph = null;
        IJpegVideoSourceFilter rawFilter = null;
        IMediaControl mediaControl = null;
        IMediaEvent mediaEvent = null;
        IVideoWindow captureWindow = null;
        IVideoWindow previewWindow = null;

        static void checkHR(int hr, string msg)
        {
            if (hr < 0)
            {
                Console.WriteLine(msg);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        static IPin GetPin(IBaseFilter filter,string pinname)
        {
            IEnumPins epins;
            int hr = filter.EnumPins(out epins);
            checkHR(hr,"Can't enumerate pins");
            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IPin[] pins = new IPin[1];
            while (epins.Next(1,pins,fetched) == 0)
            {
                PinInfo pinfo;
                pins[0].QueryPinInfo(out pinfo);
                bool found = (pinfo.name == pinname);
                DsUtils.FreePinInfo(pinfo);
                if (found)
                    return pins[0];
            }
            checkHR(-1,"Pin not found");
            return null;
        }

        void BuildGraph()
        {
            int hr = 0;

            //graph builder
            ICaptureGraphBuilder2 pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            hr = pBuilder.SetFiltergraph(graph);
            checkHR(hr, "Can't SetFiltergraph");

            Guid CLSID_AlaxInfoJPEGVideoSource = new Guid("{A8DA2ECB-DEF6-414D-8CE2-E651640DBA4F}"); //IpVideoSource.dll
            Guid CLSID_VideoRenderer = new Guid("{6BC1CFFA-8FC1-4261-AC22-CFB4CC38DB50}"); //quartz.dll

            //add Alax.Info JPEG Video Source
            Guid CLSID_VideoSource = new Guid("{A8DA2ECB-DEF6-414D-8CE2-E651640DBA4F}");    // IpVideoSource.dll
            rawFilter = (IJpegVideoSourceFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoSource));
            IBaseFilter pAlaxInfoJPEGVideoSource = rawFilter as IBaseFilter;
            hr = graph.AddFilter(pAlaxInfoJPEGVideoSource, "Alax.Info JPEG Video Source");
            checkHR(hr, "Can't add Alax.Info JPEG Video Source to graph");

            rawFilter.Location = "http://100.0.193.98:1026/axis-cgi/mjpg/video.cgi?resolution=640x360";
            rawFilter.Width = 640;
            rawFilter.Height = 360;

            //add Smart Tee
            IBaseFilter pSmartTee = (IBaseFilter) new SmartTee();
            hr = graph.AddFilter(pSmartTee, "Smart Tee");
            checkHR(hr, "Can't add Smart Tee to graph");

            //add Color Space Converter
            IBaseFilter pColorSpaceConverter = (IBaseFilter) new Colour();
            hr = graph.AddFilter(pColorSpaceConverter, "Color Space Converter");
            checkHR(hr, "Can't add Color Space Converter to graph");

            //add Color Space Converter
            IBaseFilter pColorSpaceConverter2 = (IBaseFilter) new Colour();
            hr = graph.AddFilter(pColorSpaceConverter2, "Color Space Converter");
            checkHR(hr, "Can't add Color Space Converter to graph");

            //add Video Renderer
            IBaseFilter pPreviewRenderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoRenderer));
            hr = graph.AddFilter(pPreviewRenderer, "Preview Video Renderer");
            checkHR(hr, "Can't add Video Renderer to graph");
            previewWindow = pPreviewRenderer as IVideoWindow;

            //add Video Renderer
            IBaseFilter pCaptureRenderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoRenderer));
            hr = graph.AddFilter(pCaptureRenderer, "Capture Video Renderer");
            checkHR(hr, "Can't add Video Renderer to graph");
            captureWindow = pCaptureRenderer as IVideoWindow;

            //connect Alax.Info JPEG Video Source and Smart Tee
            hr = graph.ConnectDirect(GetPin(pAlaxInfoJPEGVideoSource, "Output"), GetPin(pSmartTee, "Input"), null);
            checkHR(hr, "Can't connect Alax.Info JPEG Video Source and Smart Tee");

            //connect Smart Tee and Color Space Converter
            hr = graph.ConnectDirect(GetPin(pSmartTee, "Capture"), GetPin(pColorSpaceConverter, "Input"), null);
            checkHR(hr, "Can't connect Smart Tee and Color Space Converter");

            //connect Color Space Converter and Video Renderer
            hr = graph.ConnectDirect(GetPin(pColorSpaceConverter, "XForm Out"), GetPin(pCaptureRenderer, "VMR Input0"), null);
            checkHR(hr, "Can't connect Color Space Converter and Video Renderer");

            //connect Smart Tee and Video Renderer
            // hr = graph.ConnectDirect(GetPin(pSmartTee,"Capture"),GetPin(pCaptureRenderer,"VMR Input0"),null);
            // checkHR(hr,"Can't connect Smart Tee and Video Renderer");
            // Can't connect Smart Tee and Video Renderer
            // COM error: System.Runtime.InteropServices.COMException(0x80040207): There is no common media type between these pins.

            //connect Smart Tee and Color Space Converter
            hr = graph.ConnectDirect(GetPin(pSmartTee, "Preview"), GetPin(pColorSpaceConverter2, "Input"), null);
            checkHR(hr, "Can't connect Smart Tee and Color Space Converter");

            //connect Color Space Converter and Video Renderer
            hr = graph.ConnectDirect(GetPin(pColorSpaceConverter2, "XForm Out"), GetPin(pPreviewRenderer, "VMR Input0"), null);
            checkHR(hr, "Can't connect Color Space Converter and Video Renderer");

        }

        Program()
        {
            graph = (IGraphBuilder)new FilterGraph();
            mediaControl = (IMediaControl)graph;
            mediaEvent = (IMediaEvent)graph;
        }

        static void Main(string[] args)
        {
            bool stop = false;
            EventCode ev;
            int hr;
            IntPtr p1, p2;

            try
            {
                Program theClass = new Program();

                Console.WriteLine("Building graph...");
                theClass.BuildGraph();

                Console.WriteLine("Running...");
                hr = theClass.mediaControl.Run();
                checkHR(hr, "Can't run the graph");

                theClass.previewWindow.put_Caption("Preview");
                theClass.captureWindow.put_Caption("Capture");

                while (!stop)
                {
                    System.Threading.Thread.Sleep(500);
                    Console.Write(".");

                    System.Windows.Forms.Application.DoEvents();
                    while (theClass.mediaEvent.GetEvent(out ev, out p1, out p2, 0) == 0)
                    {
                        if (ev == EventCode.Complete || ev == EventCode.UserAbort)
                        {
                            Console.WriteLine("Done!");
                            stop = true;
                        }
                        else if (ev == EventCode.ErrorAbort)
                        {
                            Console.WriteLine("An error occured: HRESULT={0:X}", p1);
                            theClass.mediaControl.Stop();
                            stop = true;
                        }
                        theClass.mediaEvent.FreeEventParams(ev, p1, p2);
                    }
                }
            }
            catch (COMException ex)
            {
                Console.WriteLine("COM error: " + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
    }
} 
