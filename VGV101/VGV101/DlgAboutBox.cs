using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
//using System.Drawing;
//using System.Text;
using System.Windows.Forms;

//using System.IO;  // For Databasing
using System.Data.OleDb;  // For Databasing

// Note to myself:  We need to store the hardware configuration data in a secret, secure, way since the code that users buy is based on it and we don't want people being able to fudge the data.


namespace VGV101
{
    public partial class DlgAboutBox : Form
    {
        public DlgAboutBox()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            txtCfgRoot.Text = cfg.CfgRoot;
            txtLogRoot.Text = cfg.LogRoot;
            txtMediaRoot.Text = cfg.MediaRoot;

            // Hardware Info
            string pathToExcelFile = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+cfg.MediaRoot+@"Show Files\meeting.xlsx; Extended Properties=""Excel 8.0; HDR=Yes;"";";
            OleDbConnection connectionToExcelFile = new OleDbConnection(pathToExcelFile);
            OleDbDataAdapter queryString = new OleDbDataAdapter("Select * from [Hardware Configuration$]", connectionToExcelFile);
            DataTable dataTableWithInfo = new DataTable();
            queryString.Fill(dataTableWithInfo);
            dataGridView1.DataSource = dataTableWithInfo;

            //  DataGridView1 is already set to read-only mode
        }

        private void btnOpenCfgRoot_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txtCfgRoot.Text);
        }

        private void btnOpenMediaRoot_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txtMediaRoot.Text);
        }

        private void btnOpenLogRoot_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txtLogRoot.Text);
        }
    }
}
