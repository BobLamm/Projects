using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

//using System.IO;  // For WebRequests
//using System.Net;
using System.Text;

namespace VGV101
{
    public class BobsButton : Button
    {
        public int offsetX = 0;
        public int offsetY = 0;
        private bool moveButton = false;
        private bool scaleButton = false;
        private bool isDragging = false;
        private bool isScaling = false;
        private int nRow;  // button number (row in buttonsData)
        private DataGridView buttonsData;  // where the button info is.
//        private DataGridView camerasData;  // where the camera info is.

        public BobsButton(DataGridView dGrid, int row/*, DataGridView dGrid2*/)
        {
            _FinishConstructing(dGrid, row/*, dGrid2*/);
        }

        public BobsButton(int offsetX, int offsetY, DataGridView dGrid,int row/*, DataGridView dGrid2*/)
	    {
            this.Left = this.offsetX = offsetX;
            this.Top = this.offsetY = offsetY;
            _FinishConstructing(dGrid, row/*, dGrid2*/);
	    }

        private void _FinishConstructing(DataGridView dGrid,int row/*, DataGridView dGrid2*/)
        {
            buttonsData = dGrid;  // load up buttonsData with button info that was passed to the function
            nRow = row;  // load up button number
//            camerasData = dGrid2;  // load up camerasData with camera info that was passed to the function

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BobsButtonMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BobsButtonMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BobsButtonMouseUp);

            this.Click += new EventHandler(BobsButtonClick);  // Create Right-Click Button Event Handler 

            this.ContextMenuStrip = new ContextMenuStrip();
            ToolStripItem editor = this.ContextMenuStrip.Items.Add("Edit Button Functions");
            editor.Click += new EventHandler(editButtonToolStripMenuItem_Click);
            ToolStripItem mover = this.ContextMenuStrip.Items.Add("Move Button");
            mover.Click += new EventHandler(moveButtonToolStripMenuItem_Click);
            ToolStripItem scaler = this.ContextMenuStrip.Items.Add("Scale Button");
            scaler.Click += new EventHandler(resizeButtonToolStripMenuItem_Click);
            ToolStripItem colorer = this.ContextMenuStrip.Items.Add("Set Button Color");
            colorer.Click += new EventHandler(buttonColorToolStripMenuItem_Click);
            ToolStripItem imager = this.ContextMenuStrip.Items.Add("Set Button Image");
            imager.Click += new EventHandler(buttonImageToolStripMenuItem_Click);
            ToolStripItem deleter = this.ContextMenuStrip.Items.Add("Delete Button");
            deleter.Click += new EventHandler(buttonDeleteToolStripMenuItem_Click);
        }

        // User Button Mouse Event Down starts dragging or scaling
        private void BobsButtonMouseDown(object sender, MouseEventArgs e)
        {
            // MessageBox.Show("MouseDown Sensed");
            if (moveButton)
            {
                // MessageBox.Show("MouseDown - User Mouse Starts Dragging");
                isDragging = true;
                offsetX = e.X;
                offsetY = e.Y;
            }
            if (scaleButton)
            {
                // MessageBox.Show("MouseDown - User Mouse Starts Scaling");
                isScaling = true;
                offsetX = e.X;
                offsetY = e.Y;
            }
        }

        // User button is following mouse
        private void BobsButtonMouseMove(object sender, MouseEventArgs e)
        {
            if (moveButton && isDragging)
            {
                Left = e.X + Left - offsetX;
                Top = e.Y + Top - offsetY;
            }
            if (scaleButton && isScaling)
            {
                Width = e.X;
                Height = e.Y;
            }
        }

