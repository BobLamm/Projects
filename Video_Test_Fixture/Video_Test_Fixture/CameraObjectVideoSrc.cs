/**
 * File: CameraObjectVideoSrc.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/CameraObjectVideoSrc.cs,v $
 */
/**
*	Implements the BaseVgvVideoSrc derivation parts of the CameraObject class
*
*	Author:			Fred Koschara
*	Creation Date:	April nineteenth, 2017
*	Last Modified:	April 22, 2017 @ 4:52 am
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/22	wfredk	original development
*		|						|
*	2017/04/19	wfredk	original development, extracted from CameraObject.cs
*/
using System;
//using System.Runtime.InteropServices;
using System.Windows.Forms;

using Utility.VgvUtility;

using AlaxInfoIpVideoSource;
using DirectShowLib;

namespace Video_Test_Fixture
{
    partial class CameraObject
    {
        // --------------------------------------------------------------------
        // video filter graph properties, BaseVgvVideoSrc derivation

        /// <summary>
        /// bool, set if the camera is in a paused state
        /// </summary>
        protected bool bPaused = false;

        /// <summary>
        /// The source filter
        /// </summary>
        protected IBaseFilter srcFilter = null;

        /// <summary>
        /// The capture output filter
        /// </summary>
        protected IBaseFilter outFilter = null;

        private IBaseFilter pCaptureColorSpaceConverter = null;
        private IJpegVideoSourceFilter rawFilter = null;
        private VgvBridge previewBridge = null;

        /// <summary>
        /// The preview window
        /// </summary>
        protected WndMonitor wndPreview = null;

        // --------------------------------------------------------------------
        // video filter graph properties

        /// <summary>
        /// property (accessor) for this camera's source filter object
        /// </summary>
        public IBaseFilter SrcFilter
        {   get
            {   if (srcFilter == null)
                {   if (graph != null)
                    {   int hr = 0;
                        string userPass = (userName != null && password != null)
                                        ? userName + ":" + password + "@"
                                        : "";
                        string location = "http://" + userPass + ipAddrPort + "/axis-cgi/mjpg/video.cgi?resolution="
                                        + scanWidth + "x" + scanLines;

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
                        rawFilter.Width = scanWidth;
                        rawFilter.Height = scanLines;
                    }
                }
                return srcFilter;
            }
        }

        /// <summary>
        /// OutFilter
        /// </summary>
        public IBaseFilter OutFilter
        {
            get
            {
                return outFilter;
            }
        }

        // --------------------------------------------------------------------
        // video filter graph methods

        /// <summary>
        /// connects this graph as the source for a bridge in the downstream graph
        /// </summary>
        /// <param name="bridge"></param>
        /// <returns></returns>
        public IBaseFilter connectSrcGraph(VgvBridge bridge)
        {
            if (outFilter != null)
                return outFilter;
            try
            {
                outFilter = bridge.ConnectInput(Graph,VgvUtil.GetPin(pCaptureColorSpaceConverter,"XForm Out"));
            }
            catch (Exception ex)
            {
                errorStatus = VgvErrorCode.FAILED_CREATE_BRIDGE_FILTER;
                errorString = "Create sink filter for " + cameraName + "failed: " + ex.ToString();
                outFilter = null;
            }
            return outFilter;
        }

        // --------------------------------------------------------------------
        // internal methods

        /// <summary>
        /// Builds the graph.
        /// </summary>
        protected override void buildGraph()
        {
            try
            {
                int hr;

                // graph builder
                ICaptureGraphBuilder2 pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
                hr = pBuilder.SetFiltergraph(graph);
                VgvUtil.checkHR(hr,"Can't SetFiltergraph");

                // add Smart Tee
                IBaseFilter pSmartTee = (IBaseFilter)new SmartTee();
                hr = graph.AddFilter(pSmartTee,"Smart Tee");
                VgvUtil.checkHR(hr,"Can't add Smart Tee to graph");

                // connect Alax.Info JPEG Video Source and Smart Tee
                hr = graph.ConnectDirect(VgvUtil.GetPin(SrcFilter,"Output"),
                                         VgvUtil.GetPin(pSmartTee,"Input"),
                                         null);
                VgvUtil.checkHR(hr,"Can't connect Alax.Info JPEG Video Source and Smart Tee");

                // add capture Color Space Converter
                pCaptureColorSpaceConverter = (IBaseFilter)new Colour();
                hr = graph.AddFilter(pCaptureColorSpaceConverter,"Capture Color Space Converter");
                VgvUtil.checkHR(hr,"Can't add capture Color Space Converter to graph");

                // connect Smart Tee and capture Color Space Converter
                hr = graph.ConnectDirect(VgvUtil.GetPin(pSmartTee,"Capture"),
                                         VgvUtil.GetPin(pCaptureColorSpaceConverter,"Input"),
                                         null);
                VgvUtil.checkHR(hr,"Can't connect Smart Tee and capture Color Space Converter");

                // capture CSC will be connected to output bridge later in connectSrcGraph() call

                // add preview Color Space Converter
                IBaseFilter pPreviewColorSpaceConverter = (IBaseFilter)new Colour();
                hr = graph.AddFilter(pPreviewColorSpaceConverter,"Preview Color Space Converter");
                VgvUtil.checkHR(hr,"Can't add preview Color Space Converter to graph");

                // connect Smart Tee and preview Color Space Converter
                hr = graph.ConnectDirect(VgvUtil.GetPin(pSmartTee,"Preview"),
                                         VgvUtil.GetPin(pPreviewColorSpaceConverter,"Input"),
                                         null);
                VgvUtil.checkHR(hr,"Can't connect Smart Tee and preview Color Space Converter");

                previewBridge = new VgvBridge(graph,VgvUtil.GetPin(pPreviewColorSpaceConverter,"XForm Out"));
            }
            catch (Exception ex)
            {
//              Console.WriteLine("Error: " + ex.ToString());
                MessageBox.Show(ex.ToString(),"ERROR");
            }
        }

        // --------------------------------------------------------------------
        // query configuration

        /// <summary>
        /// returns the type of input that is configured
        /// </summary>
        /// <returns>string, "camera"</returns>
        public override string getType()
        {   return "camera";
        }
        /// <summary>
        /// returns the camera name and its IP address/port
        /// 
        /// If no input is configured, it will be "no camera source"
        /// </summary>
        /// <returns>string, input name</returns>
        public override string getId()
        {   return cameraName + " @ " + ipAddrPort;
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
            wndPreview = new WndMonitor(previewBridge);
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
// EOF: CameraObjectVideoSrc.cs
