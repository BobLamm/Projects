/**
 * File: DlgShotGraphicButton.cs
 * 
 *	Copyright © 2016 by City Council Video.  All rights reserved.
 *
 *	$Id: /DlgShotGraphicButton.cs,v $
 */
/**
*	Provides an interface to set camera shots and graphic text that a user button calls up
*	
*	Author:			Fred Koschara and Bob Lamm
*	Creation Date:	December tenth, 2016
*	Last Modified:	December 23, 2016 @ 11:17 am
*
*	Revision History:
*	   Date		  by		Description
*	2016/12/23	blamm	original development
*	
*	TO DO:  Remove temporary items.  Figure out how to refresh button display after a name or text source change.	|					
*/
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

//using System.Collections;  // For Axis
//using System.ComponentModel;
using AXISMEDIACONTROLLib;

using System.IO;  // For WebRequests
using System.Net;
using System.Text;

using Utility.AxisCamera;

namespace VGV101
{
    // previously Form2
    public partial class DlgShotGraphicButton : Form
    {
        private int nRow;  // row in dataGridView - also the button number
        private int camNumber = 0;  // To keep track of camera number in case properties are changed
        // private DataGridView buttonsDataGridView;  // DataGridView to hold button information - now on form itself
        bool formFocusBack = false;  // to sense when the list editor has returned focus to the form and it should reload the updated button data.

        public DlgShotGraphicButton(DataGridView tempDataGridView, int theRow)  // When called, the function gets the DataGridView with the button info as well as the button number (row in dataGridView)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // LOAD UP FOUR DATAVIEWS WITH BUTTON, CAMERA, LIST AND CURRENT ACTIVE LIST ENTRY INFO.  
            // Button info is passed to bobsButton class function (along with number (nRow) of button), 
            // camera info is loaded into camerasData, 
            // and when needed: list info into displayedListDataGridView and active list into into activeRowDataGridView...
            // Only the first 'Text From' field is used, when imported, all the 3 text lines are imported as a block.

            buttonsDataGridView = tempDataGridView;  // Load up Button information that was passed to function
            nRow = theRow;  // Button number (row) passed to function

            textBox1.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Button_Name"].Index].Value.ToString();  // Load Button name from Button Settings in buttonsData...

