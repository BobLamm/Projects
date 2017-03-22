/**
 * File: VgvUtility.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/VgvUtility.cs,v $
 */
/**
*	Utility functions for use in the VGV code
*
*	Author:			Fred Koschara
*	Creation Date:	March nineteenth, 2017
*	Last Modified:	March 19, 2017 @ 4:47 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/03/19	wfredk	original development
*/
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DirectShowLib;

namespace Utility.VgvUtility
{
    class VgvUtil
    {
        static public void checkHR(int hr,string msg)
        {
            if (hr < 0)
            {
                MessageBox.Show(msg);
                //Console.WriteLine(msg);
                DsError.ThrowExceptionForHR(hr);
            }
        }

/*
        static public int EnumFilters(IFilterGraph graph)
        {
              = null;
            IEnumFilters eFilters = null;
            int hr;

            hr = graph.EnumFilters(out eFilters);
            checkHR(hr,"Can't enumerate filters");

            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IBaseFilter[] filter = new IBaseFilter[1];
            while (eFilters.Next(1,filter,fetched) == 0)
            {
                FilterInfo fInfo;
                filter[0].QueryFilterInfo(out fInfo);
                bool found = (pinfo.name == pinname);
                DsUtils.FreeFilterInfo(fInfo);
                if (found)
                    return pins[0];
            }

            return 0;
        }
*/

        static public IPin GetPin(IBaseFilter filter,string pinname)
        {
            IEnumPins epins;
            int hr = filter.EnumPins(out epins);
            checkHR(hr,"Can't enumerate pins");
            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IPin[] pins = new IPin[1];
            while (epins.Next(1,pins,fetched) == 0)
            {
                PinInfo pinfo;
                pins[0].QueryPinInfo(out pinfo);
                bool found = (pinfo.name == pinname);
                DsUtils.FreePinInfo(pinfo);
                if (found)
                    return pins[0];
            }
            checkHR(-1,"Pin not found");
            return null;
        }

        static public string TerminatePath(string path)
        {
            if (!path.EndsWith(@"\"))
                return path + @"\";

            return path;
        }
    }
}
//
// EOF: VgvUtility.cs
