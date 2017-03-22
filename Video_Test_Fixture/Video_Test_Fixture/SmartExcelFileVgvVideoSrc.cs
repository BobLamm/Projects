/**
 * File: SmartExcelFileVgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/SmartExcelFileVgvVideoSrc.cs,v $
 */
/**
*	Implements an Excel file viewer "video" source with controls
*
*	TODO: finish implementation
*
*	Author:			Fred Koschara
*	Creation Date:	January eighth, 2017
*	Last Modified:	March 19, 2017 @ 3:33 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/03/19	wfredk	add preview() method
*	2017/01/09	wfredk	original development
*		|						|
*	2017/01/08	wfredk	original development
*/
using System;

namespace Video_Test_Fixture
{
    public class SmartExcelFileVgvVideoSrc : ExcelFileVgvVideoSrc
    {
        // --------------------------------------------------------------------
        // constructor

        public SmartExcelFileVgvVideoSrc(string filename)
            : base(filename)
        {
            // TODO
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
            // TODO: open the file in a preview window

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
        /// This method may be overridden in derived classes.  The derived method
        /// should call this base implementation for the basic file open operations.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool start()
        {
            base.start();       // open the file

            // TODO

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
        /// This method may be overridden in derived classes.  The derived method
        /// should call this base implementation for the basic file close operations.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool stop()
        {
            // TODO

            base.stop();        // close the file

            return false;
        }

        public bool gotoCell(int x,int y)
        {
            // TODO
            return false;
        }
        public bool changeSheet(int nSheet)
        {
            // TODO
            return false;
        }

        // TODO: more functions?
    }
}
//
// EOF: SmartExcelFileVgvVideoSrc.cs
