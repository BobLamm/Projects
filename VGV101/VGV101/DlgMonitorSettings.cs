using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;  // For Databasing
using System.Data.OleDb;  // For Databasing

namespace VGV101
{
    // MONITOR SETTINGS PAGE
    // previously Form17
    public partial class DlgMonitorSettings : Form
    {
        public DlgMonitorSettings()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // Read Monitor Settings from File
            if (!cfg.GetCurrentXml("Monitor_Settings", monitorsData)) // we can't proceed from here
            {
                MessageBox.Show("Current Monitor Settings not available", "ERROR");
                this.Close();
                return;
            }

            // Set DataGridView read/write parameters and background colors

            monitorsData.Columns[0].ReadOnly = true;
            monitorsData.Columns[1].DefaultCellStyle.BackColor = Color.White;
            monitorsData.Columns[2].DefaultCellStyle.BackColor = Color.White;
            monitorsData.Columns[3].DefaultCellStyle.BackColor = Color.White;
            monitorsData.Columns[4].DefaultCellStyle.BackColor = Color.White;
            monitorsData.Columns[5].DefaultCellStyle.BackColor = Color.White;
        }


        // UPDATE MONITOR SETTINGS

        private void button1_Click(object sender, EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            // Update Monitor Settings file from info in dataGridView1
            if (cfg.WriteCurrentXml("Monitor_Settings", monitorsData))
                MessageBox.Show("Updated Monitor Settings Temp File");
        }
    }
}
