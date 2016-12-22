using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections;  // For Axis
// using System.ComponentModel;
using AXISMEDIACONTROLLib;

using System.IO;  // For WebRequests
using System.Net;
using Utility.AxisCamera;

namespace VGV101
{
    // previously Form24
    public partial class DlgSetCameraHome : Form
    {
        private int camNumber = 0;  // To keep track of camera number

        public DlgSetCameraHome()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();
/*
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
        }
        
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)  // Selected Index (Camera) Changed - make note in camNumber, fill in values from Camera_Settings file and put camera image into monitor
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            // Turn camera controls on and identify the camNumber
            label3.Enabled = true;  // Turns on first line of camera control instructions
            label5.Enabled = true;  // Turns on second line of camera control instructions
            groupBox5.Enabled = true;  // Turn on Auto/Manual Iris Radio Buttons
            groupBox4.Enabled = true;  // Turn on Auto/Manual Focus Radio Buttons
            groupBox6.Enabled = true;  // Turn on Auto/Manual White Balance Radio Buttons
            groupBox7.Enabled = true;  // Turn on Backlight On/Off Radio Buttons
            button1.Enabled = true;  // Turn on Save Home Position button
            button16.Enabled = true;  // Turn on Recall Home Position button
            button13.Enabled = true;  // Turn on Focus On This Box button
            button14.Enabled = true;  // Turn on Iris Up button
            button15.Enabled = true;  // Turn on Iris Down button
            numericUpDown10.Enabled = true;  // Turn on Iris Steps numeric updown

            camNumber = comboBox1.SelectedIndex;
            CameraObject camera = cfg.Camera(camNumber);

            AxisCameraUtility.SetUpCameraMonitor(camera,AMC,txtCamStatus);  // Turn on camera monitor and feed camera into it
//            SetUpCameraMonitor(camerasData, camNumber);  // Turn on camera monitor and feed camera into it

            // Put up camera information and status for user to see
            txtCamStatus.Text = camera.manufacturer + " " + camera.model + " camera at " + camera.ipAddrPort + "\n" + camera.status;
/*
            txtCamStatus.Text = (camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["Manufacturer"].Index].Value.ToString()
                + " " + camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["Model"].Index].Value.ToString()
                + " camera at " + camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString()+ "\n"
                + camerasData.Rows[comboBox1.SelectedIndex].Cells[camerasData.Columns["Status"].Index].Value.ToString());  // Put up Manufacturer, Model, IP Address and status...
*/
            txtCamStatus.Visible = true;
//            if (camerasData.Rows[camNumber].Cells[camerasData.Columns["Status"].Index].Value.ToString() != "Full Communication") txtCamStatus.BackColor = Color.Red;
//            if (camerasData.Rows[camNumber].Cells[camerasData.Columns["Status"].Index].Value.ToString() == "Full Communication") txtCamStatus.BackColor = Color.LawnGreen;
        }

        private void button1_Click(object sender, EventArgs e)  // Save Home Position
        {   GlobalConfig cfg = GlobalConfig.Instance;
            CameraObject camera = cfg.Camera(camNumber);

            //  Query for position and put coordinates into 'cameraResponse'
            string cameraResponse = camera.GetPosition();
//            string cameraResponse = SendCommandToCamera(camerasData, camNumber, "ptz.cgi?query=position");

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
                        if (line.Substring(0, "tilt=".Length) == "tilt=") { tilt = line.Substring("tilt=".Length); } // MessageBox.Show("Tilt is: " + tilt); }
                        if (line.Substring(0, "zoom=".Length) == "zoom=") { zoom = line.Substring("zoom=".Length); } // MessageBox.Show("Zoom is: " + zoom); }
                        if (line.Substring(0, "iris=".Length) == "iris=") { iris = line.Substring("iris=".Length); } // MessageBox.Show("Iris is: " + iris); }
                    }
                } while (line != null);
            }

            // Write Position Data into camerasData
            camera.homePanPosition = pan;
            camera.homeTiltPosition = tilt;
            camera.homeZoomPosition = zoom;
/*
            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Pan_Position"].Index].Value = pan;
            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Tilt_Position"].Index].Value = tilt;
            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Zoom_Position"].Index].Value = zoom;
            // camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Focus_Position"].Index].Value = focus;
*/
            MessageBox.Show("Camera: " + camNumber + " Home Position Saved: Pan: " + pan + ", Tilt = " + tilt + ", Zoom = " + zoom);

            // Get current iris position  TEMPORARY IMPLEMENTATION
            string currentIrisPosition = "10";
            // Write Iris Position Data into camerasData
            camera.homeIrisPosition = currentIrisPosition;
//            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Iris_Position"].Index].Value = currentIrisPosition;
            // Current or Auto Iris Setting?
            camera.homeAutoIrisPreset = !radioButton9.Checked;
//            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Auto_Iris_Preset"].Index].Value = (radioButton9.Checked ? "Off" : "On");

            // Get current focus position  TEMPORARY IMPLEMENTATION
            string currentFocusPosition = "20";
            // Write Focus Position Data into camerasData
            camera.homeFocusPosition = currentFocusPosition;
//            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Focus_Position"].Index].Value = currentFocusPosition;
            // Current or Auto Focus Setting?
            camera.homeAutoFocusPreset = !radioButton1.Checked;
