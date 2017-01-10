/**
 * File: VgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/VgvVideoSrc.cs,v $
 */
/**
*	Provides a common interface for video sources
*
*	Author:			Fred Koschara
*	Creation Date:	January fourth, 2017
*	Last Modified:	January 9, 2017 @ 6:39 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/01/09	wfredk	original development
*		|						|
*	2017/01/04	wfredk	original development
*/
using System;

namespace Video_Test_Fixture
{
    public interface IVgvVideoSrc
    {
        VgvErrorCode ErrorStatus();
        string ErrorString();

        bool input(IVgvVideoSrc src,bool bForce = false);

        string getType();
        string getId();

        int numOverlays();
        IVgvVideoSrc overlay(int nOverlay);

        bool start();
        bool pause();
        bool stop();
        int setOverlay(IVgvVideoSrc src,int top,int left,int bot,int right,int ht = -1,int wd = -1);
        bool showOverlay(int nOverlay,bool show = true,bool autoStart=true);
    }

    public class BaseVgvVideoSrc : IVgvVideoSrc
    {
        protected IVgvVideoSrc inputSrc;
        protected int nOverlays;
        protected IVgvVideoSrc[] overlays;
        private CameraObject inputCam;
        private string inputFile;

        private VgvErrorCode errorStatus = VgvErrorCode.NO_ERROR;
        public VgvErrorCode ErrorStatus()
        {   return errorStatus;
        }
        private string errorString = "";
        public string ErrorString()
        {   return errorString;
        }

        // --------------------------------------------------------------------
        // constructors

        public BaseVgvVideoSrc()
        {
        }
        /// <summary>
        /// construct a camera source
        /// </summary>
        /// <param name="cam">CameraObject instance</param>
        public BaseVgvVideoSrc(CameraObject cam)
        {   inputCam = cam;
        }
        /// <summary>
        /// construct a file source
        /// </summary>
        /// <param name="filename">string, filespec of the source</param>
        public BaseVgvVideoSrc(string filename)
        {   inputFile = filename;
        }

        // --------------------------------------------------------------------
        // configure inputs

        public virtual bool input(IVgvVideoSrc src,bool bForce = false)
        {
            if (inputSrc != null)
            {
                if (bForce)
                {
                    // close camera connection or file, if open
                    inputSrc = null;
                }
                else
                {
                    errorStatus = VgvErrorCode.INPUT_ALREADY_SET;
                    errorString = "Input source is already set";
                    return false;
                }
            }
            inputSrc = src;
            return true;
        }
        /// <summary>
        /// set input source to camera
        /// </summary>
        /// <param name="cam">CameraObject instance</param>
        /// <param name="bForce">boolean, true=replace input if already set</param>
        /// <returns>boolean, true=success</returns>
        public bool input(CameraObject cam,bool bForce=false)
        {    if (inputCam!=null || inputFile!=null)
            {   if (bForce)
                {
                    // close camera connection or file, if open
                    inputCam = null;
                    inputFile = null;
                }
                else
                {   errorStatus = VgvErrorCode.INPUT_ALREADY_SET;
                    errorString = "Input source is already set";
                    return false;
                }
            }
            inputCam = cam;
            return true;
        }

        /// <summary>
        /// set input source to file
        /// </summary>
        /// <param name="filename">string, filespec of the source</param>
        /// <param name="bForce">boolean, true=replace input if already set</param>
        /// <returns>boolean, true=success</returns>
        public bool input(string filename,bool bForce=false)
        {    if (inputCam != null || inputFile != null)
            {   if (bForce)
                {
                    // close camera connection or file, if open
                    inputCam = null;
                    inputFile = null;
                }
                else
                {    errorStatus = VgvErrorCode.INPUT_ALREADY_SET;
                    errorString = "Input source is already set";
                    return false;
                }
            }
            inputFile = filename;
            return true;
        }

        // --------------------------------------------------------------------
        // query configuration

        /// <summary>
        /// returns the type of input that is configured
        /// </summary>
        /// <returns>string, "camera" "file" or "off"</returns>
        public virtual string getType()
        {
            if (inputCam != null)
                return "camera";
            else if (inputFile != null)
                return "file";
            return "off";
        }
        /// <summary>
        /// returns a name of the configured input
        /// 
        /// For cameras, this will be the camera name and its IP address/port
        /// For files, it will be the filespec
        /// If no input is configured, it will be "no source"
        /// </summary>
        /// <returns>string, input name</returns>
        public virtual string getId()
        {
            if (inputCam != null)
                return inputCam.cameraName+" @ "+inputCam.ipAddrPort;
            else if (inputFile != null)
                return inputFile;
            return "no source";
        }

        /// <summary>
        /// returns the number of overlays attached to this video source
        /// </summary>
        /// <returns>int, number of overlays</returns>
        public int numOverlays()
        {
            try
            {
                return overlays.GetLength(0);
            }
            catch(Exception e)
            {
                e.ToString();
                return 0;
            }
        }
        /// <summary>
        /// returns the selected overlay for this video source
        /// 
        /// If an out-of-bounds index is passed, or there are no overlays attached
        /// to this video source, null will be returned.
        /// </summary>
        /// <param name="nOverlay">int, zero-based index of the desired overlay</param>
        /// <returns>IVgvVideoSrc, selected overlay, or null</returns>
        public IVgvVideoSrc overlay(int nOverlay)
        {
            if (nOverlay < 0 || nOverlay > overlays.GetLength(0))
                return null;
            return overlays[nOverlay];
        }

        // --------------------------------------------------------------------
        // operational controls

        public virtual bool start()
        {
            return false;
        }
        public virtual bool pause()
        {
            return false;
        }
        public virtual bool stop()
        {
            return false;
        }
        public int setOverlay(IVgvVideoSrc src,int top,int left,int bot,int right,int ht=-1,int wd=-1)
        {
            return -1;
        }
        public bool showOverlay(int nOverlay,bool show,bool autoStart)
        {
            return false;
        }
        public int setText(string text, int top, int left, int bot, int right/*color, font*/)
        {
            return -1;
        }
        public bool showText(int nText, bool show = true)
        {
            return false;
        }
    }
}
//
// EOF: VgvVideoSrc.cs