            if (buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Name_From_Graphic"].Index].Value.ToString() == "Yes")  // Load checkbox status on whether button name comes from graphic  
            {
                textBox1.Text = textBox2.Text;
                checkBox5.Checked = true;
                button4.Enabled = false;
            }

            cfg.ReadXMLFile("Camera_Settings.xml", cameraDataGridView);  // Read camera settings file into dataGridView1

            // Enter camera names into camera selector combobox
            comboBox1.Items[0] = cameraDataGridView.Rows[0].Cells[cameraDataGridView.Columns["Camera_Name"].Index].Value.ToString();
            comboBox1.Items[1] = cameraDataGridView.Rows[1].Cells[cameraDataGridView.Columns["Camera_Name"].Index].Value.ToString();
            comboBox1.Items[2] = cameraDataGridView.Rows[2].Cells[cameraDataGridView.Columns["Camera_Name"].Index].Value.ToString();
            comboBox1.Items[3] = cameraDataGridView.Rows[3].Cells[cameraDataGridView.Columns["Camera_Name"].Index].Value.ToString();

            /*
                for (int cnt = 0, nLim = cfg.NumCameras; cnt < nLim; cnt++)
                comboBox1.Items[cnt] = cfg.Camera(cnt).cameraName;
            */

            // FILL IN GRAPHICS INFO - Get from buttonsDataGridView - only the first TextLine1Source is used for the entire 3-line block of text.

            string lineOneTextSource = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_1_Source"].Index].Value.ToString();  // See where the text for this button is coming from.
            
            if (lineOneTextSource == "Text")  // If the text is typed in right here on this form, get it from the buttonsDataGridView...
            {
                textBox2.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_1"].Index].Value.ToString();
                textBox3.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_2"].Index].Value.ToString();
                textBox4.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_3"].Index].Value.ToString();
            }
            else  // Or if the text comes from one of the lists...
            {
                textBox2.ReadOnly = true; textBox3.ReadOnly = true; textBox4.ReadOnly = true;
                textBox2.BackColor = System.Drawing.SystemColors.Menu; textBox3.BackColor = System.Drawing.SystemColors.Menu; textBox4.BackColor = System.Drawing.SystemColors.Menu;
                textBox2.ForeColor = Color.Red; textBox3.ForeColor = Color.Red; textBox4.ForeColor = Color.Red;
                button5.BackColor = Color.Yellow;  // Highlight the button and have it display where the text is coming from.
                button5.Font = new Font(button5.Font, FontStyle.Bold);
                button5.Text = "Text from " + lineOneTextSource;

                // Read appropriate list file into displayedListDataGridView
                // Read current active list entries file into activeRowDataGridView to see which row in list is active
                if (!GetListData(lineOneTextSource))
                    return;

                // Find the row that has the proper list information:
                int lineNumber = 0; // Default is first line
                
                if (activeRowDataGridView.Rows[0].Cells[activeRowDataGridView.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowDataGridView.Rows[0].Cells[activeRowDataGridView.Columns["Active_Entry"].Index].Value.ToString());
                }
                if (activeRowDataGridView.Rows[1].Cells[activeRowDataGridView.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowDataGridView.Rows[1].Cells[activeRowDataGridView.Columns["Active_Entry"].Index].Value.ToString());
                }
                if (activeRowDataGridView.Rows[2].Cells[activeRowDataGridView.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowDataGridView.Rows[2].Cells[activeRowDataGridView.Columns["Active_Entry"].Index].Value.ToString());
                }
                if (activeRowDataGridView.Rows[3].Cells[activeRowDataGridView.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowDataGridView.Rows[3].Cells[activeRowDataGridView.Columns["Active_Entry"].Index].Value.ToString());
                }
                
                textBox2.Text = displayedListDataGridView.Rows[lineNumber].Cells[displayedListDataGridView.Columns["First_Line"].Index].Value.ToString();  // Read this info into text boxes.  // = "Text from " + listFileName + ", Line #: " + lineNumber + ", :" + displayedListData.Rows[lineNumber].Cells[0].Value.ToString();
                textBox3.Text = displayedListDataGridView.Rows[lineNumber].Cells[displayedListDataGridView.Columns["Second_Line"].Index].Value.ToString();
                textBox4.Text = displayedListDataGridView.Rows[lineNumber].Cells[displayedListDataGridView.Columns["Third_Line"].Index].Value.ToString();
            }

            // Recall Graphic Repeat Cycle Info
            numericUpDown7.Value = int.Parse(buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Repeat_Graphic_Seconds"].Index].Value.ToString());  // Recall graphic repeat time  (Repeat_Graphic_Seconds)
            numericUpDown8.Value = int.Parse(buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Leave_Graphic_Seconds"].Index].Value.ToString());  // Recall graphic duration  (Leave_Graphic_Seconds)

            if (buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Bring_Graphic_Up_Second_Click"].Index].Value.ToString() == "Yes")  // Load graphic repeat status and set radio buttons and disable numerical display if repeat will be by a second click  (Bring_Graphic_Up_Second_Click)  
            {
                radioButton7.Checked = true;
                numericUpDown7.Enabled = false;
                numericUpDown8.Enabled = false;
            }
        }

        private bool GetListData(string lineOneTextSource)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            // Open appropriate list file
            string listFileName = "People.xml";  // This is the default
            if (lineOneTextSource == "Agenda Items") { listFileName = "Agenda_Items.xml"; }
            if (lineOneTextSource == "Notices") { listFileName = "Notices.xml"; }
            if (lineOneTextSource == "Other Items") { listFileName = "Other_Items.xml"; }

            // Read appropriate list file into displayedListDataGridView
            if (!cfg.ReadXMLFile(listFileName, displayedListDataGridView)) // we can't proceed from here
            {
                MessageBox.Show(listFileName + " data not available", "ERROR");
                this.Close();
                return false;
            }

            // Read current active list entries file into activeRowDataGridView to see which row in list is active
            if (!cfg.ReadXMLFile("Current_Active_List_Entries.xml", activeRowDataGridView)) // we can't proceed from here
            {
                MessageBox.Show("Current Active List Entries not available", "ERROR");
                this.Close();
                return false;
            }

            return true;
        }

        // Form got focus back after list data changed (actual entry changed or row selection changed.)
        private void Form2_Activated(object sender, EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            if (formFocusBack == true)  // This is the variable that tests if the list data changed.  It is set by the 'Get Text From' button (button5)
            {
                // MessageBox.Show("Data Changed! Got Activated!"); 
                formFocusBack = false;  // reset variable

                // Open Buttons.xml File to get updated info...
                if (!cfg.ReadXMLFile("Buttons.xml", buttonsDataGridView)) // we can't proceed from here
                    {
                    MessageBox.Show("Button data not available", "ERROR");
                    this.Close();
                    return;
                }

                string lineOneTextSource = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_1_Source"].Index].Value.ToString();  // See where the text for this button is coming from.
                
                if (lineOneTextSource == "Text")  // If the text is typed in right here on this form, get it from the buttonsDataGridView...
                {
                    // MessageBox.Show("If statement:  lineOneText Source was 'text'");
                    textBox2.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_1"].Index].Value.ToString();
                    textBox3.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_2"].Index].Value.ToString();
                    textBox4.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_3"].Index].Value.ToString();
                    // MessageBox.Show("Finished the TEXT side of 'if' statement");
                }
                else  // Or if the text comes from one of the lists...
                {
                    // MessageBox.Show("If statement: lineOneText Source was " + lineOneTextSource); 
                    textBox2.ReadOnly = true;
                    textBox2.BackColor = System.Drawing.SystemColors.Menu;
                    textBox2.ForeColor = Color.Red;
                    button5.BackColor = Color.Yellow;  // Highlight the button and have it display where the text is coming from.
                    button5.Font = new Font(button5.Font, FontStyle.Bold);
                    button5.Text = "Text From " + lineOneTextSource;
                    // MessageBox.Show("Activation lineOneTextSource: " + lineOneTextSource);

                    // Read appropriate list file into displayedListDataGridView
                    // Read current active list entries file into activeRowDataGridView to see which row in list is active
                    if (!GetListData(lineOneTextSource))
                    {
                        MessageBox.Show("List data not available", "ERROR");
                        return;
                    }
                       

                    // Find the row that has the proper list information:
                    int lineNumber = 0;

                    if (activeRowDataGridView.Rows[0].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowDataGridView.Rows[0].Cells[1].Value.ToString()); }
                    if (activeRowDataGridView.Rows[1].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowDataGridView.Rows[1].Cells[1].Value.ToString()); }
                    if (activeRowDataGridView.Rows[2].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowDataGridView.Rows[2].Cells[1].Value.ToString()); }
                    if (activeRowDataGridView.Rows[3].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowDataGridView.Rows[3].Cells[1].Value.ToString()); }

                    textBox2.Text = displayedListDataGridView.Rows[lineNumber].Cells[0].Value.ToString();  // textBox2.Text = "Text from " + listFileName + ", Line #: " + lineNumber + ", :" + displayedListData.Rows[lineNumber].Cells[0].Value.ToString();
                    textBox3.Text = displayedListDataGridView.Rows[lineNumber].Cells[1].Value.ToString();
                    textBox4.Text = displayedListDataGridView.Rows[lineNumber].Cells[2].Value.ToString();
                    // MessageBox.Show("Finished the LIST side of 'if' statement - Text was from list was true");
                }
                // MessageBox.Show("Reached end of total if statement - - formFocustBack was true");
            }
            // else { MessageBox.Show("formFocusBack is false"); }
        }

        private void button4_Click_1(object sender, EventArgs e)  // Save Button Name - this button is only enabled when checkbox is turned off.
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Button_Name"].Index].Value = textBox1.Text;  // Update button name in buttonsDataGridView
            cfg.WriteCurrentXml("Buttons",buttonsDataGridView);   // Update Buttons file from info in buttonsDataGridView
            // ApplicationRestart? (See Form 1) or just read updated text?
            // btnArray[nRow].Text = textBox1.Text;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)  // Checkbox (is the button name coming from the graphic) changed.
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            if (checkBox5.Checked == true)
            {
                textBox1.Text = textBox2.Text;
                button4.Enabled = false;
                buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Name_From_Graphic"].Index].Value = "Yes";  
            }
            else
            {
                textBox1.Text = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Button_Name"].Index].Value.ToString(); 
                button4.Enabled = true;
                buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Name_From_Graphic"].Index].Value = "No";
            }

            // cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
            cfg.WriteXMLFile(buttonsDataGridView, "Buttons.xml");
            // ApplicationRestart?
        }

        // Selected Index (Camera) Changed - make note in camNumber, fill in values from Camera_Settings file and put camera image into monitor
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            // Turn camera controls on and identify the camNumber
            label3.Enabled = true;  // Turns on first line of camera control instructions
            label5.Enabled = true;  // Turns on second line of camera control instructions
            groupBox5.Enabled = true;  // Turn on Auto/Manual Iris Radio Buttons
            groupBox4.Enabled = true;  // Turn on Auto/Manual Focus Radio Buttons
            groupBox6.Enabled = true;  // Turn on Auto/Manual White Balance Radio Buttons
            groupBox7.Enabled = true;  // Turn on Backlight On/Off Radio Buttons
            groupBox8.Enabled = true;  // Turn on Camera Hold numeric up/down
            button1.Enabled = true;  // Turn on Save Primary Shot button
            button16.Enabled = true; // Turn on Recall Primary Shot
            button10.Enabled = true;  // Turn on Secondary Shot button
            button12.Enabled = true;  // Turn on Recall Secondary SHot button
            button13.Enabled = true;  // Turn on Focus On This Box button
            button14.Enabled = true;  // Turn on Iris Up button
            button15.Enabled = true;  // Turn on Iris Down button
            irisSteps.Enabled = true;  // Turn on Iris Steps numeric updown

            camNumber = comboBox1.SelectedIndex;
            CameraObject camera = cfg.Camera(camNumber);

            AxisCameraUtility.SetUpCameraMonitor(camera,AMC,txtCamStatus);  // Turn on camera monitor and feed camera into it
