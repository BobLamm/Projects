using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
using System.Windows.Forms;

namespace VGV101
{
    public partial class DlgStartupSettingsEntry : Form
    {
        public DlgStartupSettingsEntry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  // Memorize Home Positions
        {
            DlgSetCameraHome frm = new DlgSetCameraHome();
            frm.ShowDialog(this);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
