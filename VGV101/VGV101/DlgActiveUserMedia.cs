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
    public partial class DlgActiveUserMedia : Form
    {
        public DlgActiveUserMedia()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            if (!cfg.GetCurrentXml("Media", mediasData)) // we can't proceed from here
            {
                MessageBox.Show("Media data not available", "ERROR");
                this.Close();
                return;
            }
        }
    }
}

