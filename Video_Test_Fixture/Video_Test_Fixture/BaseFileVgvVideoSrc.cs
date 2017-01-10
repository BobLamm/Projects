/**
 * File: FileVgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/FileVgvVideoSrc.cs,v $
 */
/**
*	Provides a common interface for file-based video sources
*
*	TODO: finish implementation
*
*	Author:			Fred Koschara
*	Creation Date:	January eighth, 2017
*	Last Modified:	January 9, 2017 @ 11:16 pm
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
    public class BaseFileVgvVideoSrc : BaseVgvVideoSrc
    {
        protected bool bPaused=false;
        protected string inputFile;

        // --------------------------------------------------------------------
        // constructor

        /// <summary>
        /// construct a file video source
        /// </summary>
        /// <param name="filename">string, filespec of the source</param>
        protected BaseFileVgvVideoSrc(string filename)
        {
            inputFile = filename;

            // TODO
        }

        // --------------------------------------------------------------------
        // query configuration

        /// <summary>
        /// returns the type of input that is configured
        /// </summary>
        /// <returns>string, "file"</returns>
        public override string getType()
        {
            return "file";
        }
        /// <summary>
        /// returns the filespec configured as the input
        ///
        /// If no input is configured, it will be "no file source"
        /// </summary>
        /// <returns>string, input name</returns>
        public override string getId()
        {
            return (inputFile == null) ? "no file source" : inputFile;
        }

        // --------------------------------------------------------------------
        // operational controls

        /// <summary>
        /// starts the output from this video source
        /// 
        /// If the video source was previously paused, it will resume from where
        /// it left off.  Otherwise, output starts from the beginning of the stream.
        /// 
        /// This method should be overridden in derived classes.  The derived method
        /// should call this base implementation for the basic file open operations.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool start()
        {
            if (bPaused)
            {   bPaused = false;
                return true;
            }

            // TODO: open the file
            bPaused = false;

            return false;
        }

        /// <summary>
        /// stops output from this video source, closes any associated files or handles
        /// 
        /// This method should be overridden in derived classes.  The derived method
        /// should call this base implementation for the basic file close operations.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool stop()
        {
            // TODO: close the file

            return false;
        }
    }
}
//
// EOF: FileVgvVideoSrc.cs