//            SetUpCameraMonitor(camerasData, camNumber);  // Turn on camera monitor and feed camera into it

            // Put up camera information and status for user to see
            txtCamStatus.Text = camera.manufacturer + " " + camera.model + " camera at " + camera.ipAddrPort + "\n" + camera.status;
            txtCamStatus.Visible = true;
            //            if (camerasData.Rows[camNumber].Cells[camerasData.Columns["Status"].Index].Value.ToString() != "Full Communication") txtCamStatus.BackColor = Color.Red;
            //            if (camerasData.Rows[camNumber].Cells[camerasData.Columns["Status"].Index].Value.ToString() == "Full Communication") txtCamStatus.BackColor = Color.LawnGreen;

            // Fill in numericUpDown values...
            numericUpDown6.Value = int.Parse(camera.holdFrames);
//            numericUpDown6.Value = int.Parse(camerasData.Rows[camNumber].Cells[camerasData.Columns["Hold_Frames"].Index].Value.ToString());  // Recall hold from dataGridView/file
            numericUpDown6.Enabled = true;
        }

        private void numericUpDown6_Validated(object sender, EventArgs e)  // Hold Duration Validated
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            // Update Camera Hold Duration in camera's data
            cfg.Camera(camNumber).holdFrames = numericUpDown6.Value.ToString();
            cfg.SaveCameras();

