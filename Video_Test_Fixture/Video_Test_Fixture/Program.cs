/**
 * File: Program.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/Program.cs,v $
 */
/**
*	C# program entry point
*
*	Author:			Bob Lamm
*	Creation Date:	October twentieth, 2016
*	Last Modified:	January 9, 2017 @ 7:29 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/01/09	wfredk	GlobalConfig.Init() reads Registry, not this module (restored)
*	2016/12/20	wfredk	add Registry support, catch startup exceptions
*	2016/12/20	wfredk	rename program to Video_Test_Fixture, add documentation
*	2016/10/20	blamm	original development ("Prototype for Fred" C# solution)
*/
using System;
using System.Windows.Forms;

namespace Video_Test_Fixture
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
            {   MessageBox.Show(ex.ToString(), "Program Halted");
            }
        }
    }
}
//
// EOF: Program.cs
