using System;
using System.Windows.Forms;
using Utility.ModifyRegistry;

namespace VGV101
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
            // MessageBox.Show(regData.SubKey);    // "SOFTWARE\VGV101"
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
            {   MessageBox.Show(ex.ToString(),"Program Halted");
            }
        }
    }
}
