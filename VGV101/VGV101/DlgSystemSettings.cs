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

using System.Xml;  // for XML

namespace VGV101
{
    public partial class DlgSystemSettings : Form
    {
        public DlgSystemSettings()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            if (!cfg.GetCurrentXml("System_Settings", dataGridView1)) // we can't proceed from here
            {
                MessageBox.Show("Could not read Current System Settings File", "ERROR");
                this.Close();
                return;
            }


            /*
            // Load system settings
            string pathToExcelFile = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + cfg.MediaRoot+@"Show Files\meeting.xlsx; Extended Properties=""Excel 8.0; HDR=Yes;"";";
            OleDbConnection connectionToExcelFile = new OleDbConnection(pathToExcelFile);
            OleDbDataAdapter queryString = new OleDbDataAdapter("Select * from [System Settings$]", connectionToExcelFile);
            DataTable dataTableWithInfo = new DataTable();
            queryString.Fill(dataTableWithInfo);
            dataGridView1.DataSource = dataTableWithInfo;
             * 
             * */

            // Set DataGridView1 read/write parameters and background colors

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
        }

        private void Form12_Load(object sender, EventArgs e)
        {
//            dataGridView1.Rows[13].Cells[1].ReadOnly = false;
//            dataGridView1.Rows[13].Cells[1].Style.BackColor = Color.White;
//            dataGridView1.Rows[14].Cells[1].ReadOnly = false;
//            dataGridView1.Rows[14].Cells[1].Style.BackColor = Color.White;
//            dataGridView1.Rows[15].Cells[1].ReadOnly = false;
//            dataGridView1.Rows[15].Cells[1].Style.BackColor = Color.White;
//            dataGridView1.Rows[16].Cells[1].ReadOnly = false;
//            dataGridView1.Rows[16].Cells[1].Style.BackColor = Color.White;
//            dataGridView1.Rows[17].Cells[1].ReadOnly = false;
//            dataGridView1.Rows[17].Cells[1].Style.BackColor = Color.White;
//            dataGridView1.Rows[18].Cells[1].ReadOnly = false;
//            dataGridView1.Rows[18].Cells[1].Style.BackColor = Color.White;
//            dataGridView1.Rows[19].Cells[1].ReadOnly = false;
//            dataGridView1.Rows[19].Cells[1].Style.BackColor = Color.White;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Updates System Settings and Meeting (.MTG) file");
        }
    }
}
