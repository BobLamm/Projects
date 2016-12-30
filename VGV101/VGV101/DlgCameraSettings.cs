using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Text;
using System.Windows.Forms;

//using System.IO;  // For Databasing
//using System.Data.OleDb;  // For Databasing

using System.Net;  // For WebRequests

using AXISMEDIACONTROLLib;

// FIX CREDENTIALS IN FORM2!!!

namespace VGV101
{
    // CAMERA SETTINGS PAGE
    // previously Form7
    public partial class DlgCameraSettings : Form
    {
        public DlgCameraSettings()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // Read Camera Settings from Camera_Settings.xml file into camerasData...
            if (!cfg.GetCurrentXml("Camera_Settings", camerasData)) // we can't proceed from here
            {
                MessageBox.Show("Camera data not available in Form 7: Camera Setup and Diagnostics", "ERROR");
                this.Close();
                return;
            }

            camerasData.RowTemplate.MinimumHeight = 30;

            // Set basic camerasData read/write parameters and background colors
            foreach (DataGridViewColumn column in camerasData.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            // Make particular columns editable
            camerasData.Columns["IP_Address"].DefaultCellStyle.BackColor = Color.White;
            camerasData.Columns["IP_Address"].ReadOnly = false;
            
            camerasData.Columns["User_Name"].DefaultCellStyle.BackColor = Color.White;
            camerasData.Columns["User_Name"].ReadOnly = false;

            camerasData.Columns["Password"].DefaultCellStyle.BackColor = Color.White;
            camerasData.Columns["Password"].ReadOnly = false;

            camerasData.Columns["Camera_Name"].DefaultCellStyle.BackColor = Color.White;
            camerasData.Columns["Camera_Name"].ReadOnly = false;

            camerasData.Columns["Manufacturer"].DefaultCellStyle.BackColor = Color.White;
            camerasData.Columns["Manufacturer"].ReadOnly = false;

            camerasData.Columns["Model"].DefaultCellStyle.BackColor = Color.White;
            camerasData.Columns["Model"].ReadOnly = false;

            camerasData.Columns["Status"].DefaultCellStyle.BackColor = Color.Red;
        }

        private void button17_Click(object sender, EventArgs e)  // Camera 0 Ping Button
        {
            System.Diagnostics.Process.Start(@"C:\windows\system32\CMD.exe", "/c ping " + camerasData.Rows[0].Cells[1].Value.ToString());
        }

        private void button16_Click(object sender, EventArgs e)  // Camera 1 Ping Button
        {
            System.Diagnostics.Process.Start(@"C:\windows\system32\CMD.exe", "/c ping " + camerasData.Rows[1].Cells[1].Value.ToString());
        }

        private void button15_Click(object sender, EventArgs e)  // Camera 2 Ping Button
        {
            System.Diagnostics.Process.Start(@"C:\windows\system32\CMD.exe", "/c ping " + camerasData.Rows[2].Cells[1].Value.ToString());
        }

