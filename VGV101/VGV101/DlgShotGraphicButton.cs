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
        private DataGridView buttonsData;  // DataGridView to hold button information
        bool formFocusBack = false;  // to sense when the list editor has returned focus to the form and it should reload the updated button data.

        public DlgShotGraphicButton(DataGridView dGrid, int theRow)  // When called, the function gets the DataGridView with the button info as well as the button number (row in dataGridView)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // LOAD UP FOUR DATAVIEWS WITH BUTTON, CAMERA, LIST AND CURRENT ACTIVE LIST ENTRY INFO.  
            // Button info is passed to bobsButton class function (along with number (nRow) of button), 
            // camera info is loaded into camerasData, 
            // and when needed: list info into displayedListData and active list into into activeRowData...
            // Only the first 'Text From' field is used, when imported, all the 3 text lines are imported as a block.

            buttonsData = dGrid;  // Load up Button information that was passed to function
            nRow = theRow;  // Button number (row) passed to function

            textBox1.Text = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Button_Name"].Index].Value.ToString();  // Load Button name from Button Settings in buttonsData...

            if (buttonsData.Rows[nRow].Cells[buttonsData.Columns["Name_From_Graphic"].Index].Value.ToString() == "Yes")  // Load checkbox status on whether button name comes from graphic  
            {
                textBox1.Text = textBox2.Text;
                checkBox5.Checked = true;
                button4.Enabled = false;
            }
/*
            // Read Camera Settings from Camera_Settings.xml file into camerasData...
            if (!cfg.GetCurrentXml("Camera_Settings", camerasData)) // we can't proceed from here
            {
                MessageBox.Show("Camera data not available", "ERROR");
                this.Close();
                return;
            }
*/
            // Enter camera names into camera selector combobox            
            for (int cnt = 0, nLim = cfg.NumCameras; cnt < nLim; cnt++)
                comboBox1.Items[cnt] = cfg.Camera(cnt).cameraName;
