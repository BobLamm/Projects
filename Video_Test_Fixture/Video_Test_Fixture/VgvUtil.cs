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
*	Last Modified:	April 22, 2017 @ 2:38 am
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/22	wfredk	add ListPins() method
*	2017/03/22	wfredk	finish EnumFilters() implementation
*	                    add ReleaseFilters() method
*	                    add documentation
*	2017/03/21	wfredk	work on implementing EnumFilters()
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
        /// <summary>
        /// checks handles returned by graph operations for errors
        /// 
        /// An error is indicated by a negative handle value.
        /// 
        /// If an error is detected, this method throws a DsERror exception
        /// to terminate the calling function's operaation.
        /// </summary>
        /// <param name="hr">int, handle returned by a graph operation</param>
        /// <param name="msg">string, message displayed on error</param>
        static public void checkHR(int hr,string msg)
        {
            if (hr < 0)
            {
                MessageBox.Show(msg,"Error Encountered");
                //Console.WriteLine(msg);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        /// <summary>
        /// experimental method for displaying information about filters in a graph
        /// </summary>
        /// <param name="graph">IFilterGraph, the graph to be examined</param>
        /// <returns></returns>
        static public int EnumFilters(IFilterGraph graph)
        {
            IEnumFilters eFilters = null;
            int hr;

            hr = graph.EnumFilters(out eFilters);
            checkHR(hr,"Can't enumerate filters");

            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IBaseFilter[] filter = new IBaseFilter[1];
            while (eFilters.Next(1,filter,fetched) == 0)
            {
                FilterInfo fInfo;
                if ((hr = filter[0].QueryFilterInfo(out fInfo)) < 0)
                {
                    MessageBox.Show("Could not get the filter info","ERROR");
                    continue;   // maybe the next one will work
                }
                FilterState state;
                Guid guid;
                filter[0].GetClassID(out guid);
                filter[0].GetState(5,out state);
                MessageBox.Show("Filter Name: "+fInfo.achName+"\r\n"
                                +"fInfo ToString(): "+fInfo.ToString()+"\r\n"   // USELESS: always returns "DirectShowLib.FilterInfo"
                                +"Filter ToString(): "+filter[0].ToString()+"\r\n"
                                +"Filter CLSID: "+guid+"\r\n"
                                +"Filter Type: "+filter[0].GetType()+"\r\n"
                                +"Filter State: "+state.ToString()
                                ,"Filter Details");
                // The FILTER_INFO structure holds a pointer to the Filter Graph
                // Manager, with a reference count that must be released.
                if (fInfo.pGraph != null)
                {
                    fInfo.pGraph = null;
                }
                filter[0] = null;
            }

            return 0;
        }

        /// <summary>
        /// finds a pin with the given name on the passed filter
        /// 
        /// If a matching pin cannot be found, a MessageBox is displayed and
        /// an exception is thrown
        /// </summary>
        /// <param name="filter">IBaseFilter, the filter to be examined</param>
        /// <param name="pinname">string, the name of the pin to find</param>
        /// <returns></returns>
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
            checkHR(-1,"Pin "+pinname+" not found");
            return null;
        }

        /// <summary>
        /// Lists the pins on a filter
        /// </summary>
        /// <param name="filter">The filter.</param>
        static public void ListPins(IBaseFilter filter)
        {
            IEnumPins epins;
            int cnt = 0;
            int ctr = 0;
            int hr = filter.EnumPins(out epins);
            checkHR(hr,"Can't enumerate pins");
            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IPin[] pins = new IPin[1];
            while (epins.Next(1,pins,fetched) == 0)
                cnt++;
            epins.Reset();
            string[] strings = new string[cnt];
            while (epins.Next(1,pins,fetched) == 0)
            {
                PinInfo pinfo;
                pins[0].QueryPinInfo(out pinfo);
                strings[ctr++] = pinfo.name +", "+pinfo.dir.ToString()+" ("+pinfo.ToString()+")";
                DsUtils.FreePinInfo(pinfo);
            }
            string txt = "Pins found on " + filter.ToString() + ":\n\n";
            string sep = "";
            for (ctr = 0; ctr < cnt; ctr++)
            {
                txt += sep + strings[ctr];
                sep = "\n";
            }
            MessageBox.Show(txt,"Info");
        }

        /// <summary>
        /// releases all of the filters from a graph
        /// </summary>
        /// <param name="graph">IFilterGraph, the graph being discarded</param>
        /// <returns></returns>
        static public int ReleaseFilters(IFilterGraph graph)
        {
            IEnumFilters eFilters = null;
            int hr;

            hr = graph.EnumFilters(out eFilters);
            checkHR(hr,"Can't enumerate filters");

            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IBaseFilter[] filter = new IBaseFilter[1];
            while (eFilters.Next(1,filter,fetched) == 0)
            {
                hr = graph.RemoveFilter(filter[0]);
                checkHR(hr,"Can't remove filter");
                hr = eFilters.Reset();
                checkHR(hr,"Can't reset enumerator");
                filter[0] = null;
            }

            return 0;
        }

        /// <summary>
        /// ensures a path name ends with a backslash so that a filename can
        /// be appended directly to create a fully qualifiled filespec
        /// </summary>
        /// <param name="path">string, drive/directory path to be tested</param>
        /// <returns></returns>
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
