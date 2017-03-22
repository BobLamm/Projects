namespace Video_Test_Fixture
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.camera1Button = new System.Windows.Forms.Button();
            this.camera2Button = new System.Windows.Forms.Button();
            this.addGraphicButton = new System.Windows.Forms.Button();
            this.addTextButton = new System.Windows.Forms.Button();
            this.connectToCameras = new System.Windows.Forms.Button();
            this.pictureBoxForVideo = new AxAXISMEDIACONTROLLib.AxAxisMediaControl();
            this.btnPreview1 = new System.Windows.Forms.Button();
            this.btnPreview2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxForVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // camera1Button
            // 
            this.camera1Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.camera1Button.Location = new System.Drawing.Point(413, 124);
            this.camera1Button.Name = "camera1Button";
            this.camera1Button.Size = new System.Drawing.Size(128, 23);
            this.camera1Button.TabIndex = 1;
            this.camera1Button.Text = "Camera 1";
            this.camera1Button.UseVisualStyleBackColor = true;
            this.camera1Button.Click += new System.EventHandler(this.camera1Button_Click);
            // 
            // camera2Button
            // 
            this.camera2Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.camera2Button.Location = new System.Drawing.Point(413, 153);
            this.camera2Button.Name = "camera2Button";
            this.camera2Button.Size = new System.Drawing.Size(128, 23);
            this.camera2Button.TabIndex = 2;
            this.camera2Button.Text = "Camera 2";
            this.camera2Button.UseVisualStyleBackColor = true;
            this.camera2Button.Click += new System.EventHandler(this.camera2Button_Click);
            // 
            // addGraphicButton
            // 
            this.addGraphicButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addGraphicButton.Location = new System.Drawing.Point(413, 199);
            this.addGraphicButton.Name = "addGraphicButton";
            this.addGraphicButton.Size = new System.Drawing.Size(128, 23);
            this.addGraphicButton.TabIndex = 3;
            this.addGraphicButton.Text = "Superimpose Graphic";
            this.addGraphicButton.UseVisualStyleBackColor = true;
            this.addGraphicButton.Click += new System.EventHandler(this.addGraphicButton_Click);
            // 
            // addTextButton
            // 
            this.addTextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addTextButton.Location = new System.Drawing.Point(413, 228);
            this.addTextButton.Name = "addTextButton";
            this.addTextButton.Size = new System.Drawing.Size(128, 23);
            this.addTextButton.TabIndex = 4;
            this.addTextButton.Text = "Superimpose Text";
            this.addTextButton.UseVisualStyleBackColor = true;
            this.addTextButton.Click += new System.EventHandler(this.addTextButton_Click);
            // 
            // connectToCameras
            // 
            this.connectToCameras.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.connectToCameras.Location = new System.Drawing.Point(413, 12);
            this.connectToCameras.Name = "connectToCameras";
            this.connectToCameras.Size = new System.Drawing.Size(128, 23);
            this.connectToCameras.TabIndex = 5;
            this.connectToCameras.Text = "Connect to Cameras";
            this.connectToCameras.UseVisualStyleBackColor = true;
            this.connectToCameras.Click += new System.EventHandler(this.connectToCameras_Click);
            // 
            // pictureBoxForVideo
            // 
            this.pictureBoxForVideo.Enabled = true;
            this.pictureBoxForVideo.Location = new System.Drawing.Point(13, 12);
            this.pictureBoxForVideo.Name = "pictureBoxForVideo";
            this.pictureBoxForVideo.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("pictureBoxForVideo.OcxState")));
            this.pictureBoxForVideo.Size = new System.Drawing.Size(375, 276);
            this.pictureBoxForVideo.TabIndex = 6;
            this.pictureBoxForVideo.OnClick += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnClickEventHandler(this.pictureBoxForVideo_Click);
            // 
            // btnPreview1
            // 
            this.btnPreview1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview1.Location = new System.Drawing.Point(413, 53);
            this.btnPreview1.Name = "btnPreview1";
            this.btnPreview1.Size = new System.Drawing.Size(128, 23);
            this.btnPreview1.TabIndex = 7;
            this.btnPreview1.Text = "Preview Camera 1";
            this.btnPreview1.UseVisualStyleBackColor = true;
            this.btnPreview1.Click += new System.EventHandler(this.btnPreview1_Click);
            // 
            // btnPreview2
            // 
            this.btnPreview2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview2.Location = new System.Drawing.Point(413, 83);
            this.btnPreview2.Name = "btnPreview2";
            this.btnPreview2.Size = new System.Drawing.Size(128, 23);
            this.btnPreview2.TabIndex = 8;
            this.btnPreview2.Text = "Preview Camera 2";
            this.btnPreview2.UseVisualStyleBackColor = true;
            this.btnPreview2.Click += new System.EventHandler(this.btnPreview2_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 300);
            this.Controls.Add(this.btnPreview2);
            this.Controls.Add(this.btnPreview1);
            this.Controls.Add(this.pictureBoxForVideo);
            this.Controls.Add(this.connectToCameras);
            this.Controls.Add(this.addTextButton);
            this.Controls.Add(this.addGraphicButton);
            this.Controls.Add(this.camera2Button);
            this.Controls.Add(this.camera1Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Video_Test_Fixture";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxForVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button camera1Button;
        private System.Windows.Forms.Button camera2Button;
        private System.Windows.Forms.Button addGraphicButton;
        private System.Windows.Forms.Button addTextButton;
        private System.Windows.Forms.Button connectToCameras;
        private AxAXISMEDIACONTROLLib.AxAxisMediaControl pictureBoxForVideo;
        private System.Windows.Forms.Button btnPreview1;
        private System.Windows.Forms.Button btnPreview2;
    }
}