/*
            comboBox1.Items[0] = camerasData.Rows[0].Cells[camerasData.Columns["Camera_Name"].Index].Value.ToString();
            comboBox1.Items[1] = camerasData.Rows[1].Cells[camerasData.Columns["Camera_Name"].Index].Value.ToString();
            comboBox1.Items[2] = camerasData.Rows[2].Cells[camerasData.Columns["Camera_Name"].Index].Value.ToString();
            comboBox1.Items[3] = camerasData.Rows[3].Cells[camerasData.Columns["Camera_Name"].Index].Value.ToString();
*/

            // FILL IN GRAPHICS INFO - Get from buttonsData - only the first TextLine1Source is used for the entire 3-line block of text.

            string lineOneTextSource = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_1_Source"].Index].Value.ToString();  // See where the text for this button is coming from.
            
            if (lineOneTextSource == "Text")  // If the text is typed in right here on this form, get it from the buttonsData...
            {
                textBox2.Text = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_1"].Index].Value.ToString();
                textBox3.Text = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_2"].Index].Value.ToString();
                textBox4.Text = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_3"].Index].Value.ToString();
            }
            else  // Or if the text comes from one of the lists...
            {
                textBox2.ReadOnly = true; textBox3.ReadOnly = true; textBox4.ReadOnly = true;
                textBox2.BackColor = System.Drawing.SystemColors.Menu; textBox3.BackColor = System.Drawing.SystemColors.Menu; textBox4.BackColor = System.Drawing.SystemColors.Menu;
                textBox2.ForeColor = Color.Red; textBox3.ForeColor = Color.Red; textBox4.ForeColor = Color.Red;
                button5.BackColor = Color.Yellow;  // Highlight the button and have it display where the text is coming from.
                button5.Font = new Font(button5.Font, FontStyle.Bold);
                button5.Text = "Text from " + lineOneTextSource;
                // MessageBox.Show("Initialization lineOneTextSource: " + lineOneTextSource);

                // Read appropriate list file into displayedListData
                // Read current active list entries file into activeRowData to see which row in list is active
                if (!GetListData(lineOneTextSource))
                    return;

                // Find the row that has the proper list information:
                int lineNumber = 0; // Default is first line
                
                if (activeRowData.Rows[0].Cells[activeRowData.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowData.Rows[0].Cells[activeRowData.Columns["Active_Entry"].Index].Value.ToString());
                }
                if (activeRowData.Rows[1].Cells[activeRowData.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowData.Rows[1].Cells[activeRowData.Columns["Active_Entry"].Index].Value.ToString());
                }
                if (activeRowData.Rows[2].Cells[activeRowData.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowData.Rows[2].Cells[activeRowData.Columns["Active_Entry"].Index].Value.ToString());
                }
                if (activeRowData.Rows[3].Cells[activeRowData.Columns["List"].Index].Value.ToString() == lineOneTextSource)
                {   lineNumber = int.Parse(activeRowData.Rows[3].Cells[activeRowData.Columns["Active_Entry"].Index].Value.ToString());
                }
                
                textBox2.Text = displayedListData.Rows[lineNumber].Cells[displayedListData.Columns["First_Line"].Index].Value.ToString();  // Read this info into text boxes.  // = "Text from " + listFileName + ", Line #: " + lineNumber + ", :" + displayedListData.Rows[lineNumber].Cells[0].Value.ToString();
                textBox3.Text = displayedListData.Rows[lineNumber].Cells[displayedListData.Columns["Second_Line"].Index].Value.ToString();
                textBox4.Text = displayedListData.Rows[lineNumber].Cells[displayedListData.Columns["Third_Line"].Index].Value.ToString();
            }

            // Recall Graphic Repeat Cycle Info
            numericUpDown7.Value = int.Parse(buttonsData.Rows[nRow].Cells[buttonsData.Columns["Repeat_Graphic_Seconds"].Index].Value.ToString());  // Recall graphic repeat time  (Repeat_Graphic_Seconds)
            numericUpDown8.Value = int.Parse(buttonsData.Rows[nRow].Cells[buttonsData.Columns["Leave_Graphic_Seconds"].Index].Value.ToString());  // Recall graphic duration  (Leave_Graphic_Seconds)

            if (buttonsData.Rows[nRow].Cells[buttonsData.Columns["Bring_Graphic_Up_Second_Click"].Index].Value.ToString() == "Yes")  // Load graphic repeat status and set radio buttons and disable numerical display if repeat will be by a second click  (Bring_Graphic_Up_Second_Click)  
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
            string listFileName = "People";  // This is the default
            if (lineOneTextSource == "Agenda Items") { listFileName = "Agenda_Items"; }
            if (lineOneTextSource == "Notices") { listFileName = "Notices"; }
            if (lineOneTextSource == "Other Items") { listFileName = "Other_Items"; }

            // Read appropriate list file into displayedListData
            if (!cfg.GetCurrentXml(listFileName, displayedListData)) // we can't proceed from here
            {
                MessageBox.Show(listFileName.Replace("_", " ") + " data not available", "ERROR");
                this.Close();
                return false;
            }

            // Read current active list entries file into activeRowData to see which row in list is active
            if (!cfg.GetCurrentXml("Current_Active_List_Entries", activeRowData)) // we can't proceed from here
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
                if (!cfg.GetCurrentXml("Buttons", buttonsData)) // we can't proceed from here
                {
                    MessageBox.Show("Button data not available", "ERROR");
                    this.Close();
                    return;
                }

                string lineOneTextSource = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_1_Source"].Index].Value.ToString();  // See where the text for this button is coming from.
                
                if (lineOneTextSource == "Text")  // If the text is typed in right here on this form, get it from the buttonsData...
                {
                    // MessageBox.Show("If statement:  lineOneText Source was 'text'");
                    textBox2.Text = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_1"].Index].Value.ToString();
                    textBox3.Text = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_2"].Index].Value.ToString();
                    textBox4.Text = "On Activation, not from list: " + buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_3"].Index].Value.ToString();
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

                    // Read appropriate list file into displayedListData
                    // Read current active list entries file into activeRowData to see which row in list is active
                    if (!GetListData(lineOneTextSource))
                        return;

                    // Find the row that has the proper list information:
                    int lineNumber = 0;

                    if (activeRowData.Rows[0].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowData.Rows[0].Cells[1].Value.ToString()); }
                    if (activeRowData.Rows[1].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowData.Rows[1].Cells[1].Value.ToString()); }
                    if (activeRowData.Rows[2].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowData.Rows[2].Cells[1].Value.ToString()); }
                    if (activeRowData.Rows[3].Cells[0].Value.ToString() == lineOneTextSource) { lineNumber = int.Parse(activeRowData.Rows[3].Cells[1].Value.ToString()); }

                    textBox2.Text = displayedListData.Rows[lineNumber].Cells[0].Value.ToString();  // textBox2.Text = "Text from " + listFileName + ", Line #: " + lineNumber + ", :" + displayedListData.Rows[lineNumber].Cells[0].Value.ToString();
                    textBox3.Text = displayedListData.Rows[lineNumber].Cells[1].Value.ToString();
                    textBox4.Text = "On Activation, from list: " + displayedListData.Rows[lineNumber].Cells[2].Value.ToString();
                    // MessageBox.Show("Finished the LIST side of 'if' statement - Text was from list was true");
                }
                // MessageBox.Show("Reached end of total if statement - - formFocustBack was true");
            }
            // else { MessageBox.Show("formFocusBack is false"); }
        }

        private void button4_Click_1(object sender, EventArgs e)  // Save Button Name - this button is only enabled when checkbox is turned off.
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Button_Name"].Index].Value = textBox1.Text;  // Update button name in buttonsData
            cfg.WriteCurrentXml("Buttons",buttonsData);   // Update Buttons file from info in buttonsData
            // ApplicationRestart? (See Form 1) or just read updated text?
            // xxx // btnArray[nRow].Text = textBox1.Text;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)  // Checkbox (is the button name coming from the graphic) changed.
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            if (checkBox5.Checked == true)
            {
                textBox1.Text = textBox2.Text;
                button4.Enabled = false;
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Name_From_Graphic"].Index].Value = "Yes";  
            }
            else
            {
                textBox1.Text = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Button_Name"].Index].Value.ToString(); 
                button4.Enabled = true;
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Name_From_Graphic"].Index].Value = "No";
            }

            cfg.WriteCurrentXml("Buttons", buttonsData);
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
/*
            txtCamStatus.Text = (camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["Manufacturer"].Index].Value.ToString()
                + " " + camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["Model"].Index].Value.ToString()
                + " camera at " + camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString() + "\n"
                + camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["Status"].Index].Value.ToString());  // Put up Manufacturer, Model, IP Address and status...
*/
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
            if (camNumber == int.Parse(buttonsData.Rows[nRow].Cells[buttonsData.Columns["Secondary_Camera_Number"].Index].Value.ToString()))  // check to make sure this isn't the same as the secondary camera...
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

            // Write Position Data into buttonsData
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Numberr"].Index].Value = camNumber;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Pan_Preset"].Index].Value = pan;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Tilt_Preset"].Index].Value = tilt;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Zoom_Preset"].Index].Value = zoom;

            // Write Iris Position Data into buttonsData
            string currentIrisPosition = "10"; // Get current iris position  TEMPORARY IMPLEMENTATION
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Iris_Preset"].Index].Value = currentIrisPosition;
            // Current or Auto Iris Setting?
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Auto_Iris_Preset"].Index].Value = (radioButton9.Checked ? "Off" : "On");

            // Write Focus Position Data into buttonsData
            string currentFocusPosition = "20"; // Get current focus position  TEMPORARY IMPLEMENTATION
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Focus_Preset"].Index].Value = currentFocusPosition;
            // Current or Auto Focus Setting?
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Auto_Focus_Preset"].Index].Value = (radioButton1.Checked ? "Off" : "On");

            // Write White Balance Data into buttonsData
            string currentWhiteBalancePosition = "30"; // Get current focus position  TEMPORARY IMPLEMENTATION
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_White_Preset"].Index].Value = currentWhiteBalancePosition;
            // Current or Auto White Balance Setting?
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Auto_White_Preset"].Index].Value = (radioButton11.Checked ? "Off" : "On");

            // Write Backlight Setting into buttonsData
            // Backlight On or Off?
            buttonsData.Rows[nRow].Cells[buttonsData.Columns[which+"_Camera_Auto_Backlight_Preset"].Index].Value = (radioButton3.Checked ? "On" : "Off");

            // Update Buttons file from buttonsData
            cfg.WriteCurrentXml("Buttons", buttonsData);
        }

        private void button10_Click(object sender, EventArgs e)  // Save Secondary Shot
        {
            if (camNumber == int.Parse(buttonsData.Rows[nRow].Cells[buttonsData.Columns["Primary_Camera_Numberr"].Index].Value.ToString()))
                MessageBox.Show("Secondary Camera has to be different from the one used for the Primary Shot");
            else SaveCameraShot("Secondary");
        }

        // GRAPHICS BUTTONS

        private void button5_Click(object sender, EventArgs e)  //Get Text from a List - Opens List menu...
        {
            // Open appropriate list file

            string lineOneTextSource = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_1_Source"].Index].Value.ToString();  // See where the text for this button is coming from.

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

            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_1"].Index].Value = textBox2.Text;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_2"].Index].Value = textBox3.Text;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_3"].Index].Value = textBox4.Text;

            // Update Buttons file from info in buttonsData
            cfg.WriteCurrentXml("Buttons", buttonsData);  
        }


        //Graphics Repeat Settings

        private void radioButton7_CheckedChanged(object sender, EventArgs e)  // Graphic Repeat Setting Changed
        {
            if (radioButton7.Checked == true)
            {
                // MessageBox.Show("Graphics will be brought up with second click on this button.");
                numericUpDown7.Enabled = false;
                numericUpDown8.Enabled = false;
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Bring_Graphic_Up_Second_Click"].Index].Value = "Yes";

                // Update Buttons file from info in buttonsData
                GlobalConfig cfg = GlobalConfig.Instance;
                cfg.WriteCurrentXml("Buttons", buttonsData);
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                // MessageBox.Show("Graphics will come up every " + numericUpDown7.Value.ToString() + " seconds.");
                numericUpDown7.Enabled = true;
                numericUpDown8.Enabled = true;
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Bring_Graphic_Up_Second_Click"].Index].Value = "No";

                // Update Buttons file from info in buttonsData
                GlobalConfig cfg = GlobalConfig.Instance;
                cfg.WriteCurrentXml("Buttons", buttonsData);
            }
        }

        private void numericUpDown7_Validated(object sender, EventArgs e)  // Bring Up Graphic Every X seconds up/Down Validated
        {
            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Repeat_Graphic_Seconds"].Index].Value = numericUpDown7.Value.ToString();

            // Update Buttons file from info in buttonsData
            GlobalConfig cfg = GlobalConfig.Instance;
            cfg.WriteCurrentXml("Buttons", buttonsData);

            // MessageBox.Show("Graphic Repeat Time Of " + numericUpDown7.Value.ToString() + " Seconds Entered.");
            // buttonsData.Rows[nRow].Cells[buttonsData.Columns["Repeat_Graphic_Seconds"].Index].Value.ToString() == "Yes"
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)  // Leave Graphic Up For X seconds up/Down Validated
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Repeat_Graphic_Seconds"].Index].Value = numericUpDown8.Value.ToString();

            // Update Buttons file from info in buttonsData
            cfg.WriteCurrentXml("Buttons", buttonsData);

            // MessageBox.Show("Graphic Duration Time Of " + numericUpDown8.Value.ToString() + " Seconds Entered.");
        }

        private void button13_Click(object sender, EventArgs e)  // Focus button pressed...
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            AMC.FocusWindowURL = "http://" + cfg.Camera(camNumber).ipAddrPort + "/axis-cgi/autofocus/focuswindow/"; // Path for focus commands
//            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // get camera IP address
//            AMC.FocusWindowURL = "http://" + cameraIPAddress + "/axis-cgi/autofocus/focuswindow/"; // Path for focus commands

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

            // Update buttonsData...
            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Text_Line_1_Source"].Index].Value = "Text";

            // Update Buttons.xml file from info in buttonsData...
            cfg.WriteCurrentXml("Buttons", buttonsData);
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
/*
            GlobalConfig cfg = GlobalConfig.Instance;

            //  Send command to close iris
            int irisStepValue = Convert.ToInt32(irisSteps.Value) * (-10);

            SendCommandToCamera(camerasData, camNumber, "ptz.cgi?riris=" + irisStepValue);
            MessageBox.Show("ptz.cgi?riris =" + irisStepValue);
*/
        }


        // UTILITY FUNCTIONS


        private void AMC_OnError(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent e)  // On AMC error
        {
            MessageBox.Show("Error with AMC viewer on Shot/Graphic Settings Menu ");
/*
            camerasData.Rows[comboBox1.SelectedIndex].Cells[14].Value = "Error!";
            txtCamStatus.BackColor = Color.Red;
            txtCamStatus.Text = (camerasData.Rows[comboBox1.SelectedIndex].Cells[3].Value.ToString()
                + " " + camerasData.Rows[comboBox1.SelectedIndex].Cells[4].Value.ToString()
                + " camera at " + camerasData.Rows[comboBox1.SelectedIndex].Cells[1].Value.ToString() + "\n"
                + camerasData.Rows[comboBox1.SelectedIndex].Cells[14].Value.ToString());  // Put up Manufacturer, Model, IP Address and status...
*/
        }
