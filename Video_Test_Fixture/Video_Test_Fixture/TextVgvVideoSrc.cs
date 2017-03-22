/**
 * File: TextVgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/TextVgvVideoSrc.cs,v $
 */
/**
*	Implements a text "video" source
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
    public class TextVgvVideoSrc : BaseVgvVideoSrc
    {
        protected string srcText;

        // --------------------------------------------------------------------
        // constructor

        public TextVgvVideoSrc(string text)
        {
            srcText = text;
        }
        public int setText(string text,int top,int left,int bot,int right/*color, font*/)
        {
            return -1;
        }
        public bool showText(int nText,bool show = true)
        {
            return false;
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
            // TODO: open the display in a preview window

            return base.preview();     // bring visible window to top of Z stack
        }

        /// <summary>
        /// shows or hides the output of this video source when it's an overlay
        /// If this video source is NOT an overlay, the returned value will always
        /// be the same as the passed one:  "show" succeeds, "hide" fails
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
        /// "starting" a text source is meaningless, always succeeds
        /// </summary>
        /// <returns>bool, true</returns>
        public override bool start()
        {
            return true;
        }

        /// <summary>
        /// "pausing" a text source is meaningless, always succeed
        /// </summary>
        /// <returns>bool, true</returns>
        public override bool pause()
        {
            return true;
        }

        /// <summary>
        /// "stopping" a text source is meaningless, always succeed
        /// </summary>
        /// <returns>bool, true</returns>
        public override bool stop()
        {
            return true;
        }
    }
}
//
// EOF: TextVgvVideoSrc.cs
