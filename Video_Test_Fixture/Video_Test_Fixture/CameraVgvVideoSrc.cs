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
*	Last Modified:	January 9, 2017 @ 11:18 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/01/09	wfredk	original development
*		|						|
*	2017/01/08	wfredk	original development
*/
using System;

namespace Video_Test_Fixture
{
    public class CameraVgvVideoSrc : BaseVgvVideoSrc
    {
        protected bool bPaused = false;
        protected CameraObject inputCam;

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