//            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Auto_Focus_Preset"].Index].Value = (radioButton1.Checked ? "Off" : "On");

            // Get current focus position  TEMPORARY IMPLEMENTATION
            string currentWhiteBalancePosition = "30";
            // Write White Balance Data into camerasData
            camera.homeWhitePosition = currentWhiteBalancePosition;
            //            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_White_Position"].Index].Value = currentWhiteBalancePosition;
            // Current or Auto White Balance Setting?
            camera.homeAutoWhitePreset = !radioButton11.Checked;
//            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Auto_White_Preset"].Index].Value = (radioButton11.Checked ? "Off" : "On");

            // Write Backlight Setting into camerasData
            // Backlight On or Off?
            camera.homeAutoBacklightPreset = radioButton3.Checked;
//            camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Auto_Backlight_Preset"].Index].Value = (radioButton3.Checked ? "On" : "Off");

            // Update Camera_Settings file from info in camerasData
            cfg.SaveCameras();
//            cfg.WriteCurrentXml("Camera_Settings", camerasData);
        }

        private void button16_Click(object sender, EventArgs e)  // Recall Home Position
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            CameraObject camera = cfg.Camera(camNumber);
/*
            if (!cfg.GetCurrentXml("Camera_Settings", camerasData)) // we can't proceed from here
            {   MessageBox.Show("Camera data not available", "ERROR");
                return;
            }
*/

            string pan = camera.homePanPosition;
            string tilt = camera.homeTiltPosition;
            string zoom = camera.homeZoomPosition;
            camera.GoTo(pan, tilt, zoom);
/*
            string pan = camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Pan_Position"].Index].Value.ToString();
            string tilt = camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Tilt_Position"].Index].Value.ToString();
            string zoom = camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Zoom_Position"].Index].Value.ToString();
            SendCommandToCamera(camerasData, camNumber, "ptz.cgi?pan=" + pan + "&tilt=" + tilt + "&zoom=" + zoom );
 */
            MessageBox.Show("Recalled Camera Number: " + camNumber + " Position: Pan: " + pan + ", Tilt = " + tilt + ", Zoom = " + zoom);
        }

        // UTILITY FUNCTIONS
/*
        private void SetUpCameraMonitor(DataGridView dataGridView1, int camNumber)  // Initialize camera monitor and feed camera into it
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            AMC.Visible = true;
            // MessageBox.Show("Camera Monitor turned on");

            try
            {
                //Stops possible streams
                AMC.Stop();

                // Set the PTZ properties
                AMC.PTZControlURL = "http://" + dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString() + "/axis-cgi/com/ptz.cgi";
                AMC.UIMode = "ptz-relative";  // alternate is "ptz-absolute"

                // Enable PTZ-position presets from AMC context menu
                AMC.PTZPresetURL = "http://" + dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString() + "/axis-cgi/param.cgi?usergroup=anonymous&action=list&group=PTZ.Preset.P0";

                // Enable joystick support
                AMC.EnableJoystick = true;

                // Enable area zoom
                AMC.EnableAreaZoom = true;

                // Enable one-click-zoom
                //AMC.OneClickZoom = true;

                // Set overlay settings
                AMC.EnableOverlays = true;
                AMC.ClientOverlay = (int)AMC_OVERLAY.AMC_OVERLAY_CROSSHAIR | (int)AMC_OVERLAY.AMC_OVERLAY_VECTOR | (int)AMC_OVERLAY.AMC_OVERLAY_ZOOM;

                // Do not show the status bar and the tool bar in the AXIS Media Control
                AMC.ShowStatusBar = false;
                AMC.ShowToolbar = false;
                AMC.StretchToFit = true;
                AMC.MaintainAspectRatio = true;
                AMC.EnableContextMenu = true;
                AMC.ToolbarConfiguration = "+ptz";  // "default,-mute,-volume,+ptz"

                // Set the media URL
                AMC.MediaURL = "http://" + dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString() + "/axis-cgi/mjpg/video.cgi";
                AMC.MediaUsername = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["User_Name"].Index].Value.ToString();
                AMC.MediaPassword = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["Password"].Index].Value.ToString();

                // Start the download of the mjpeg stream from the Axis camera/video server
                AMC.Play();
                txtCamStatus.BackColor = Color.LawnGreen;
            }
            catch (ArgumentException ArgEx)
            {
                MessageBox.Show(ArgEx.Message, " Monitor Error");
                txtCamStatus.BackColor = Color.Red;
                dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["Status"].Index].Value = "Error!";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, " Monitor Error");
                txtCamStatus.BackColor = Color.Red;
                dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["Status"].Index].Value = "Error!";
            }
        }
*/
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
            string userName = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["User_Name"].Index].Value.ToString();  // Get camera username from dataGridView1
            string password = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["Password"].Index].Value.ToString();  // Get camera password from camera dataGridView1
            string cameraIPAddress = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString();  // Get IP Address from camera dataGridView1
            var client = new WebClient { Credentials = new NetworkCredential(userName, password) };
            string response = client.DownloadString("http://" + cameraIPAddress + "/axis-cgi/com/" + command);  // 'response' has the position data
            // MessageBox.Show("Camera response is: \n\n" + response);  // TEMPORARY
            return response;
        }
*/
    }
}