        // User Button dragging/scaling is completed
        private void BobsButtonMouseUp(object sender, MouseEventArgs e)
        {   GlobalConfig cfg = GlobalConfig.Instance;

            moveButton = isDragging = scaleButton = isScaling = false;

            // MessageBox.Show("BobsButton Move/Scale Completed: " + btn.Text);

            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Location_X"].Index].Value = Left;  // buttonsData.Rows[nRow].Cells[5  buttonsData.Columns["Location_X"].Index].Value = Left;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Location_Y"].Index].Value = Top;  // buttonsData.Rows[nRow].Cells[6  buttonsData.Columns["Location_Y"].Index].Value = Top;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Width"].Index].Value = Width;  // buttonsData.Rows[nRow].Cells[7  buttonsData.Columns["Width"].Index].Value = Width;
            buttonsData.Rows[nRow].Cells[buttonsData.Columns["Height"].Index].Value = Height;  // buttonsData.Rows[nRow].Cells[8  buttonsData.Columns["Height"].Index].Value = Height;

            cfg.WriteCurrentXml("Buttons", buttonsData);
        }

        private void BobsButtonClick(object sender, EventArgs e)  // Click on User Button
        {   if (!moveButton && !scaleButton)
            {   GlobalConfig cfg = GlobalConfig.Instance;

                Button btn = (Button)sender;
                string cameraNumber = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Primary_Camera_Numberr"].Index].Value.ToString();
                string pan = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Primary_Camera_Pan_Preset"].Index].Value.ToString();
                string tilt = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Primary_Camera_Tilt_Preset"].Index].Value.ToString();
                string zoom = buttonsData.Rows[nRow].Cells[buttonsData.Columns["Primary_Camera_Zoom_Preset"].Index].Value.ToString();
                // MessageBox.Show("BobsButton " + btn.Tag + " Clicked. Reading Button file:\n\nCamera Number is: " + cameraNumber + "\n\n Coordinates are:\n\n Pan: " + pan + "\n  Tilt: " + tilt + "\n  Zoom: " +zoom);

                int camNumber = Int32.Parse(buttonsData.Rows[nRow].Cells[buttonsData.Columns["Primary_Camera_Numberr"].Index].Value.ToString());  // get camera number from button file
                cfg.Camera(camNumber).GoTo(pan, tilt, zoom);
/*
                // Read Camera Settings from Camera_Settings.xml file into dataGridView2 - needed to get IP address for camera and whatever latest settings are
                if (!cfg.GetCurrentXml("Camera_Settings", camerasData)) // we can't proceed from here
                {
                    MessageBox.Show("Camera data not available in button class","ERROR");
                    return;
                }

                //  Send command to go to position
                int camNumber = Int32.Parse(buttonsData.Rows[nRow].Cells[buttonsData.Columns["Primary_Camera_Numberr"].Index].Value.ToString());  // get camera number from button file
                // MessageBox.Show("Camera Number From Button File Is: " + camNumber + " and the name is: " + dataGridView2.Rows[camNumber].Cells[dataGridView2.Columns["Camera_Name"].Index].Value.ToString());
                string userName = camerasData.Rows[camNumber].Cells[camerasData.Columns["User_Name"].Index].Value.ToString();  // get camera user name from camera file
                // MessageBox.Show("User Name From Camera File is " + userName);
                string password = camerasData.Rows[camNumber].Cells[camerasData.Columns["Password"].Index].Value.ToString();  // get camera password from camera file
                // MessageBox.Show("Password From Camera File Is " + password);
                string cameraIPAddress = camerasData.Rows[camNumber].Cells[camerasData.Columns["IP_Address"].Index].Value.ToString();  // get camera IP address from camera file
                // MessageBox.Show("IP Address From Camera File Is " + cameraIPAddress);
                var client = new WebClient { Credentials = new NetworkCredential(userName, password) };  // put username and password into credentials
                var response = client.DownloadString("http://" + cameraIPAddress + "/axis-cgi/com/ptz.cgi?pan=" + pan + "&tilt=" + tilt + "&zoom=" + zoom);  // make up webrequest to camera
*/
            }
        }

        private void editButtonToolStripMenuItem_Click(object sender, EventArgs e)  //ContextMenu Item:  Edit User Button Shot/Graphic Settings
        {
            DlgShotGraphicButton frm = new DlgShotGraphicButton(buttonsData,nRow);
            frm.Show();
        }

        private void moveButtonToolStripMenuItem_Click(object sender, EventArgs e)  //ContextMenu Item:  Move User Button
        {
            moveButton = true;
            scaleButton = false;
            // MessageBox.Show("Start Button Ready To Move!");
        }

        private void resizeButtonToolStripMenuItem_Click(object sender, EventArgs e)  // ContextMenu Item: Scale User Button
        {
            scaleButton = true;
            moveButton = false;
            // MessageBox.Show("User Button Ready To Scale!");
        }

        private void buttonColorToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu Item:  Set User Button Color
        {
            System.Windows.Forms.ColorDialog colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                BackgroundImage = null;
                BackColor = colorDialog1.Color;
                ForeColor = Color.FromArgb(0, 0, 0);  // Button Text is black
                TextAlign = ContentAlignment.MiddleCenter;  // Text on Button is Centered
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Red"].Index].Value = BackColor.R;  // Button RGB Color  //  buttonsData.Rows[nRow].Cells[10].Value = BackColor.R;
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Green"].Index].Value = BackColor.G;  // buttonsData.Rows[nRow].Cells[11].Value = BackColor.G;
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Blue"].Index].Value = BackColor.B;  // buttonsData.Rows[nRow].Cells[12].Value = BackColor.B;
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Image"].Index].Value = "No";  // No Image  // buttonsData.Rows[nRow].Cells[13].Value = "No";
            }
        }

        private void buttonImageToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu Item:  Set User Button Image
        {
            System.Windows.Forms.OpenFileDialog openFileDialog2 = new OpenFileDialog();
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                GlobalConfig cfg = GlobalConfig.Instance;

                BackgroundImage = Image.FromFile(openFileDialog2.FileName);
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Image"].Index].Value = "Yes";
                buttonsData.Rows[nRow].Cells[buttonsData.Columns["Image_Path"].Index].Value = openFileDialog2.FileName.Replace(cfg.MediaRoot,"%MEDIA_ROOT%");
                Width = BackgroundImage.Width;
                Height = BackgroundImage.Height;
                ForeColor = Color.FromArgb(255, 255, 255);  // Button text is white
                TextAlign = ContentAlignment.BottomCenter;  // Button text is pushed to bottom
            }
        }

        private void buttonDeleteToolStripMenuItem_Click(object sender, EventArgs e)  // Context Menu Item:  Delete User button
        {
            MessageBox.Show("Delete function: " + this.Text);
            this.Hide();
        }
/*
        private void InitializeComponent()
        {
            this.cameraDataGridViewer = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.cameraDataGridViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // cameraDataGridViewer
            // 
            this.cameraDataGridViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraDataGridViewer.Location = new System.Drawing.Point(0, 0);
            this.cameraDataGridViewer.Name = "cameraDataGridViewer";
            this.cameraDataGridViewer.Size = new System.Drawing.Size(240, 150);
            this.cameraDataGridViewer.TabIndex = 0;
//            this.cameraDataGridViewer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CameraDataGridViewer_CellContentClick);
            ((System.ComponentModel.ISupportInitialize)(this.cameraDataGridViewer)).EndInit();
            this.ResumeLayout(false);
        }
*/
    }
}
