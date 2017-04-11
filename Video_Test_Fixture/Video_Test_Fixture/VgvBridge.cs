/**
 * File: VgvBridge.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/VgvBridge.cs,v $
 */
/**
*	Encapsulates the GMFBridge object for the VGV applications
*
*	Author:			Fred Koschara
*	Creation Date:	April seventh, 2017
*	Last Modified:	April 10, 2017 @ 4:23 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/10	wfredk	original development
*		|						|
*	2017/04/07	wfredk	original development
*/
using System;
using GMFBridgeLib;

namespace Video_Test_Fixture
{
    /// <summary>
    /// encapsulates the GMFBridge object for the VGV applications
    /// </summary>
    public class VgvBridge
    {
        private GlobalConfig cfg = null;
        private IGMFBridgeController bridge = null;

        /// <summary>
        /// Gets the bridge.
        /// </summary>
        /// <value>
        /// The bridge.
        /// </value>
        public IGMFBridgeController Bridge
        {   get
            {   return bridge;
            }
        }

        // --------------------------------------------------------------------
        /// <summary>
        /// constructor - nitializes a new instance of the <see cref="VgvBridge"/> class.
        /// </summary>
        public VgvBridge()
        {
            bridge = (IGMFBridgeController)new GMFBridgeController();
            bridge.AddStream(1,eFormatType.eMuxInputs,1);
        }

        // --------------------------------------------------------------------
        /// <summary>
        /// destructor
        /// </summary>
        ~VgvBridge()
        {
            // TODO
        }
    }
}
//
// EOF: VgvBridge.cs
