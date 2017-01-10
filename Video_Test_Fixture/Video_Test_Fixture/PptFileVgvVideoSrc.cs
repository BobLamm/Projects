/**
 * File: PptFileVgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/PptFileVgvVideoSrc.cs,v $
 */
/**
*	Implements a PowerPoint file "video" source
*
*	TODO: finish implementation
*
*	Author:			Fred Koschara
*	Creation Date:	January eighth, 2017
*	Last Modified:	January 9, 2017 @ 11:21 pm
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
    public class PptFileVgvVideoSrc : BaseFileVgvVideoSrc
    {
        protected int currSlide = 0;
        protected int nSlides;

        // --------------------------------------------------------------------
        // constructor

        public PptFileVgvVideoSrc(string filename)
            : base(filename)
        {
            // TODO
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
        /// This method may be overridden in derived classes.  The derived method
        /// should call this base implementation for the basic file open operations.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool start()
        {
            base.start();       // open the file

            // TODO
            nSlides = 0;

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

        public bool gotoSlide(int nSlide)
        {
            // TODO

            return false;
        }
        public bool firstSlide()
        {
            return gotoSlide(0);
        }
        public bool lastSlide()
        {
            return gotoSlide(nSlides-1);
        }
        public bool nextSlide()
        {
            if (currSlide == nSlides-1)
                return false;
            return gotoSlide(++currSlide);
        }
        public bool prevSlide()
        {
            if (currSlide == 0)
                return false;
            return gotoSlide(--currSlide);
        }

        public bool run()
        {
            bPaused = false;
            return false;
        }
        public bool run(int nDelay)
        {
            bPaused = false;
            return false;
        }
    }
}
//
// EOF: PptFileVgvVideoSrc.cs
