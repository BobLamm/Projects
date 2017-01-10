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
*	Last Modified:	January 8, 2017 @ 9:34 am
*
*	Revision History:
*	   Date		  by		Description
*	2017/01/08	wfredk	original development
*		|						|
*	2017/01/08	wfredk	original development
*/
using System;

namespace Video_Test_Fixture
{
    public enum VgvErrorCode : int
    {
        NO_ERROR = 0,
        INPUT_ALREADY_SET,
        NO_INPUT_CONFIGURED,
        END_OF_FILE,
        NO_SIGNAL
    }
}
//
// EOF: VgvErrorCode.cs