//            camerasData.Rows[camNumber].Cells[camerasData.Columns["Hold_Frames"].Index].Value = numericUpDown6.Value.ToString();  // Update Camera Hold Duration in camerasData
//            cfg.WriteCurrentXml("Camera_Settings", camerasData);  // Update Camera_Settings file from info in camerasData
        }

        // Save Primary Shot
        private void button1_Click(object sender, EventArgs e)
        {
            if (camNumber == int.Parse(buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Secondary_Camera_Number"].Index].Value.ToString()))  // check to make sure this isn't the same as the secondary camera...
                MessageBox.Show("Primary Camera has to be different from the one used for the Secondary Shot");
            else SaveCameraShot("Primary");
        }

        private void SaveCameraShot(string which)
        {    GlobalConfig cfg = GlobalConfig.Instance;

            //  Query for position and put coordinates into 'cameraResponse'
            string cameraResponse = cfg.Camera(camNumber).GetPosition();
//            string cameraResponse = SendCommandToCamera(camerasData, camNumber, "ptz.cgi?query=position");  //  Query for position and put coordinates into 'cameraResponse'
            MessageBox.Show("Camera response is:\n\n" + cameraResponse);  // TEMPORARY

            string pan = "0";  // initialize variables to prevent crashes if they aren't in response.
            string tilt = "0";
            string zoom = "5000";
            string iris = "0";

            using (StringReader reader = new StringReader(cameraResponse))  // Get pan/tilt/zoom values from web 'response' - parse into 'line' with reader.ReadLine
            {   string line = string.Empty;  // Empty the 'Line' variable
                do
                {    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (line.Substring(0, "pan=".Length) == "pan=") { pan = line.Substring("pan=".Length); } // MessageBox.Show("Pan is: " + pan); }
                        if (line.Substring(0, "tilt=".Length) == "tilt=") { tilt = line.Substring("tilt=".Length); } // MessageBox.Show("Tilt is: " + pan); }
                        if (line.Substring(0, "zoom=".Length) == "zoom=") { zoom = line.Substring("zoom=".Length); } // MessageBox.Show("Zoom is: " + tilt); }
                        if (line.Substring(0, "iris=".Length) == "iris=") { iris = line.Substring("iris=".Length); } // MessageBox.Show("Iris is: " + zoom); }
                    }
                } while (line != null);
            }

            // Write Position Data into buttonsDataGridView
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Number"].Index].Value = camNumber;
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Pan_Preset"].Index].Value = pan;
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Tilt_Preset"].Index].Value = tilt;
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Zoom_Preset"].Index].Value = zoom;

            // Write Iris Position Data into buttonsDataGridView
            string currentIrisPosition = "10"; // Get current iris position  TEMPORARY IMPLEMENTATION
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Iris_Preset"].Index].Value = currentIrisPosition;
            // Current or Auto Iris Setting?
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Auto_Iris_Preset"].Index].Value = (radioButton9.Checked ? "Off" : "On");

            // Write Focus Position Data into buttonsDataGridView
            string currentFocusPosition = "20"; // Get current focus position  TEMPORARY IMPLEMENTATION
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Focus_Preset"].Index].Value = currentFocusPosition;
            // Current or Auto Focus Setting?
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Auto_Focus_Preset"].Index].Value = (radioButton1.Checked ? "Off" : "On");

            // Write White Balance Data into buttonsDataGridView
            string currentWhiteBalancePosition = "30"; // Get current focus position  TEMPORARY IMPLEMENTATION
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_White_Preset"].Index].Value = currentWhiteBalancePosition;
            // Current or Auto White Balance Setting?
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Auto_White_Preset"].Index].Value = (radioButton11.Checked ? "Off" : "On");

            // Write Backlight Setting into buttonsDataGridView
            // Backlight On or Off?
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns[which+"_Camera_Auto_Backlight_Preset"].Index].Value = (radioButton3.Checked ? "On" : "Off");

            // Update Buttons file from buttonsDataGridView
            cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
        }

        private void button10_Click(object sender, EventArgs e)  // Save Secondary Shot
        {
            if (camNumber == int.Parse(buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Primary_Camera_Number"].Index].Value.ToString()))
                MessageBox.Show("Secondary Camera has to be different from the one used for the Primary Shot");
            else SaveCameraShot("Secondary");
        }


        // RECALL SHOTS - changing the comboboes reinitializes camera view

        private void button16_Click(object sender, EventArgs e)  // Recall Primary Shot
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            string cameraNumber = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Primary_Camera_Number"].Index].Value.ToString();
            string pan = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Primary_Camera_Pan_Preset"].Index].Value.ToString();
            string tilt = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Primary_Camera_Tilt_Preset"].Index].Value.ToString();
            string zoom = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Primary_Camera_Zoom_Preset"].Index].Value.ToString();

            // Send camera to PTZ position
            int camNumber = Int32.Parse(buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Primary_Camera_Number"].Index].Value.ToString());  // get camera number from button file
            comboBox1.SelectedIndex = camNumber;
            cfg.Camera(camNumber).GoTo(pan, tilt, zoom);
        }

        private void button12_Click(object sender, EventArgs e)  // Recall Secondary Shot
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            string cameraNumber = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Secondary_Camera_Number"].Index].Value.ToString();
            string pan = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Secondary_Camera_Pan_Preset"].Index].Value.ToString();
            string tilt = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Secondary_Camera_Tilt_Preset"].Index].Value.ToString();
            string zoom = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Secondary_Camera_Zoom_Preset"].Index].Value.ToString();

            // Send camera to PTZ position
            int camNumber = Int32.Parse(buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Secondary_Camera_Number"].Index].Value.ToString());  // get camera number from button file
            comboBox1.SelectedIndex = camNumber;
            cfg.Camera(camNumber).GoTo(pan, tilt, zoom);
        }

        
        // GRAPHICS BUTTONS

        private void button5_Click(object sender, EventArgs e)  //Get Text from a List - Opens List menu...
        {
            // Open appropriate list file

            string lineOneTextSource = buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_1_Source"].Index].Value.ToString();  // See where the text for this button is coming from.

            int listFileNumber = 0;  // Default is 'People'
            if (lineOneTextSource == "Agenda Items") { listFileNumber = 1; }
            if (lineOneTextSource == "Notices") { listFileNumber = 2; }
            if (lineOneTextSource == "Other Items") { listFileNumber = 3; }

            DlgListEditor frm = new DlgListEditor(listFileNumber, nRow);    // First variable tells new form which list is being called (by the combobox number), second variable tells new form which button it is connected to.
            frm.Show();

            formFocusBack = true;  // Sets flag so text is updated when the form becomes active again
        }

        private void button11_Click(object sender, EventArgs e)  // Choose Template
        {
            MessageBox.Show("This chooses the template the graphic is based on.");
        }

        private void button6_Click(object sender, EventArgs e)  // Save Graphic
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_1"].Index].Value = textBox2.Text;
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_2"].Index].Value = textBox3.Text;
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_3"].Index].Value = textBox4.Text;

            // Update Buttons file from info in buttonsDataGridView
            // cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
            cfg.WriteXMLFile(buttonsDataGridView, "Buttons.xml");
        }


        //Graphics Repeat Settings

        private void radioButton7_CheckedChanged(object sender, EventArgs e)  // Graphic Repeat Setting Changed
        {
            if (radioButton7.Checked == true)
            {
                // MessageBox.Show("Graphics will be brought up with second click on this button.");
                numericUpDown7.Enabled = false;
                numericUpDown8.Enabled = false;
                buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Bring_Graphic_Up_Second_Click"].Index].Value = "Yes";

                // Update Buttons file from info in buttonsDataGridView
                GlobalConfig cfg = GlobalConfig.Instance;
                // cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
                cfg.WriteXMLFile(buttonsDataGridView, "Buttons.xml");
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                // MessageBox.Show("Graphics will come up every " + numericUpDown7.Value.ToString() + " seconds.");
                numericUpDown7.Enabled = true;
                numericUpDown8.Enabled = true;
                buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Bring_Graphic_Up_Second_Click"].Index].Value = "No";

                // Update Buttons file from info in buttonsDataGridView
                GlobalConfig cfg = GlobalConfig.Instance;
                // cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
                cfg.WriteXMLFile(buttonsDataGridView, "Buttons.xml");
            }
        }

        private void numericUpDown7_Validated(object sender, EventArgs e)  // Bring Up Graphic Every X seconds up/Down Validated
        {
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Repeat_Graphic_Seconds"].Index].Value = numericUpDown7.Value.ToString();

            // Update Buttons file from info in buttonsDataGridView
            GlobalConfig cfg = GlobalConfig.Instance;
            // cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
            cfg.WriteXMLFile(buttonsDataGridView, "Buttons.xml");
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)  // Leave Graphic Up For X seconds up/Down Validated
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Repeat_Graphic_Seconds"].Index].Value = numericUpDown8.Value.ToString();

            // Update Buttons file from info in buttonsDataGridView
            // cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
            cfg.WriteXMLFile(buttonsDataGridView, "Buttons.xml");
        }

        private void button13_Click(object sender, EventArgs e)  // Focus button pressed...
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            AMC.FocusWindowURL = "http://" + cfg.Camera(camNumber).ipAddrPort + "/axis-cgi/autofocus/focuswindow/"; // Path for focus commands

            if (button13.BackColor == SystemColors.Control)
            {
                button13.BackColor = Color.Yellow;
                AMC.FocusWindowMode = 2;  //  Sets that a rectangle can be drawn to specify a focus window.
            }
            else
            {
                button13.BackColor = SystemColors.Control;
                AMC.FocusWindowMode = 0;  //  Turns focusing off.           
            }
        }

        private void button9_Click(object sender, EventArgs e)  // Cancel List Button
        {   GlobalConfig cfg = GlobalConfig.Instance;

            textBox2.ReadOnly = false; textBox3.ReadOnly = false; textBox4.ReadOnly = false;
            textBox2.BackColor = SystemColors.Window; textBox3.BackColor = SystemColors.Window; textBox4.BackColor = SystemColors.Window;
            textBox2.ForeColor = SystemColors.WindowText; textBox3.ForeColor = SystemColors.WindowText; textBox4.ForeColor = SystemColors.WindowText;

            // Turn List button off...
            button5.BackColor = SystemColors.Control;
            button5.Font = new Font(button5.Font, FontStyle.Regular);
            button5.Text = "Get text from...";

            // Update buttonsDataGridView...
            buttonsDataGridView.Rows[nRow].Cells[buttonsDataGridView.Columns["Text_Line_1_Source"].Index].Value = "Text";

            // Update Buttons.xml file from info in buttonsDataGridView...
            // cfg.WriteCurrentXml("Buttons", buttonsDataGridView);
            cfg.WriteXMLFile(buttonsDataGridView, "Buttons.xml");
        }

        private void button14_Click(object sender, EventArgs e)  // Iris up button
        {
            AdjustIris(10);     //  Send command to open iris
        }

        private void AdjustIris(int multiplier)
        {   GlobalConfig cfg = GlobalConfig.Instance;

            int irisStepValue = Convert.ToInt32(irisSteps.Value) * multiplier;

            string response = cfg.Camera(camNumber).PtzCommand("riris=" + irisStepValue);
            MessageBox.Show("Camera response is:\n\n" + response);  // TEMPORARY

            MessageBox.Show("ptz.cgi?riris =" + irisStepValue);
        }

        private void button15_Click(object sender, EventArgs e)  // Iris down button
        {
            AdjustIris(-10);    //  Send command to close iris
        }


        // UTILITY FUNCTIONS

        private void AMC_OnError(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent e)  // On AMC error
        {
            MessageBox.Show("Error with AMC viewer on Shot/Graphic Settings Menu ");
        }


    }    
}