        private void button14_Click(object sender, EventArgs e)  // Camera 3 Ping Button
        {
            System.Diagnostics.Process.Start(@"C:\windows\system32\CMD.exe", "/c ping " + camerasData.Rows[3].Cells[1].Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)  // Test Camera 0 status and put on monitor
        {
            TestCamera(0);
        }

        private void button6_Click(object sender, EventArgs e)  // Test Camera 1 status and put on monitor
        {
            TestCamera(1);
        }

        private void button9_Click(object sender, EventArgs e)  // Test Camera 2 status and put on monitor
        {
            TestCamera(2);
        }

        private void button12_Click(object sender, EventArgs e)  // Test Camera 3 status and put on monitor
        {
            TestCamera(3);
        }

        private string CameraString(int camNumber)
        {
            return "Camera Number " +
                camerasData.Rows[camNumber].Cells[camerasData.Columns["Camera_Number"].Index].Value.ToString() + ", " +
                camerasData.Rows[camNumber].Cells[camerasData.Columns["Camera_Name"].Index].Value.ToString() + ", " +
                camerasData.Rows[camNumber].Cells[camerasData.Columns["Manufacturer"].Index].Value.ToString() + " " +
                camerasData.Rows[camNumber].Cells[camerasData.Columns["Model"].Index].Value.ToString() + " at IP address: " +
                camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString() + "\n";
        }

        private string CamCommand(int camNumber, string command)
        {
            string userName = camerasData.Rows[camNumber].Cells[camerasData.Columns["User_Name"].Index].Value.ToString();  // get camera user name
            string password = camerasData.Rows[camNumber].Cells[camerasData.Columns["Password"].Index].Value.ToString();  // get camera password
            var client = new WebClient { Credentials = new NetworkCredential(userName, password) };
            return client.DownloadString(command);
        }

        private void TestCamera(int camNumber) // Subroutine that tests camera
        {
            // Start assembling message string...
            string cameraString = CameraString(camNumber);
            MessageBox.Show("Initiating connection with: " + cameraString +  "\n\nClick 'OK' to continue...");

            // Initialize and feed camera into monitor
            SetupViewer(camNumber);  //  This calls a separate subroutine that sets up the viewer

            // Query for information on camera...
            string userName = camerasData.Rows[camNumber].Cells[camerasData.Columns["User_Name"].Index].Value.ToString();  // get camera user name
            string password = camerasData.Rows[camNumber].Cells[camerasData.Columns["Password"].Index].Value.ToString();  // get camera password
            var client = new WebClient { Credentials = new NetworkCredential(userName, password) };

            // Set up parameters for this particular camera
            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // Get IP Address from File
            string cmdRoot = "http://" + cameraIPAddress + "/axis-cgi/";
            string ptzRoot = cmdRoot + "com/ptz.cgi?";
            string paramRoot = cmdRoot + "param.cgi?action=list&group=Properties.";

            string whoami = client.DownloadString(ptzRoot+"whoami=1");
            string positionQuery = client.DownloadString(ptzRoot + "query=position");
            string speedQuery = client.DownloadString(ptzRoot + "query=speed");
            string limitsQuery = client.DownloadString(ptzRoot + "query=limits");
            string infoQuery = client.DownloadString(ptzRoot+"info=1");
            string vapixVersion = client.DownloadString(paramRoot+"API.HTTP.Version");
            string availResolutions = client.DownloadString(paramRoot + "Image.Resolution");
            string currentResolution = client.DownloadString(cmdRoot+"imagesize.cgi?camera=1");
            // string list = client.DownloadString(cmdRoot+"param.cgi?action=list");

            // http://myserver/axis-cgi/param.cgi?action=list&group=ImageSource.*.Name
            // string currentVideoStatus = client.DownloadString(cmdRoot+"videostatus.cgi?status=1,2,3,4");
            // string commandSet = client.DownloadString(ptzRoot+"info=1&camera=1");

            MessageBox.Show("Connected with " + cameraString +
                "\nCamera Firmware Version:\n" + whoami +
                "\nSupported VAPIX Version:\n" + vapixVersion +
                "\nAvailable Resolutions:\n" + availResolutions +
                "\nCurrent Resolution:\n" + currentResolution +
                "\nPosition:\n" + positionQuery +
                "\nSpeed:\n" + speedQuery +
                "\nLimits:\n" + limitsQuery +
                // "\nList:\n" + list +
                // "\nCommand Set:\n" + commandSet +
                "\n");
        }

        private void SetupViewer(int camNumber) // Builds the viewing monitor on the side...
        {
            // Initialize and feed camera into monitor
            try
            {
                AMC.Stop();         // stop any running streams

                string cmdRoot = "http://" + camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString() + "/axis-cgi/";
                // Set the PTZ properties  - XXX DO I WANT TO CHANGE THIS?
                AMC.PTZControlURL = cmdRoot+"com/ptz.cgi";
                AMC.UIMode = "ptz-relative";  // alternate is "ptz-absolute"

                // Enable PTZ-position presets from AMC context menu
                AMC.PTZPresetURL = cmdRoot+"param.cgi?usergroup=anonymous&action=list&group=PTZ.Preset.P0";

                AMC.EnableJoystick =        // Enable joystick support
                AMC.EnableAreaZoom =        // Enable area zoom
                AMC.OneClickZoom = true;    // Enable one-click-zoom

                // Set overlay settings
                AMC.EnableOverlays = true;
                AMC.ClientOverlay = (int)AMC_OVERLAY.AMC_OVERLAY_CROSSHAIR | (int)AMC_OVERLAY.AMC_OVERLAY_VECTOR | (int)AMC_OVERLAY.AMC_OVERLAY_ZOOM;

                // Do not show the status bar and the tool bar in the AXIS Media Control but do allow context menu and other stuff...
                AMC.ShowStatusBar = AMC.ShowToolbar = false;
                AMC.StretchToFit = AMC.MaintainAspectRatio = AMC.EnableContextMenu = true;
                AMC.ToolbarConfiguration = "+ptz";  // "default,-mute,-volume,+ptz"

                // Set the media URL
                AMC.MediaURL = cmdRoot+"mjpg/video.cgi";
                AMC.MediaUsername = camerasData.Rows[camNumber].Cells[camerasData.Columns["User_Name"].Index].Value.ToString();
                AMC.MediaPassword = camerasData.Rows[camNumber].Cells[camerasData.Columns["Password"].Index].Value.ToString();

                // Start the download of the mjpeg stream from the Axis camera/video server
                AMC.Play();
                // label2.BackColor = Color.LawnGreen;
            }
            catch (ArgumentException ArgEx)
            {
                MessageBox.Show(ArgEx.Message, "Argument Exception Error from AMC Setup in Camera Setup and Diagnostics form.");
                // label2.BackColor = Color.Red;
                camerasData.Rows[camNumber].Cells[camerasData.Columns["Status"].Index].Value = "Argument Exception Error from AMC Setup in Camera Setup and Diagnostics form.";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Exception Error from AMC Setup in Camera Setup and Diagnostics form.");
                // label2.BackColor = Color.Red;
                camerasData.Rows[camNumber].Cells[camerasData.Columns["Status"].Index].Value = "Exception Error from AMC Setup in Camera Setup and Diagnostics form.";
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Get Camera 0 Data 
        {
            GetCameraData(0);
        }

        private void button5_Click(object sender, EventArgs e)  // Get Camera 1 Data 
        {
            GetCameraData(1);
        }

        private void button8_Click(object sender, EventArgs e)  // Get Camera 2 Data 
        {
            GetCameraData(2);
        }

        private void button11_Click(object sender, EventArgs e)  // Get Camera 3 Data 
        {
            GetCameraData(3);
        }

        private void GetCameraData(int camNumber) // Subroutine that gets complete set of data from camera and displays it
        {
            // Start assembling message string...
            string cameraString = CameraString(camNumber);
            MessageBox.Show("Initiating connection with: " + cameraString + "\n\nClick 'OK' to continue...");

            // Initialize and feed camera into monitor
            SetupViewer(camNumber);  //  This calls a separate subroutine that sets up the viewer

            // Query for information on camera...
            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // Get IP Address from File
            string list = CamCommand(camNumber,"http://" + cameraIPAddress + "/axis-cgi/param.cgi?action=list");
            
            // Clean up returned data - it doesn't always have clean carriage returns...
            string n = Environment.NewLine;
            string fixedList = list.Replace("\n", n).Replace("\r", n);
            string FinalList = "Connected with " + cameraString + n +n + "Returned camera data is:" + n + n + fixedList;

            // Put up form with data
            DlgDeviceData frm = new DlgDeviceData(FinalList);
            frm.ShowDialog(this);
        }

        private void button3_Click(object sender, EventArgs e)  // Send Camera 0 home...
        {
            GoHome(0);
        }

        private void button4_Click(object sender, EventArgs e)  // Control Camera 2
        {
            GoHome(1);
        }

        private void button7_Click(object sender, EventArgs e)  // Control Camera 3
        {
            GoHome(2);
        }

        private void button10_Click(object sender, EventArgs e)  // Control Camera 4
        {
            GoHome(3);
        }

        private void GoHome(int camNumber) // Subroutine sends camera to home position
        {
            // Initialize and feed camera into monitor
            SetupViewer(camNumber);

            string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // get camera IP address
            string panPosition = camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Pan_Position"].Index].Value.ToString();  // get Home pan position
            string tiltPosition = camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Tilt_Position"].Index].Value.ToString();  // get Home tilt position
            string zoomPosition = camerasData.Rows[camNumber].Cells[camerasData.Columns["Home_Zoom_Position"].Index].Value.ToString();// get Home zoom position
            CamCommand(camNumber,"http://" + cameraIPAddress + "/axis-cgi/com/ptz.cgi?pan=" + panPosition + "&tilt=" + tiltPosition + "&zoom=" + zoomPosition);
        }

        private void button13_Click(object sender, EventArgs e)  // Open Network Settings Folder
        {
            System.Diagnostics.Process.Start("NCPA.cpl");
        }


        // UPDATE CAMERA SETTINGS

        private void button18_Click(object sender, EventArgs e)  // Save Back to File
        {   GlobalConfig cfg = GlobalConfig.Instance;

            // Update Camera Settings file from info in camerasData
            if (cfg.WriteCurrentXml("Camera_Settings", camerasData))
            {
                cfg.ResetCameraConfig();
                // MessageBox.Show("Updated Camera Settings Temp File");
            }

            this.Close();
        }

        private void button19_Click(object sender, EventArgs e)  // Advanced Button makes all columns editable
        {
            if (button19.BackColor == SystemColors.Control)  // Turn on Advanced Mode
            {
                button19.BackColor = Color.Yellow; button19.Text = "Go back to Normal Mode";
                MessageBox.Show("This makes all data editable.  Are you sure you want to do this?  It makes it possible to enter incompatible data in.");
                // Set basic camerasData read/write parameters and background colors
                foreach (DataGridViewColumn column in camerasData.Columns)
                {
                    if (column.HeaderText != "Status") { column.DefaultCellStyle.BackColor = Color.White; column.ReadOnly = false; }
                }

            }
            else
            {
                button19.BackColor = SystemColors.Control; button19.Text = "Advanced";
                // Set basic camerasData read/write parameters and background colors
                foreach (DataGridViewColumn column in camerasData.Columns)
                {
                    if (column.HeaderText != "Status") { column.DefaultCellStyle.BackColor = SystemColors.Control; column.ReadOnly = true; }
                }
                // Make particular columns editable
                camerasData.Columns["IP_Address"].DefaultCellStyle.BackColor = Color.White;
                camerasData.Columns["IP_Address"].ReadOnly = false;
            
                camerasData.Columns["User_Name"].DefaultCellStyle.BackColor = Color.White;
                camerasData.Columns["User_Name"].ReadOnly = false;

                camerasData.Columns["Password"].DefaultCellStyle.BackColor = Color.White;
                camerasData.Columns["Password"].ReadOnly = false;

                camerasData.Columns["Camera_Name"].DefaultCellStyle.BackColor = Color.White;
                camerasData.Columns["Camera_Name"].ReadOnly = false;

                camerasData.Columns["Manufacturer"].DefaultCellStyle.BackColor = Color.White;
                camerasData.Columns["Manufacturer"].ReadOnly = false;

                camerasData.Columns["Model"].DefaultCellStyle.BackColor = Color.White;
                camerasData.Columns["Model"].ReadOnly = false;
            }
        }
    }
}
