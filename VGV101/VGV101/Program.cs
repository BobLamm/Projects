/**
 * File: Program.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /VGV101/Program.cs,v $
 */
/**
*	C# program entry point
*
*	Author:			Visual Studio, Fred Koschara
*	Creation Date:	prior to September, 2016
*	Last Modified:	January 9, 2017 @ 7:14 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/01/09	wfredk	documentation updates
*	2016/12/29	wfredk	GlobalConfig.Init() reads Registry, not this module
*	2016/12/29	wfredk	added documentation header
*/
using System;
using System.Windows.Forms;

namespace VGV101
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {   GlobalConfig cfg = GlobalConfig.Instance;
            cfg.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {   Application.Run(new MainWindow());
            }
            catch (Exception ex)
            {   MessageBox.Show(ex.ToString(),"Program Halted");
            }
        }
    }
}
//
// EOF: Program.cs