/*
private void button2_Click_1(object sender, EventArgs e)  // Temporary pan left
{
    GlobalConfig cfg = GlobalConfig.Instance;
    string pan = "-10";
    string response = cfg.Camera(camNumber).Pan(pan);
    response = "Move Left";
    textBox5.Text = response;
}

private void button3_Click_1(object sender, EventArgs e)  // Temporary pan right
{
    GlobalConfig cfg = GlobalConfig.Instance;
    string pan = "10";
    string response = cfg.Camera(camNumber).Pan(pan);
    response = "Move Right";
    textBox5.Text = response;
}

private void button7_Click_1(object sender, EventArgs e)  // Temporary query for Position
{
    GlobalConfig cfg = GlobalConfig.Instance;
    string response = cfg.Camera(camNumber).GetPosition();
    textBox5.Text = response;

    string pan = "0";
    string tilt = "0";
    string zoom = "5000";
    string iris = "0";

    using (StringReader reader = new StringReader(response))  // Get pan/tilt/zoom values from web 'response' - read into 'line' with reader.ReadLine
    {
        string line = string.Empty;
        // MessageBox.Show("Empty the 'Line' Variable");
        do
        {
            line = reader.ReadLine();
            if (line != null)
            {
                // MessageBox.Show("Line is: " + line);
                if (line.Substring(0, "pan=".Length) == "pan=") { pan = line.Substring("pan=".Length); } // MessageBox.Show("Pan is: " + pan); }
                if (line.Substring(0, "tilt=".Length) == "tilt=") { tilt = line.Substring("tilt=".Length); } // MessageBox.Show("Tilt is: " + pan); }
                if (line.Substring(0, "zoom=".Length) == "zoom=") { zoom = line.Substring("zoom=".Length); } // MessageBox.Show("Zoom is: " + pan); }
                if (line.Substring(0, "iris=".Length) == "iris=") { iris = line.Substring("iris=".Length); } // MessageBox.Show("Iris is: " + pan); }
            }
        } while (line != null);
    }
}

private void button8_Click(object sender, EventArgs e)  // Temporary Go To Position
{
    GlobalConfig cfg = GlobalConfig.Instance;
    string pan = "0";
    string tilt = "0";
    string zoom = "5000";
    textBox5.Text = cfg.Camera(camNumber).GoTo(pan, tilt, zoom);
}
*/


//
// EOF: DlgShotGraphicButton.cs