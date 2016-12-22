using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;  // For Databasing
// using System.Data.OleDb;  // For Databasing

namespace VGV101
{
    // previously Form15
    public partial class DlgRegistrationPage : Form
    {
        public DlgRegistrationPage()
        {   GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // Read User Information from File
            if (!cfg.GetCurrentXml("User_Information", userInfoData)) // we can't proceed from here
            {
                MessageBox.Show("Current User Information not available", "ERROR");
                this.Close();
                return;
            }

            // Set userInfoData read/write parameters and background colors
            userInfoData.Columns[0].ReadOnly = true;
            userInfoData.Columns[1].DefaultCellStyle.BackColor = Color.White;

            // Read Hardware Information from File
            if (!cfg.GetCurrentXml("Hardware_Configuration", hwConfigData)) // we can't proceed from here
            {
                MessageBox.Show("Hardware Configuration not available", "ERROR");
//                this.Close();
                return;
            }

            // Read Key Information from File
            if (!cfg.GetCurrentXml("Keys", keysData)) // we can't proceed from here
            {
                MessageBox.Show("Current Key Information not available", "ERROR");
                this.Close();
                return;
            }

            // Set DataGridView3 read/write parameters and background colors
            keysData.Columns[0].ReadOnly = true;
            keysData.Columns[1].DefaultCellStyle.BackColor = Color.White;
        }


        // REGISTER TO PURCHASE AND REMOVE WATERMARKS

        private void button18_Click(object sender, EventArgs e)  
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            // keysData.ReadOnly = true;  // Freezes key info.
            // keysData.Columns[1].DefaultCellStyle.BackColor = SystemColors.Control;  // Updates background to reflect this

            // Update User Information file from info in userInfoData
            if (cfg.WriteCurrentXml("User_Information", userInfoData)
            // Update Keys file from info in keysData
            &&  cfg.WriteCurrentXml("Keys", keysData))
                MessageBox.Show(@"Product Registered.
Customer info can be changed and the key will continue to work unless the hardware (hard drive, motherboard and NIC Card) changes.
At that point a new purchase will be required to work with the new hardware.");
        }
    }
}
