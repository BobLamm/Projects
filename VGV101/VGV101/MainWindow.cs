using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading;
using System.Windows.Forms;

using System.Diagnostics;  // For ProcessStart
using System.IO;  // For ProcessStart

//using System.Data.OleDb;  // For Excel File import

using System.IO.Compression;  // For Zipping Files
// using System.IO.Compression.FileSystem;

namespace VGV101
{
    // previously Form1
    public partial class MainWindow : Form
    {
        // PUBLIC VARIABLES

        public static bool takeToProgram = false;  // To keep track of whether cameras should be switched to Program or Preview
        string monitorDescriptor = "Monitor";  // To feed labels to monitors
        int camera = 0;  // To keep track of which camera (or other source) is being referred to (such as what each monitor is looking at)
        int tally = 1;  // To keep track of which camera is on the air

        // int userButtonNumber;  // To keep track of which user button is being manipulated.

        // Variables for Start Button drag/drop...

        private int offsetX = 0;
        private int offsetY = 0;
        private bool moveButton = false;
        private bool scaleButton = false;
        private bool isDragging = false;
        private bool isScaling = false;

        Button[] btnArray = new BobsButton[14];  // Declare button array = 100 buttons

        public MainWindow()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // Open Buttons File...
            if (!cfg.GetCurrentXml("Buttons", buttonsData)) // we can't proceed from here
            {
                MessageBox.Show("Button data not available","FATAL ERROR");
                this.Close();
                return;
            }

/*            // Read Camera Settings from Camera_Settings.xml file into camerasData...
            if (!cfg.GetCurrentXml("Camera_Settings", camerasData)) // we can't proceed from here
            {
                MessageBox.Show("Camera data not available","FATAL ERROR");
                this.Close();
                return;
            }
*/
            // Set Background, if any
            if (buttonsData.Rows[0].Cells[buttonsData.Columns["Image"].Index].Value.ToString() == "Yes")     // buttonDataGridView.Rows[nRow].Cells[buttonDataGridView.Columns["Name_From_Graphic"].Index].Value.ToString()
            {
                this.BackgroundImage = Image.FromFile(buttonsData.Rows[0].Cells[buttonsData.Columns["Image_Path"].Index].Value.ToString().Replace("%MEDIA_ROOT%", cfg.MediaRoot));
            }
            else
            {
                this.BackgroundImage = null;
            }

            // Set Start button (Button 1) location, size, color, etc.

            button1.Left = int.Parse(buttonsData.Rows[1].Cells[buttonsData.Columns["Location_X"].Index].Value.ToString());
            button1.Top = int.Parse(buttonsData.Rows[1].Cells[buttonsData.Columns["Location_Y"].Index].Value.ToString());
            button1.Width = int.Parse(buttonsData.Rows[1].Cells[buttonsData.Columns["Width"].Index].Value.ToString());
            button1.Height = int.Parse(buttonsData.Rows[1].Cells[buttonsData.Columns["Height"].Index].Value.ToString());
            int red = int.Parse(buttonsData.Rows[1].Cells[buttonsData.Columns["Red"].Index].Value.ToString());  // Variables to hold Start Button background color values
            int green = int.Parse(buttonsData.Rows[1].Cells[buttonsData.Columns["Green"].Index].Value.ToString());
            int blue = int.Parse(buttonsData.Rows[1].Cells[buttonsData.Columns["Blue"].Index].Value.ToString());
            button1.BackColor = Color.FromArgb(red, green, blue);  // Start Button background color
            if (buttonsData.Rows[1].Cells[buttonsData.Columns["Image"].Index].Value.ToString() == "Yes")
            {
                button1.BackgroundImage = Image.FromFile(buttonsData.Rows[1].Cells[buttonsData.Columns["Image_Path"].Index].Value.ToString().Replace("%MEDIA_ROOT%", cfg.MediaRoot));
                button1.ForeColor = Color.FromArgb(255, 255, 255);  // Button Text is White
                button1.TextAlign = ContentAlignment.BottomCenter;  // Button Text is aligned along bottom
            }
            else
            {
                button1.ForeColor = Color.FromArgb(0, 0, 0);  // Button text is black
                button1.TextAlign = ContentAlignment.MiddleCenter;  // Button text is centered
            }

            button1.Text = buttonsData.Rows[1].Cells[buttonsData.Columns["Button_Name"].Index].Value.ToString();  // Get button name
            button1.Tag = "Start";    // Put numbers or 'Start' text into tags so we can identify them
        }


        // MAKE USER BUTTONS