/*
        private string SendCommandToCamera(DataGridView dataGridView1, int camNumber, string command)  // Send a command to a camera - cmmand does not have api name so it can change
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string response = cfg.Camera(camNumber).CamCommand(command);
/*
            string userName = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["User_Name"].Index].Value.ToString();  // Get camera username from dataGridView1
            string password = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["Password"].Index].Value.ToString();  // Get camera password from camera dataGridView1
            string cameraIPAddress = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString();  // Get IP Address from camera dataGridView1
            var client = new WebClient { Credentials = new NetworkCredential(userName, password) };
            string response = client.DownloadString("http://" + cameraIPAddress + "/axis-cgi/com/" + command);  // 'response' has the position data
*//*
            MessageBox.Show("Camera response is:\n\n" + response);  // TEMPORARY
            return response;
        }
*/

        // TEMPORARY BUTTONS

        private void button2_Click_1(object sender, EventArgs e)  // Temporary pan left
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string pan = "-10";
            string response = cfg.Camera(camNumber).Pan(pan);
/*
            var client = new WebClient { Credentials = new NetworkCredential("root", "root") };
            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // Get IP Address from File
            var response = client.DownloadString("http://" + cameraIPAddress + "/axis-cgi/com/ptz.cgi?rpan=-10");
*/
            response = "Move Left";
            textBox5.Text = response;
        }

        private void button3_Click_1(object sender, EventArgs e)  // Temprary pan right
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string pan = "10";
            string response = cfg.Camera(camNumber).Pan(pan);
