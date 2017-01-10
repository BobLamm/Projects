/**
 * File: ImgFileVgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/ImgFileVgvVideoSrc.cs,v $
 */
/**
*	Implements an image file viewer "video" source
*
*	TODO: finish implementation
*
*	Author:			Fred Koschara
*	Creation Date:	January eighth, 2017
*	Last Modified:	January 9, 2017 @ 11:28 pm
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
    public class ImgFileVgvVideoSrc : BaseFileVgvVideoSrc
    {
        // --------------------------------------------------------------------
        // constructor

        public ImgFileVgvVideoSrc(string filename)
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
        /// "starting" an image is the same as opening the file
        /// </summary>
        /// <returns>bool, true</returns>
        public override bool start()
        {
            return base.start();       // open the file
        }

        /// <summary>
        /// "pausing" an image is meaningless, always succeed
        /// </summary>
        /// <returns>bool, true</returns>
        public override bool pause()
        {
            return true;
        }

        /// <summary>
        /// "stopping" an image is the same as closing the file
        /// </summary>
        /// <returns>bool, true</returns>
        public override bool stop()
        {
            return base.stop();        // close the file
        }
    }
}
//
// EOF: ImgFileVgvVideoSrc.cs
