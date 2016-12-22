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
    /*
    public int SetDataGridColColor()
    {
        return (5);
    }
    */
    public partial class DlgButtonSettings : Form
    {
        public DlgButtonSettings()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // BUTTONS PAGE

            // Read Button Settings from File
            if (!cfg.GetCurrentXml("Buttons", buttonsData)) // we can't proceed from here
            {
                MessageBox.Show("Button data not available", "ERROR");
                this.Close();
                return;
            }

            // Set DataGridView read/write parameters and background colors

            buttonsData.RowTemplate.MinimumHeight = 30;

            buttonsData.Columns[0].ReadOnly = true;
            buttonsData.Columns[1].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[2].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[3].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[4].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[5].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[6].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[7].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[8].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[9].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[10].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[11].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[12].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[13].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[14].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[15].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[16].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[17].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[18].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[19].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[20].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[21].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[22].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[23].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[24].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[25].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[26].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[27].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[28].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[29].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[30].DefaultCellStyle.BackColor = Color.White;
            buttonsData.Columns[31].DefaultCellStyle.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)  // Test Selected Button
        {
            MessageBox.Show("Tests Selected Button To See What It Does");
        }


        // UPDATE BUTTONS SETTINGS

        private void button18_Click(object sender, EventArgs e)  // Update Buttons
        {
            // Update Startup Settings file from info in buttonsData

            GlobalConfig cfg = GlobalConfig.Instance;
            if (cfg.WriteCurrentXml("Buttons", buttonsData))
                MessageBox.Show("Updated Buttons Settings");
        }


        /*
        public string GetDataGridViewData(DataGridView dataGridViewTemp, string columnName, int rowNumber)
        {
            foreach (DataGridViewColumn col in dataGridViewTemp.Columns)
            {
                if (col.Name == columnName)
                {
                    string returnValue = dataGridViewTemp.Rows[rowNumber].Cells[col.Index].Value.ToString();
                }
                else
                {
                    MessageBox.Show("Could not find " + columnName + " information from file.");
                }
            }
            return ("5");
        }
        */
    }
}
