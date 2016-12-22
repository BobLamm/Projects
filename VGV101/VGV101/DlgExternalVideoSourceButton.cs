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
    public partial class DlgExternalVideoSourceButton : Form
    {
        public DlgExternalVideoSourceButton()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // Load video source settings
            string pathToExcelFile = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+cfg.MediaRoot+@"Show Files\meeting.xlsx; Extended Properties=""Excel 8.0; HDR=Yes;"";";
            OleDbConnection connectionToExcelFile = new OleDbConnection(pathToExcelFile);
            OleDbDataAdapter queryString = new OleDbDataAdapter("Select * from [Video Source Settings$]", connectionToExcelFile);
            DataTable dataTableWithInfo = new DataTable();
            queryString.Fill(dataTableWithInfo);
            dataGridView1.DataSource = dataTableWithInfo;

            // Enter video source names into camera selector combobox

            comboBox1.Items[0] = dataGridView1.Rows[0].Cells[2].Value.ToString();
            comboBox1.Items[1] = dataGridView1.Rows[1].Cells[2].Value.ToString();
            comboBox1.Items[2] = dataGridView1.Rows[2].Cells[2].Value.ToString();
            comboBox1.Items[3] = dataGridView1.Rows[3].Cells[2].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)  // Save Button Name
        {
            // FileName.Replace(cfg.MediaRoot,"%MEDIA_ROOT%");
            MessageBox.Show("This updates the name of the button.  (Unless it's being taken from the graphic text below.)");
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                textBox1.Text = textBox2.Text;
            }
            else
            {
                textBox1.Text = "Button Name";  // 
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  // Selected Index Changed
        {
            // MessageBox.Show("Selected Index =  " + comboBox1.SelectedItem.ToString() + " - Camera Number " + comboBox1.SelectedIndex.ToString() + " - Panel enabled.");
//            panel1.Enabled = true;
            label18.Text = (dataGridView1.Rows[comboBox1.SelectedIndex].Cells[3].Value.ToString() + " " + dataGridView1.Rows[comboBox1.SelectedIndex].Cells[4].Value.ToString() + " at " + dataGridView1.Rows[comboBox1.SelectedIndex].Cells[1].Value.ToString());  // Manufacturer
            label18.Visible = true;
            //           label17.Text = dataGridView1.Rows[comboBox1.SelectedIndex].Cells[4].Value.ToString();  // Model Number
            //           label17.Visible = true;
            //           label16.Text = dataGridView1.Rows[comboBox1.SelectedIndex].Cells[1].Value.ToString();  // IP Address
            //           label16.Visible = true;
            label2.Text = dataGridView1.Rows[comboBox1.SelectedIndex].Cells[5].Value.ToString();  // Status
            label2.Visible = true;
        }


        // Graphics Buttons

        private void button5_Click(object sender, EventArgs e)  //Get Text from List Button - needs to tell menu which button is calling this, keep track of category and which entry it's on.
        {
            int isConnectedToButton = 2;
            DlgListEditor frm = new DlgListEditor(0, isConnectedToButton);
            frm.Show();
        }
        
        private void button11_Click_1(object sender, EventArgs e)  // Choose Template
        {
            MessageBox.Show("This chooses the template the graphic is based on.");
        }

        private void button6_Click_1(object sender, EventArgs e)  // Save Graphic
        {
            // FileName.Replace(cfg.MediaRoot,"%MEDIA_ROOT%");
            MessageBox.Show("This updates the graphic information associated with this button and saves the MTG file.");
        }


        //Graphics Repeat Settings

        private void radioButton7_CheckedChanged_1(object sender, EventArgs e)  // Graphic Repeat Setting Changed
        {
            if (radioButton7.Checked == true)
            {
                MessageBox.Show("Graphics will be brought up with second click on this button.");
            }
            else
            {
                MessageBox.Show("Graphics will come up every " + numericUpDown7.Value.ToString() + " seconds.");
            }
        }


        // Create Button

        private void button2_Click_1(object sender, EventArgs e)  // Create Button
        {
            MessageBox.Show("New Button Will Be Created and Stored in MTG File.");
        }

    }
}