        private void Form1_Load(object sender, EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            int xPos = 100;  //  Declare X Position Variable
            int yPos = 200;  //  Declare Y Position Variable
            int red = 0;  // Declare RGB Button Background Color Variables
            int green = 0;
            int blue = 0;

            int n = 2;
            while (n < 14)  //  Set parameters for each button....
            {
                xPos = int.Parse(buttonsData.Rows[n].Cells[buttonsData.Columns["Location_X"].Index].Value.ToString());  //  X Position of button
                yPos = int.Parse(buttonsData.Rows[n].Cells[buttonsData.Columns["Location_Y"].Index].Value.ToString());  //  Y Position of button
                btnArray[n] = new BobsButton(xPos, yPos, buttonsData,n/*, camerasData*/);  // Initialize each button
                btnArray[n].Height = int.Parse(buttonsData.Rows[n].Cells[buttonsData.Columns["Height"].Index].Value.ToString());  // Height of button 
                btnArray[n].Width = int.Parse(buttonsData.Rows[n].Cells[buttonsData.Columns["Width"].Index].Value.ToString());  // Width of button 
                red = int.Parse(buttonsData.Rows[n].Cells[buttonsData.Columns["Red"].Index].Value.ToString());  // Variables to hold background color values
                green = int.Parse(buttonsData.Rows[n].Cells[buttonsData.Columns["Green"].Index].Value.ToString());
                blue = int.Parse(buttonsData.Rows[n].Cells[buttonsData.Columns["Blue"].Index].Value.ToString());
                btnArray[n].BackColor = Color.FromArgb(red, green, blue);  // Button background color
                btnArray[n].BackgroundImageLayout = BackgroundImageLayout = ImageLayout.Stretch;
                if (buttonsData.Rows[n].Cells[13].Value.ToString() == "Yes")
                {
                    btnArray[n].BackgroundImage = Image.FromFile(buttonsData.Rows[n].Cells[buttonsData.Columns["Image_Path"].Index].Value.ToString().Replace("%MEDIA_ROOT%", cfg.MediaRoot));  // works   
                    btnArray[n].ForeColor = Color.FromArgb(255, 255, 255);  // Button Text is White
                    btnArray[n].TextAlign = ContentAlignment.BottomCenter;  // Button Text is aligned along bottom
                }
                btnArray[n].Text = buttonsData.Rows[n].Cells[buttonsData.Columns["Button_Name"].Index].Value.ToString();  // Get button name
                btnArray[n].Tag = n.ToString();   // Put numbers into tags so we can identify them
                if (buttonsData.Rows[n].Cells[buttonsData.Columns["Active"].Index].Value.ToString() == "Yes")  //Is button active (visible)?
                {
                    btnArray[n].Visible = true;  // Button is visible (active)
                }
                else
                {
                    btnArray[n].Visible = false;    // Button is not visible (inactive)
                }
                
                this.Controls.Add(btnArray[n]); // Add button to form 

                n++;
            }
        }


        // STRIP MENU ITEMS - This is the main toolbar at the top of the window.

        private void newToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  File Menu:  New Meeting
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            MessageBox.Show("Erase all buttons and background (loads up Default MTG file).  Keep settings and graphics templates.");

            string startPath = cfg.CfgRoot+"Default\\Default.MTG";  // The MTG file to be opened and imported.
            string destPath = cfg.CfgRoot + "Current";  // where all the configuration XML files should be put
            string backupPath = cfg.CfgRoot + "Backup";  // Place to backup the current settings files

            System.IO.Directory.Delete(backupPath, true);  // Remove old backup directory.
            Directory.Move(destPath, backupPath);  // Rename old destination directory to backup folder
            Directory.CreateDirectory(destPath);  // Make new destination directory
            ZipFile.ExtractToDirectory(startPath, destPath);  // Unzip files to destination directory

