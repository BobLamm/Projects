/**
 * File: VgvErrorCode.cs
 * 
 *	Copyright © 2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/VgvErrorCode.cs,v $
 */
/**
*	Error code values used in the VGV software
*
*	Author:			Fred Koschara
*	Creation Date:	January eighth, 2017
*	Last Modified:	April 19, 2017 @ 10:00 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/19	wfredk	original development
*		|						|
*	2017/01/08	wfredk	original development
*/
using System;

namespace Video_Test_Fixture
{
    /// <summary>
    /// error code values used in the VGV software
    /// </summary>
    public enum VgvErrorCode : int
    {
        /// <summary>
        /// no error
        /// </summary>
        NO_ERROR = 0,
        /// <summary>
        /// input already set
        /// </summary>
        INPUT_ALREADY_SET,
        /// <summary>
        /// no input configured
        /// </summary>
        NO_INPUT_CONFIGURED,
        /// <summary>
        /// end of file encountered
        /// </summary>
        END_OF_FILE,
        /// <summary>
        /// no signal present
        /// </summary>
        NO_SIGNAL,
        /// <summary>
        /// camera command failed
        /// </summary>
        FAILED_CAM_COMMAND,
        /// <summary>
        /// add datatable row failed
        /// </summary>
        FAILED_ADD_DATATABLE_ROW,
        /// <summary>
        /// create bridge filter failed
        /// </summary>
        FAILED_CREATE_BRIDGE_FILTER
    }
}
//
// EOF: VgvErrorCode.cs
