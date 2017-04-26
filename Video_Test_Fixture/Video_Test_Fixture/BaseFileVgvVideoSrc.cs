/**
 * File: BaseFileVgvVideoSrc.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/BaseFileVgvVideoSrc.cs,v $
 */
/**
*	Provides a common interface for file-based video sources
*
*	TODO: finish implementation
*
*	Author:			Fred Koschara
*	Creation Date:	January eighth, 2017
*	Last Modified:	April 26, 2017 @ 11:53 am
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/26	wfredk	corrected internal documentation
*	2017/04/22	wfredk	add default buildGraph(), preview() implementations
*	2017/03/19	wfredk	add preview() method
*	2017/01/09	wfredk	original development
*		|						|
*	2017/01/08	wfredk	original development
*/
using System;

namespace Video_Test_Fixture
{
    /// <summary>
    /// provides a common interface for file-based video sources
    /// </summary>
    /// <seealso cref="Video_Test_Fixture.BaseVgvVideoSrc" />
    public class BaseFileVgvVideoSrc : BaseVgvVideoSrc
    {
        /// <summary>
        /// set if this video source is paused
        /// </summary>
        protected bool bPaused=false;
        /// <summary>
        /// the input file
        /// </summary>
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

        /// <summary>
        /// Builds the graph.
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override void buildGraph()
        {
            throw new NotImplementedException();
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
        /// temporarily stops the video output from this source
        /// 
        /// If start() is called after the output has been paused by this method,
        /// output will resume from where it was stopped.
        /// 
        /// This method may be overridden in derived classes.
        /// 
        /// N.B. This method is a no-op for most file types, and returns false.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool pause()
        {
            // no-op for most file types
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
// EOF: BaseFileVgvVideoSrc.cs
