/**
 * File: Program.cs
 * 
 *	Copyright © 2016 by City Council Video.  All rights reserved.
 *
 *	$Id: /Program.cs,v $
 */
/**
*	FUNCTION
*
*	Author:			Visual Studio, Fred Koschara
*	Creation Date:	prior to September, 2016
*	Last Modified:	December 29, 2016 @ 4:54 pm
*
*	Revision History:
*	   Date		  by		Description
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