            Application.Restart();
            Application.ExitThread();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) // Strip Menu Item:  File Menu:  Open Meeting File Dialog Box
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GlobalConfig cfg = GlobalConfig.Instance;

                string startPath = openFileDialog1.FileName;  // The MTG file to be opened and imported.
                string destPath = cfg.CfgRoot + "Current";  // where all the configuration XML files should be put
                string backupPath = cfg.CfgRoot + "Backup";  // Place to backup the current settings files

                System.IO.Directory.Delete(backupPath, true);  // Remove old backup directory.
                Directory.Move(destPath, backupPath);  // Rename old destination directory to backup folder
                Directory.CreateDirectory(destPath);  // Make new destination directory
                ZipFile.ExtractToDirectory(startPath, destPath);  // Unzip files to destination directory

                Application.Restart();
                Application.ExitThread();
            }
        }

        private void saveMeetingToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:   File Menu:  Save Meeting File Dialog Box
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GlobalConfig cfg = GlobalConfig.Instance;

                string startPath = cfg.CfgRoot + "Current";  // where all the configuration XML files are
                string zipPath = cfg.CfgRoot + "Temp\\Temp.zip";  // temp workspace for creating a zip archive of them
                string destPath = saveFileDialog1.FileName;  // Where the resulting renamed MTG file is wanted.

                // Makes sure the temp Zip file doesn't exist before making a new one
                if (!File.Exists(zipPath))
                {
                    ZipFile.CreateFromDirectory(startPath, zipPath);  // Temp ZIP File doesn't exist
                    // MessageBox.Show("Temp ZIP File Created");
                }
                else   
                {                
                    File.Delete(zipPath);  // Temp ZIP File exists, delete before making a new one.
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                    // MessageBox.Show("Temp ZIP File Deleted and Re-Created");
                }
                File.Copy(zipPath, destPath, true);
            }
        } 

        private void startStreamToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  File Menu:  Start Stream
        {
            MessageBox.Show("Start streaming.  Parameters in Setup/Utilities menu.");
        }

        private void stopStreamToolStripMenuItem_Click(object sender, EventArgs e)    // Strip Menu Item: File Menu:  Stop Stream
        {
            MessageBox.Show("Stop Stream.");
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  File Menu:  Start Recording
        {
            MessageBox.Show("Start Recording.  Parameters in Setup/Utilities menu.  Recording files have meeting name plus date plus number to identify them.");
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item: File Menu:  Stop Recording
        {
            MessageBox.Show("Stop Recording.");
        }

        private void uploadRecordingToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item: File Menu:  Upload Recordings
        {
            MessageBox.Show("Upload Recordings.  Parameters in Setup/Utilities menu.");
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  File Menu:  Quit
        {
            this.Close();
        }

        // Strip Menu Items Continued:  Graphics/Templates Menu

        private void createTemplateToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Graphics/Templates Menu:  Create Template
        {
            DlgGraphicsTemplateEditor frm = new DlgGraphicsTemplateEditor();
            frm.Show();
        }

        private void deleteTemplateToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Graphics/Templates Menu:  Delete Template
        {
            MessageBox.Show("Opens Template Palette for user to delete template.");
        }

        // Strip Menu Items Continued:  People/Items Menu

        private void addPersonToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  People/Items Menu:  Add/Edit Person
        {
            int category = 0;
            int isConnectedToButton = -1;
            DlgListEditor frm = new DlgListEditor(category, isConnectedToButton);
            frm.Show();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  People/Items Menu:  Add/Edit Agenda Item
        {
            int category = 1;
            int isConnectedToButton = -1;
            DlgListEditor frm = new DlgListEditor(category, isConnectedToButton);
            frm.Show();
        }

        private void addNewCategoryToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  People/Items Menu:  Add/Edit Notice
        {
            int category = 2;
            int isConnectedToButton = -1;
            DlgListEditor frm = new DlgListEditor(category, isConnectedToButton);
            frm.Show();
        }

        private void addOtherItemToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  People/Items Menu:  Add/Edit Other Item
        {
            int category = 3;
            int isConnectedToButton = -1;
            DlgListEditor frm = new DlgListEditor(category, isConnectedToButton);
            frm.Show();
        }

        // Strip Menu Items Continued:  Clips/Supers/Stills Menu

        private void addClipToLibraryToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Clips/Supers/Stills Menu:  Play CLip
        {
            MessageBox.Show("Opens File Explorer to allow user to browse for clips, supers (clips with alpha channel) and backgrounds and play them on the touch of a button on a menu with with settings");
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                GlobalConfig cfg = GlobalConfig.Instance;

                // Copies media to the customer media folder at cfg.MediaRoot+"Customer Media"
            }
        }

        private void replaceClipInLibraryToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Clips/Supers/Stills Menu:  Delete Clip/Super/Still
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            MessageBox.Show("Opens Windows Explorer Meeting Media folder so user can see media in it and delete it.");
            Process.Start(cfg.MediaRoot+"Media");
        }

        // Strip Menu Items Continued:  Monitors Menu

        private void camera1ToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Camera 1 Monitor
        {
            camera = 1;
            monitorDescriptor = "CAM 1: ";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void camera2ToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Camera 2 Monitor
        {
            camera = 2;
            monitorDescriptor = "CAM 2: ";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void camera3ToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Camera 3 Monitor
        {
            camera = 3;
            monitorDescriptor = "CAM 3: ";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void camera4ToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Camera 4 Monitor
        {
            camera = 4;
            monitorDescriptor = "CAM 4: ";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void otherVideoSource1ToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Other Video Source 1
        {
            camera = 5;
            monitorDescriptor = "Other Video Source 1: ";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void otherVideoSource2ToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Other Video Source 2 Monitor
        {
            camera = 6;
            monitorDescriptor = "Other Video Source 2: ";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void currentGraphicToolStripMenuItem1_Click(object sender, EventArgs e)  // // Strip Menu Item:  Monitors Menu:  Current Graphic
        {
            camera = 0;
            monitorDescriptor = @"Current Graphic";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void clipSuperStillToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Clip/Super/Still
        {
            camera = 0;
            monitorDescriptor = @"Clips/Supers/Stills";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void previewToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Preview
        {
            camera = 0;
            monitorDescriptor = "Preview";
            DlgSourceMonitor frm = new DlgSourceMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void programToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Program
        {
            camera = 0;
            monitorDescriptor = "Program";
            DlgProgramMonitor frm = new DlgProgramMonitor(camera, monitorDescriptor, tally);
            frm.Show();
        }

        private void freezeWindowsToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Monitors Menu:  Freeze Window
        {
            if (this.freezeWindowsToolStripMenuItem.Text == "Freeze Window")
            {
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.freezeWindowsToolStripMenuItem.Text = "Unfreeze Window";
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.MaximizeBox = true;
                this.MinimizeBox = true;
                this.freezeWindowsToolStripMenuItem.Text = "Freeze Window";
            }
        }

        // Strip Menu Items Continued:  Setup/Utilities Menu

        private void setupWizardToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Setup Wizard
        {
            DlgSetupWizard frm19 = new DlgSetupWizard();
            frm19.Show();




            /*
            // ManualResetEvent showNext = new ManualResetEvent(false);

            Form7 frm1 = new Form7();  // Camera Menu
            frm1.Show();
            // Form19 frm2 = new Form19("Fill in camera information and save. \n \nThen press 'Continue Setup' or 'Abandon Setup'.",showNext);
            // frm2.Show();
            /* showNext.WaitOne();  // THIS DID NOT WORK - need to come back to this...
            // if (frm2.resultCode == Form19.resultType.CONTINUE)
            // {
                showNext.Reset();  
 
                // frm1.Close();
                MessageBox.Show("Sensed Continue Result");
                Form7 frm3 = new Form7();  // Video Sources Menu
                frm3.Show();
                MessageBox.Show("About to put up Message menu");
                Form19 frm4 = new Form19("Fill in video sources information and save. (I've put in the camera menu as a placeholder.)  \n \nThen press 'Continue Setup' or 'Abandon Setup'.",showNext);
                frm4.Show();
                if (frm2.resultCode == Form19.resultType.CONTINUE)
                {
                    // display next form
                } 
            }
            else
            {
                MessageBox.Show("This did not Continue");
            } */
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)  //  Strip Menu Item:  Setup/Utilities Menu:  User Information
        {
             DlgRegistrationPage frm = new DlgRegistrationPage();
             frm.Show();
         }

        private void loadMeetingAtStartupToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Startup Settings
        {
            DlgStartupSettings frm = new DlgStartupSettings();
            frm.Show();
        }

        private void resetCamerasToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Setup Cameras
        {
            DlgCameraSettings frm = new DlgCameraSettings();
            frm.Show();
        }

        private void vIdeoSourcesToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Video Sources
        {
            MessageBox.Show("Puts up Video Sources configuration menu.");
        }

        private void customerMediaToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  User Media
        {
            DlgActiveUserMedia frm = new DlgActiveUserMedia();
            frm.Show();
        }

        private void graphicTemplateMediaToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Graphic Template Media
        {
            MessageBox.Show("Shows list of media associated with graphic templates");
        }

        private void buttonsToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Buttons
        {
            DlgButtonSettings frm = new DlgButtonSettings();
            frm.Show();
        }

        private void monitorsToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Monitors
        {
            DlgMonitorSettings frm = new DlgMonitorSettings();
            frm.Show();
        }

        private void streamingToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Streaming
        {
            MessageBox.Show("Puts up Streaming Menu with configuration and publisher information.");
        }

        private void recordingsToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Recording
        {
            MessageBox.Show("Puts up Recording Menu with configuration and file path information.");
        }

        private void publishingSharingToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Publishing/Sharing
        {
            MessageBox.Show("Puts up Publishing Menu with configuration and file posting information.");
        }

        private void setVerboseLoggingToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Set to normal logging
        {
            MessageBox.Show("Logging set to normal mode.  Errors recorded with associated parameters");
        }

        private void setVerboseLoggingToolStripMenuItem1_Click(object sender, EventArgs e)  // Strip Menu Item:  Setup/Utilities Menu:  Verbose Logging
        {
            MessageBox.Show("Logging set to verbose mode.  All actions recorded.");
        }

        // Strip Menu Items Continued:  Help Menu

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Help Menu:  User Manual
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\VGV\Help Files\VGV 1000 User Manual.pdf");
        }

        private void vGVWebSiteToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Help Menu:  Call up VGV website
        {
            Process.Start("IExplore.exe", "http://CityCouncilVideo.com");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)  // Strip Menu Item:  Help Menu:  : About
        {
            DlgAboutBox frm = new DlgAboutBox();
            frm.Show();
        }
        

        //  CONTEXT MENU 1 - Clicking on background to make new buttons and changing the background

        private void createOpeningButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create Opening Button
        {
            MessageBox.Show("Same as clip button but has provisions to put in date and editable meeting info");
        }

        private void createShotGraphicButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create Shot/Graphic Button
        {
            int theRow = 1;
            int n = 0;

            while (n < 100)  // Search for an inactive button...
            {
                if (buttonsData.Rows[n].Cells[buttonsData.Columns["Active"].Index].Value.ToString() == "Yes")  //Is button active (visible)?
                {
                    MessageBox.Show("Button Number " + n + " is active, go to next one");
                }
                else
                {
                    MessageBox.Show("Button Number " + n + " is available");

                    theRow = n;

                    btnArray[n].Visible = true;  // Button is visible (active)
                    buttonsData.Rows[n].Cells[buttonsData.Columns["Active"].Index].Value = "Yes";
                    btnArray[n].Height = 100;  // Button is 100 pixels square
                    buttonsData.Rows[n].Cells[buttonsData.Columns["Height"].Index].Value = 100;

                    btnArray[n].Width = 100;
                    btnArray[n].BackColor = Color.FromArgb(128, 128, 128);  // Button background color
                    btnArray[n].Text = "New Button";  // Button Name
                    btnArray[n].ForeColor = Color.FromArgb(0, 0, 255);  // Button Text is Black

                    // Update the Button.xml file
                    GlobalConfig cfg = GlobalConfig.Instance;
                    cfg.WriteCurrentXml("Buttons", buttonsData);

                    /*
                        <Button_Number>0</Button_Number>
                    */
                    buttonsData.Rows[n].Cells[buttonsData.Columns["Active"].Index].Value = "Yes";
                    buttonsData.Rows[n].Cells[buttonsData.Columns["Button_Type"].Index].Value = "Shot";
                    /*
                     * 
                    <Button_Type>Shot</Button_Type>
                    <Button_Name>New Button</Button_Name>
                    <Name_From_Graphic>No</Name_From_Graphic>
                    <Location_X>100</Location_X>
                    <Location_Y>100</Location_Y>
                    <Width>100</Width>
                    <Height>100</Height>
                    <Uses_Text>No</Uses_Text>
                    <Red>128</Red>
                    <Green>128</Green>
                    <Blue>128</Blue>
                    <Image>No</Image>
                    <Camera_Thumbnail>No</Camera_Thumbnail>
                    <Image_Path>" "</Image_Path>
                    <Image_Transparency>0</Image_Transparency>
                    <Proportion_Lock>Lock</Proportion_Lock>
                    <Primary_Camera_Numberr>1</Primary_Camera_Numberr>
                    <Primary_Camera_Pan_Preset>0</Primary_Camera_Pan_Preset>
                    <Primary_Camera_Tilt_Preset>0</Primary_Camera_Tilt_Preset>
                    <Primary_Camera_Zoom_Preset>0</Primary_Camera_Zoom_Preset>
                    <Primary_Camera_Focus_Preset>0</Primary_Camera_Focus_Preset>
                    <Primary_Camera_Iris_Preset>0</Primary_Camera_Iris_Preset>
                    <Primary_Camera_White_Preset>0</Primary_Camera_White_Preset>
                    <Primary_Camera_Gain_Preset>0</Primary_Camera_Gain_Preset>
                    <Primary_Camera_Backlight_Preset>0</Primary_Camera_Backlight_Preset>
                    <Primary_Camera_Auto_Focus_Preset>On</Primary_Camera_Auto_Focus_Preset>
                    <Primary_Camera_Auto_Iris_Preset>On</Primary_Camera_Auto_Iris_Preset>
                    <Primary_Camera_Auto_White_Preset>On</Primary_Camera_Auto_White_Preset>
                    <Primary_Camera_Auto_Gain_Preset>On</Primary_Camera_Auto_Gain_Preset>
                    <Primary_Camera_Auto_Backlight_Preset>On</Primary_Camera_Auto_Backlight_Preset>
                    <Secondary_Camera_Number>0</Secondary_Camera_Number>
                    <Secondary_Camera_Pan_Preset>0</Secondary_Camera_Pan_Preset>
                    <Secondary_Camera_Tilt_Preset>0</Secondary_Camera_Tilt_Preset>
                    <Secondary_Camera_Zoom_Preset>0</Secondary_Camera_Zoom_Preset>
                    <Secondary_Camera_Focus_Preset>0</Secondary_Camera_Focus_Preset>
                    <Secondary_Camera_Iris_Preset>0</Secondary_Camera_Iris_Preset>
                    <Secondary_Camera_White_Preset>0</Secondary_Camera_White_Preset>
                    <Secondary_Camera_Gain_Preset>0</Secondary_Camera_Gain_Preset>
                    <Secondary_Camera_Backlight_Preset>0</Secondary_Camera_Backlight_Preset>
                    <Secondary_Camera_Auto_Focus_Preset>On</Secondary_Camera_Auto_Focus_Preset>
                    <Secondary_Camera_Auto_Iris_Preset>On</Secondary_Camera_Auto_Iris_Preset>
                    <Secondary_Camera_Auto_White_Preset>On</Secondary_Camera_Auto_White_Preset>
                    <Secondary_Camera_Auto_Gain_Preset>On</Secondary_Camera_Auto_Gain_Preset>
                    <Secondary_Camera_Auto_Backlight_Preset>On</Secondary_Camera_Auto_Backlight_Preset>
                    <Text_Line_1_Source>Text</Text_Line_1_Source>
                    <Text_Line_1>"Line 1 should be empty"</Text_Line_1>
                    <Text_Line_2_Source>Text</Text_Line_2_Source>
                    <Text_Line_2>"Line 2 should be empty"</Text_Line_2>
                    <Text_Line_3_Source>Text</Text_Line_3_Source>
                    <Text_Line_3>"Line 3 should be empty"</Text_Line_3>
                    <Bring_Graphic_Up_Second_Click>Yes</Bring_Graphic_Up_Second_Click>
                    <Repeat_Graphic_Seconds>5</Repeat_Graphic_Seconds>
                    <Leave_Graphic_Seconds>6</Leave_Graphic_Seconds>
                    <Template>Acme Number One</Template>




                btnArray[n].Active = "Yes";
                btnArray[n].Button_Type = "Shot";
                btnArray[n].Button_Name = "New Button";
                btnArray[n].Name_From_Graphic = "No";
                btnArray[n].Location_X = 10;
                btnArray[n].Location_Y = 10;
                btnArray[n].Width = 100;
                btnArray[n].Height = 100;
                btnArray[n].Uses_Text = "No";
                btnArray[n].Red = 128;
                btnArray[n].Green = 128;
                btnArray[n].Blue = 128;
                btnArray[n].Image = "No";
                btnArray[n].Camera_Thumbnail = "No ";
                btnArray[n].Image_Transparency = 0;
                btnArray[n].Proportion_Lock = "Lock";
                btnArray[n].Primary_Camera_Numberr = 1; 
                btnArray[n].Primary_Camera_Pan_Preset = 0;
                btnArray[n].Primary_Camera_Tilt_Preset = 0; 
                btnArray[n].Primary_Camera_Zoom_Preset = 0; 
                btnArray[n].Primary_Camera_Focus_Preset = 0; 
                btnArray[n].Primary_Camera_Iris_Preset = 0; 
                btnArray[n].Primary_Camera_White_Preset = 0; 
                btnArray[n].Primary_Camera_Gain_Preset = 0; 
                btnArray[n].Primary_Camera_Backlight_Preset = 0; 
                btnArray[n].Primary_Camera_Auto_Focus_Preset = "On"; 
                btnArray[n].Primary_Camera_Auto_Iris_Preset = "On"; 
                btnArray[n].Primary_Camera_Auto_White_Preset = "On"; 
                btnArray[n].Primary_Camera_Auto_Gain_Preset = "On"; 
                btnArray[n].Primary_Camera_Auto_Backlight_Preset = "On"; 
                btnArray[n].Secondary_Camera_Number = 0; 
                btnArray[n].Secondary_Camera_Pan_Preset = 0; 
                btnArray[n].Secondary_Camera_Tilt_Preset = 0; 
                btnArray[n].Secondary_Camera_Zoom_Preset = 0; 
                btnArray[n].Secondary_Camera_Focus_Preset = 0; 
                btnArray[n].Secondary_Camera_Iris_Preset = 0; 
                btnArray[n].Secondary_Camera_White_Preset = 0; 
                btnArray[n].Secondary_Camera_Gain_Preset = 0; 
                btnArray[n].Secondary_Camera_Backlight_Preset = 0; 
                btnArray[n].Secondary_Camera_Auto_Focus_Preset = "On"; 
                btnArray[n].Secondary_Camera_Auto_Iris_Preset = "On"; 
                btnArray[n].Secondary_Camera_Auto_White_Preset = "On"; 
                btnArray[n].Secondary_Camera_Auto_Gain_Preset = "On"; 
                btnArray[n].Secondary_Camera_Auto_Backlight_Preset = "On"; 
                btnArray[n].Text_Line_1_Source = "Text"; 
                btnArray[n].Text_Line_1 = "Line 1 should be empty";
                btnArray[n].Text_Line_2_Source = "Text"; 
                btnArray[n].Text_Line_2 = "Line two should be empty"; 
                btnArray[n].Text_Line_3_Source = "Text"; 
                btnArray[n].Text_Line_3 = "Line 3 should be empty"; 
                btnArray[n].Bring_Graphic_Up_Second_Click = "Yes";
                btnArray[n].Repeat_Graphic_Seconds = 5;
                btnArray[n].Leave_Graphic_Seconds = 6;
                btnArray[n].Template ="Default";
                */


                    break;
                }
                n++;
            }

            if (theRow == 1)  // Testing to see if any inactive buttons were found...
            {
                MessageBox.Show("All 100 buttons in use!  Delete some!");  // No inactive buttons found...
            }
            else
            {
                DlgShotGraphicButton frm = new DlgShotGraphicButton(buttonsData, theRow);  // Inactive button found - open menu...
                frm.Show();
            }


        }

        private void createShotButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create Shot Button
        {
            MessageBox.Show("Same as Shot Button but no graphics capability");
        }

        private void createGraphicButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create Graphic Button
        {
            MessageBox.Show("Graphic only - does not move or switch video source");
        }

        private void createExternalVideoSourceButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create External Video Source Button 
        {
            MessageBox.Show("Put up live external source (cannot be aimed) with a (possibly empty) graphic on top");
        }

        private void createClipSuperStillButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create Clip/Super/Still Button
        {
            MessageBox.Show("Menu to choose and play clip/super/still with a (possibly empty) graphic on top");
        }

        private void createCreditsButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create Credits Button
        {
            MessageBox.Show("Make and edit rolling credits at end.");
        }

        private void createFadeToBlackButtonToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu1 Item:  Create Fade-To-Black Button
        {
            MessageBox.Show("Make fade-to-black button with fade rate setting.");
        }
        
        private void addBackgroundToolStripMenuItem_Click(object sender, EventArgs e)  //ContextMenu1 Item:  Change Background
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                // pictureBox1.Image = Image.FromFile(openFileDialog2.FileName);  // Alternative way that doesn't trim picture on top?
                this.BackgroundImage = Image.FromFile(openFileDialog2.FileName);                                  
                buttonsData.Rows[0].Cells[13].Value = "Yes";
                buttonsData.Rows[0].Cells[15].Value = openFileDialog2.FileName;
                                                                            
                // Update the Button.xml file
                GlobalConfig cfg = GlobalConfig.Instance;
                cfg.WriteCurrentXml("Buttons", buttonsData);
            }
        }

        private void removeBackgroundToolStripMenuItem_Click(object sender, EventArgs e)  // ContextMenu1 Item: Remove Background Image
        {
            this.BackgroundImage = null;
            buttonsData.Rows[0].Cells[13].Value = "No";  // "Image"
            buttonsData.Rows[0].Cells[15].Value = " ";  // "Image_Path"

            // Update the Button.xml file
            GlobalConfig cfg = GlobalConfig.Instance;
            cfg.WriteCurrentXml("Buttons", buttonsData);
        }


        // CONTEXT MENU 2 - Right-Clicking on Start Button

        private void editButtonToolStripMenuItem_Click(object sender, EventArgs e)  //ContextMenu2 Item:  Edit Start Settings
        {
            DlgStartupSettingsEntry frm = new DlgStartupSettingsEntry();
            frm.Show();
        }

        private void moveUttonToolStripMenuItem_Click(object sender, EventArgs e)  //ContextMenu 2 Item:  Move Start Button
        {
            moveButton = true;
            scaleButton = false;
            // MessageBox.Show("Start Button Ready To Move!");
        }

        private void resizeButtonToolStripMenuItem_Click(object sender, EventArgs e)  // ContextMenu 2 Item: Scale Start Button
        {
            scaleButton = true;
            moveButton = false;
            // MessageBox.Show("Start Button Ready To Scale!");
        }

        private void buttonColorToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu 2 Item:  Set Start Button Color and update current Button.xml file
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.BackgroundImage = null;
                button1.BackColor = colorDialog1.Color;
                button1.ForeColor = Color.FromArgb(0, 0, 0);  // Button text is black
                button1.TextAlign = ContentAlignment.MiddleCenter;  // Type is centered
                buttonsData.Rows[1].Cells[10].Value = button1.BackColor.R;  // Button RGB Color
                buttonsData.Rows[1].Cells[11].Value = button1.BackColor.G;
                buttonsData.Rows[1].Cells[12].Value = button1.BackColor.B;
                buttonsData.Rows[1].Cells[13].Value = "No";

                // Update the Button.xml file
                GlobalConfig cfg = GlobalConfig.Instance;
                cfg.WriteCurrentXml("Buttons", buttonsData);
            }
        }

        // Context Menu 2 Item:  Set Start Button Image - also saves to buttonsData and Buttons.xml file
        private void buttonImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                button1.BackgroundImage = Image.FromFile(openFileDialog2.FileName);
                buttonsData.Rows[1].Cells[13].Value = "Yes";
                buttonsData.Rows[1].Cells[15].Value = openFileDialog2.FileName;
                button1.Width = button1.BackgroundImage.Width;
                button1.Height = button1.BackgroundImage.Height;
                button1.ForeColor = Color.FromArgb(255, 255, 255);  // Type is white
                button1.TextAlign = ContentAlignment.BottomCenter;  // Type is pushed to bottom

                // Update the Button.xml file
                GlobalConfig cfg = GlobalConfig.Instance;
                cfg.WriteCurrentXml("Buttons", buttonsData);
            }
        }


        // MOVING/SCALING BUTTONS

        // START BUTTON DRAG/DROP/SCALING - moveButton must be TRUE

        private void button1_MouseDown_1(object sender, MouseEventArgs e)  // Start Button Mouse Event Down starts dragging or scaling
        {
            // MessageBox.Show("MouseDown Sensed");
            if (moveButton)
            {
                // MessageBox.Show("MouseDown - Start Mouse Starts Dragging");
                isDragging = true;
                offsetX = e.X;
                offsetY = e.Y;
            }
            if (scaleButton)
            {
                // MessageBox.Show("MouseDown - Start Mouse Starts Scaling");
                isScaling = true;
                offsetX = e.X;
                offsetY = e.Y;
            }
        }

        private void button1_MouseMove_1(object sender, MouseEventArgs e)  // Start button is following mouse
        {
            if ((moveButton) && (isDragging))
            {
                button1.Left = e.X + button1.Left - offsetX;
                button1.Top = e.Y + button1.Top - offsetY;
            }
            if ((scaleButton) && (isScaling))
            {
                button1.Width = e.X;
                button1.Height = e.Y;
            }
        }

        // Start Button dragging/scaling is completed, data in buttonsData is updated and temporary file is written.
        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = isScaling = false;
            buttonsData.Rows[1].Cells[5].Value = button1.Left;
            buttonsData.Rows[1].Cells[6].Value = button1.Top;
            buttonsData.Rows[1].Cells[7].Value = button1.Width;
            buttonsData.Rows[1].Cells[8].Value = button1.Height;

            // Update Buttons file and info in buttonsData
            GlobalConfig cfg = GlobalConfig.Instance;
            cfg.WriteCurrentXml("Buttons", buttonsData);

            // MessageBox.Show("Updated Buttons Temp File");
        }


        // START BUTTON CLICK

        private void button1_Click(object sender, EventArgs e)  // Click on Start Button
        {
            if (!moveButton && !scaleButton)
            {
                MessageBox.Show("Check Media and Execute Startup Sequence.");
            }
        }
    }
}
