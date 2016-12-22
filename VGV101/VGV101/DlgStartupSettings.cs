using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

//using System.IO;  // For Databasing
//using System.Data.OleDb;  // For Databasing

//using System.Xml;  // for XML

namespace VGV101
{
    // STARTUP SETTINGS PAGE
    // previously Form13
    public partial class DlgStartupSettings : Form
    {
        public DlgStartupSettings()
        {   GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // Read Startup Settings from File
            if (!cfg.GetCurrentXml("Startup_Settings", startupSettings)) // we can't proceed from here
            {   MessageBox.Show("Could not read Current Startup Settings File", "ERROR");
                this.Close();
                return;
            }

            startupSettings.Columns[0].ReadOnly = true;
            startupSettings.Columns[1].DefaultCellStyle.BackColor = Color.White;
        }

        // UPDATE STARTUP SETTINGS
        private void button18_Click(object sender, EventArgs e)
        {   GlobalConfig cfg = GlobalConfig.Instance;

            // Update Startup Settings file from info in dataGridView1
            if (cfg.WriteCurrentXml("Startup_Settings", startupSettings))
                MessageBox.Show("Updated Startup Settings Temp File");
        }
    }
}
