namespace Video_Test_Fixture
{
    partial class DlgCamConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCamConfig));
            this.btnNewCamera = new System.Windows.Forms.Button();
            this.btnEditCamera = new System.Windows.Forms.Button();
            this.btnDelCamera = new System.Windows.Forms.Button();
            this.lblCameras = new System.Windows.Forms.Label();
            this.listCameras = new System.Windows.Forms.ListView();
            this.colCamNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIpPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCamName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNewCamera
            // 
            this.btnNewCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewCamera.Location = new System.Drawing.Point(495, 30);
            this.btnNewCamera.Name = "btnNewCamera";
            this.btnNewCamera.Size = new System.Drawing.Size(75, 23);
            this.btnNewCamera.TabIndex = 0;
            this.btnNewCamera.Text = "New...";
            this.btnNewCamera.UseVisualStyleBackColor = true;
            this.btnNewCamera.Click += new System.EventHandler(this.btnNewCamera_Click);
            // 
            // btnEditCamera
            // 
            this.btnEditCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditCamera.Enabled = false;
            this.btnEditCamera.Location = new System.Drawing.Point(495, 60);
            this.btnEditCamera.Name = "btnEditCamera";
            this.btnEditCamera.Size = new System.Drawing.Size(75, 23);
            this.btnEditCamera.TabIndex = 1;
            this.btnEditCamera.Text = "Edit...";
            this.btnEditCamera.UseVisualStyleBackColor = true;
            this.btnEditCamera.Click += new System.EventHandler(this.btnEditCamera_Click);
            // 
            // btnDelCamera
            // 
            this.btnDelCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelCamera.Enabled = false;
            this.btnDelCamera.Location = new System.Drawing.Point(495, 90);
            this.btnDelCamera.Name = "btnDelCamera";
            this.btnDelCamera.Size = new System.Drawing.Size(75, 23);
            this.btnDelCamera.TabIndex = 2;
            this.btnDelCamera.Text = "Delete...";
            this.btnDelCamera.UseVisualStyleBackColor = true;
            this.btnDelCamera.Click += new System.EventHandler(this.btnDelCamera_Click);
            // 
            // lblCameras
            // 
            this.lblCameras.AutoSize = true;
            this.lblCameras.Location = new System.Drawing.Point(13, 13);
            this.lblCameras.Name = "lblCameras";
            this.lblCameras.Size = new System.Drawing.Size(51, 13);
            this.lblCameras.TabIndex = 3;
            this.lblCameras.Text = "Cameras:";
            // 
            // listCameras
            // 
            this.listCameras.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCamNum,
            this.colIpPort,
            this.colCamName,
            this.colSize});
            this.listCameras.Location = new System.Drawing.Point(16, 30);
            this.listCameras.MultiSelect = false;
            this.listCameras.Name = "listCameras";
            this.listCameras.Size = new System.Drawing.Size(458, 86);
            this.listCameras.TabIndex = 4;
            this.listCameras.UseCompatibleStateImageBehavior = false;
            this.listCameras.View = System.Windows.Forms.View.Details;
            this.listCameras.SelectedIndexChanged += new System.EventHandler(this.listCameras_SelectedIndexChanged);
            // 
            // colCamNum
            // 
            this.colCamNum.Text = "Number";
            this.colCamNum.Width = 50;
            // 
            // colIpPort
            // 
            this.colIpPort.Text = "IP:Port";
            this.colIpPort.Width = 155;
            // 
            // colCamName
            // 
            this.colCamName.Text = "Name";
            this.colCamName.Width = 130;
            // 
            // colSize
            // 
            this.colSize.Text = "Size";
            this.colSize.Width = 100;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(576, -10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(1, 1);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // DlgCamConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(586, 131);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listCameras);
            this.Controls.Add(this.lblCameras);
            this.Controls.Add(this.btnDelCamera);
            this.Controls.Add(this.btnEditCamera);
            this.Controls.Add(this.btnNewCamera);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgCamConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Camera Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewCamera;
        private System.Windows.Forms.Button btnEditCamera;
        private System.Windows.Forms.Button btnDelCamera;
        private System.Windows.Forms.Label lblCameras;
        private System.Windows.Forms.ListView listCameras;
        private System.Windows.Forms.ColumnHeader colCamNum;
        private System.Windows.Forms.ColumnHeader colIpPort;
        private System.Windows.Forms.ColumnHeader colCamName;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.Button btnCancel;
    }
}