/**
 * File: Program.cs
 * 
 *	Copyright © 2016 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/Program.cs,v $
 */
/**
*	C# program entry point
*
*	Author:			Bob Lamm
*	Creation Date:	October twentieth, 2016
*	Last Modified:	December 20, 2016 @ 1:04 am
*
*	Revision History:
*	   Date		  by		Description
*	2016/12/20	wfredk	add Registry support, catch startup exceptions
*	2016/12/20	wfredk	rename program to Video_Test_Fixture, add documentation
*	2016/10/20	blamm	original development ("Prototype for Fred" C# solution)
*/
using System;
using System.Windows.Forms;
using Utility.ModifyRegistry;

namespace Video_Test_Fixture
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ModifyRegistry regData = new ModifyRegistry();
            regData.SubKey = @"SOFTWARE\VGV101";
            String cfgRoot = regData.ReadString("CfgRoot", @"C:\VGV Software\Configuration\");
            String logRoot = regData.ReadString("LogRoot", @"C:\VGV Software\Logs\");
            String mediaRoot = regData.ReadString("MediaRoot", @"C:\VGV Customer Media\");
            GlobalConfig cfg = GlobalConfig.Instance;
            cfg.Init(cfgRoot, mediaRoot, logRoot);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {   Application.Run(new MainWindow());
            }
            catch (Exception ex)
            {   MessageBox.Show(ex.ToString(), "Program Halted");
            }
        }
    }
}
//
// EOF: Program.cs
