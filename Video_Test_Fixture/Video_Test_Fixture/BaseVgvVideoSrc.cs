﻿/**
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
*	Last Modified:	January 9, 2017 @ 10:22 pm
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
    public interface IVgvPosition
    {
        /// <summary>
        /// sets the position of the object
        /// 
        /// Any parameters that should be ignored must be set to -1.
        /// 
        /// If neither left nor right are set, the object will be horizontally centered.
        /// If neither top nor bot are set, the object will be vertically centered.
        /// If ht and/or wd are not set, the dimensions are calculated automatically.
        /// </summary>
        /// <param name="top">int, distance from the top margin, -1=don't care</param>
        /// <param name="left">int, distance from the left margin, -1=don't care</param>
        /// <param name="bot">int, distance from the bottom margin, -1=don't care</param>
        /// <param name="right">int, distance from the right margin, -1=don't care</param>
        /// <param name="ht">int, object height, -1=calculate automatically</param>
        /// <param name="wd">int, object width, -1=calculate automatically</param>
        /// <returns>bool, true=operation succeeded</returns>
        bool setPosition(int top,int left,int bot,int right,int ht = -1,int wd = -1);
        /// <summary>
        /// returns the position information for the object
        /// 
        /// Six integers are returned in the array:  In order, they are
        ///     top, left, bottom, right, height, width
        /// Any position values not set will be returned as -1.
        /// 
        /// N.B.:   Implementations may return null if a position has not been set.
        /// </summary>
        /// <returns>int[6], position information, or null</returns>
        int[] getPosition();
    }
    public interface IVgvVideoSrc
    {
        /// <summary>
        /// returns the last error code set on the video source
        /// </summary>
        /// <returns>VgvErrorCode, current error condition</returns>
        VgvErrorCode ErrorStatus();
        /// <summary>
        /// returns the last error string set on the video source
        /// </summary>
        /// <returns>string, error description</returns>
        string ErrorString();

        /// <summary>
        /// returns the type of input that is configured
        /// </summary>
        /// <returns>string, input type</returns>
        string getType();
        /// <summary>
        /// returns a name of the configured input
        /// </summary>
        /// <returns>string, input name</returns>
        string getId();

        /// <summary>
        /// returns the number of overlays attached to this video source
        /// </summary>
        /// <returns>int, number of overlays</returns>
        int numOverlays();
        /// <summary>
        /// returns the selected overlay for this video source
        /// 
        /// If an out-of-bounds index is passed, or there are no overlays attached
        /// to this video source, null will be returned.
        /// </summary>
        /// <param name="nOverlay">int, zero-based index of the desired overlay</param>
        /// <returns>IVgvVideoSrc, selected overlay, or null</returns>
        IVgvVideoSrc overlay(int nOverlay);

        /// <summary>
        /// shows or hides the output of this video source when it's an overlay
        /// If this video source is NOT an overlay, the returned value will always
        /// be the same as the passed one:  "show" succeeds, "hide" fails
        /// </summary>
        /// <param name="show">bool, true=show the overlay, false=hide it</param>
        /// <returns>bool, true=state changed successfully</returns>
        bool show(bool show = true);
        /// <summary>
        /// starts the output from this video source
        /// 
        /// If the video source was previously paused, it will resume from where
        /// it left off.  Otherwise, output starts from the beginning of the stream.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        bool start();
        /// <summary>
        /// temporarily stops the video output from this source
        /// 
        /// If start() is called after the output has been paused by this method,
        /// output will resume from where it was stopped.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        bool pause();
        /// <summary>
        /// stops output from this video source, closes any associated files or handles
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        bool stop();

        /// <summary>
        /// adds an overlay to this video source
        /// 
        /// Any dimensions that should be ignored must be set to -1.
        /// 
        /// If neither left nor right are set, the object will be horizontally centered.
        /// If neither top nor bot are set, the object will be vertically centered.
        /// If ht and/or wd are not set, the dimensions are calculated automatically.
        /// 
        /// N.B.:   The overlay will not be visible until its show() method is called.
        /// </summary>
        /// <param name="src">IVgvVideoSrc, video source to be overlaid onto this one</param>
        /// <param name="top">int, distance from the top margin, -1=don't care</param>
        /// <param name="left">int, distance from the left margin, -1=don't care</param>
        /// <param name="bot">int, distance from the bottom margin, -1=don't care</param>
        /// <param name="right">int, distance from the right margin, -1=don't care</param>
        /// <param name="ht">int, object height, -1=calculate automatically</param>
        /// <param name="wd">int, object width, -1=calculate automatically</param>
        /// <returns>int, zero-based overlay array index, -1=error</returns>
        int setOverlay(IVgvVideoSrc src,int top,int left,int bot,int right,int ht = -1,int wd = -1);
        /// <summary>
        /// shows or hides the selected overlay, optionally starting or stopping its output
        /// 
        /// N.B.:   If this method is called to hide a source that was previously paused and
        ///         autoStart is true, the overlay will be transitioned from paused to stopped.
        /// </summary>
        /// <param name="nOverlay">int, zero-based index of the overlay to act on</param>
        /// <param name="show">bool, true=show, false=hide</param>
        /// <param name="autoStart">bool, true=start/stop overlay, false=no overlay state change</param>
        /// <returns>bool, true=operation succeeded, false=error</returns>
        bool showOverlay(int nOverlay,bool show = true,bool autoStart=true);
    }

    public class BaseVgvVideoSrc : IVgvVideoSrc, IVgvPosition
    {
        protected bool bOverlay = false;
        protected int nOverlays=0;
        protected int nTop=-1;
        protected int nLeft=-1;
        protected int nBottom=-1;
        protected int nRight=-1;
        protected int nHeight = -1;
        protected int nWidth = -1;
        protected IVgvVideoSrc[] overlays;
        protected string errorString = "";
        protected VgvErrorCode errorStatus = VgvErrorCode.NO_ERROR;

        public VgvErrorCode ErrorStatus()
        {   return errorStatus;
        }
        public string ErrorString()
        {   return errorString;
        }

        // --------------------------------------------------------------------
        // constructor

        /// <summary>
        /// this class cannot be instantiated directly - use a derived class
        /// </summary>
        protected BaseVgvVideoSrc()
        {
        }

        // --------------------------------------------------------------------
        // query configuration

        /// <summary>
        /// returns the type of input that is configured
        /// 
        /// This method is expected to be overridden in the derived class
        /// to return the correct type name.
        /// </summary>
        /// <returns>string, "off"</returns>
        public virtual string getType()
        {
            return "off";
        }
        /// <summary>
        /// returns a name of the configured input
        /// 
        /// This method is expected to be overridden in the derived class
        /// to return the correct identification string.
        /// </summary>
        /// <returns>string, input name</returns>
        public virtual string getId()
        {
            return "no source";
        }

        /// <summary>
        /// returns the number of overlays attached to this video source
        /// </summary>
        /// <returns>int, number of overlays</returns>
        public int numOverlays()
        {
            if (overlays == null)
                return 0;

            return nOverlays;
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
            if (overlays==null || nOverlay < 0 || nOverlay > nOverlays)
                return null;
            return overlays[nOverlay];
        }

        /// <summary>
        /// returns the position information for the video source.  If this video
        /// source is not an overlay, null will be returned.
        /// 
        /// Six integers are returned in the array:  In order, they are
        ///     top, left, bottom, right, height, width
        /// Any position values not set will be returned as -1.
        /// </summary>
        /// <returns>int[6], position information, or null</returns>
        public int[] getPosition()
        {
            if (!bOverlay)
                return null;

            return new int[] { nTop,nLeft,nBottom,nRight,nHeight,nWidth };
        }

        // --------------------------------------------------------------------
        // operational controls

        /// <summary>
        /// shows or hides the output of this video source when it's an overlay
        /// If this video source is NOT an overlay, the returned value will always
        /// be the same as the passed one:  "show" succeeds, "hide" fails
        /// 
        /// This method is expected to be overridden in derived classes.
        /// </summary>
        /// <param name="show">bool, true=show the overlay, false=hide it</param>
        /// <returns>bool, true=state changed successfully</returns>
        public virtual bool show(bool show)
        {
            if (!bOverlay)
                return show;

            return false;
        }

        /// <summary>
        /// starts the output from this video source
        /// 
        /// If the video source was previously paused, it will resume from where
        /// it left off.  Otherwise, output starts from the beginning of the stream.
        /// 
        /// This method is expected to be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public virtual bool start()
        {
            return false;
        }

        /// <summary>
        /// temporarily stops the video output from this source
        /// 
        /// If start() is called after the output has been paused by this method,
        /// output will resume from where it was stopped.
        /// 
        /// This method is expected to be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public virtual bool pause()
        {
            return false;
        }

        /// <summary>
        /// stops output from this video source, closes any associated files or handles
        /// 
        /// This method is expected to be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public virtual bool stop()
        {
            return false;
        }

        // --------------------------------------------------------------------
        // overlay controls

        /// <summary>
        /// adds an overlay to this video source
        /// 
        /// Any dimensions that should be ignored must be set to -1.
        /// 
        /// If neither left nor right are set, the object will be horizontally centered.
        /// If neither top nor bot are set, the object will be vertically centered.
        /// If ht and/or wd are not set, the dimensions are calculated automatically.
        /// 
        /// N.B.:   The overlay will not be visible until its show() method is called.
        /// </summary>
        /// <param name="src">IVgvVideoSrc, video source to be overlaid onto this one</param>
        /// <param name="top">int, distance from the top margin, -1=don't care</param>
        /// <param name="left">int, distance from the left margin, -1=don't care</param>
        /// <param name="bot">int, distance from the bottom margin, -1=don't care</param>
        /// <param name="right">int, distance from the right margin, -1=don't care</param>
        /// <param name="ht">int, object height, -1=calculate automatically</param>
        /// <param name="wd">int, object width, -1=calculate automatically</param>
        /// <returns>int, zero-based overlay array index, -1=error</returns>
        public int setOverlay(IVgvVideoSrc src,int top,int left,int bot,int right,int ht=-1,int wd=-1)
        {
            // ../\.. TODO

            return -1;
        }

        /// <summary>
        /// shows or hides the selected overlay, optionally starting or stopping its output
        /// 
        /// N.B.:   If this method is called to hide a source that was previously paused and
        ///         autoStart is true, the overlay will be transitioned from paused to stopped.
        /// </summary>
        /// <param name="nOverlay">int, zero-based index of the overlay to act on</param>
        /// <param name="show">bool, true=show, false=hide</param>
        /// <param name="autoStart">bool, true=start/stop overlay, false=no overlay state change</param>
        /// <returns>bool, true=operation succeeded, false=error</returns>
        public bool showOverlay(int nOverlay,bool show,bool autoStart)
        {
            if (nOverlay < 0 || nOverlay > overlays.GetLength(0))
                return false;

            bool bRet = overlays[nOverlay].show(show);

            if (bRet && autoStart)
            {
                if (show)
                    bRet = overlays[nOverlay].start();
                else bRet = overlays[nOverlay].stop();
            }

            return bRet;
        }

        /// <summary>
        /// sets the position of the video source
        /// 
        /// Calling this method marks the video source as an overlay.
        /// 
        /// Any parameters that should be ignored must be set to -1.
        /// 
        /// If neither left nor right are set, the object will be horizontally centered.
        /// If neither top nor bot are set, the object will be vertically centered.
        /// If ht and/or wd are not set, the dimensions are calculated automatically.
        /// </summary>
        /// <param name="top">int, distance from the top margin, -1=don't care</param>
        /// <param name="left">int, distance from the left margin, -1=don't care</param>
        /// <param name="bot">int, distance from the bottom margin, -1=don't care</param>
        /// <param name="right">int, distance from the right margin, -1=don't care</param>
        /// <param name="ht">int, object height, -1=calculate automatically</param>
        /// <param name="wd">int, object width, -1=calculate automatically</param>
        /// <returns>bool, true=operation succeeded</returns>
        public bool setPosition(int top,int left,int bot,int right,int ht,int wd)
        {
            bOverlay = true;
            nTop = top;
            nLeft = left;
            nBottom = bot;
            nRight = right;
            nHeight = ht;
            nWidth = wd;
            return true;
        }
    }
}
//
// EOF: VgvVideoSrc.cs
