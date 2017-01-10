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
*	Last Modified:	January 9, 2017 @ 10:35 pm
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