/*
            var client = new WebClient { Credentials = new NetworkCredential("root", "root") };
            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // Get IP Address from File
            var response = client.DownloadString("http://" + cameraIPAddress + "/axis-cgi/com/ptz.cgi?rpan=10");
*/
            response = "Move Right";
            textBox5.Text = response;
        }

        private void button7_Click_1(object sender, EventArgs e)  // Temporary query for Position
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string response = cfg.Camera(camNumber).GetPosition();
/*
            var client = new WebClient { Credentials = new NetworkCredential("root", "root") };
            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // Get IP Address from File
            var response = client.DownloadString("http://" + cameraIPAddress + "/axis-cgi/com/ptz.cgi?query=position");
*/
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
                    // MessageBox.Show("Enter Loop");
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
            // MessageBox.Show("Pan is: " + pan + "\n" + "Tilt is: " + tilt + "\n " + "Zoom is: " + zoom);
        }

        private void button8_Click(object sender, EventArgs e)  // Temporary Go To Position
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string pan = "0";
            string tilt = "0";
            string zoom = "5000";
            textBox5.Text = cfg.Camera(camNumber).GoTo(pan, tilt, zoom);
/*
            string userName = camerasData.Rows[camNumber].Cells[camerasData.Columns["User_Name"].Index].Value.ToString();  // get camera user name
            string password = camerasData.Rows[camNumber].Cells[camerasData.Columns["Password"].Index].Value.ToString();  // get camera password
            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // get camera IP address
            var client = new WebClient { Credentials = new NetworkCredential(userName, password) };
            var response = client.DownloadString("http://" + cameraIPAddress + "/axis-cgi/com/ptz.cgi?pan=0&tilt=0&zoom=5000");
            textBox5.Text = response;
*/
        }
    }    
}
