namespace Video_Test_Fixture
{
    partial class DlgEditCamConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgEditCamConfig));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblIpAddr = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtIpAddr = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblScanWidth = new System.Windows.Forms.Label();
            this.lblScanLines = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.txtScanWidth = new System.Windows.Forms.TextBox();
            this.txtScanLines = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(278, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(277, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(45, 14);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIpAddr
            // 
            this.lblIpAddr.AutoSize = true;
            this.lblIpAddr.Location = new System.Drawing.Point(22, 44);
            this.lblIpAddr.Name = "lblIpAddr";
            this.lblIpAddr.Size = new System.Drawing.Size(61, 13);
            this.lblIpAddr.TabIndex = 2;
            this.lblIpAddr.Text = "IP Address:";
            this.lblIpAddr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(54, 74);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(89, 11);
            this.txtName.MaxLength = 40;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(162, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtIpAddr
            // 
            this.txtIpAddr.Location = new System.Drawing.Point(89, 41);
            this.txtIpAddr.MaxLength = 15;
            this.txtIpAddr.Name = "txtIpAddr";
            this.txtIpAddr.Size = new System.Drawing.Size(162, 20);
            this.txtIpAddr.TabIndex = 3;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(89, 71);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(66, 20);
            this.txtPort.TabIndex = 5;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(25, 104);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(58, 13);
            this.lblUser.TabIndex = 6;
            this.lblUser.Text = "Username:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(27, 134);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScanWidth
            // 
            this.lblScanWidth.AutoSize = true;
            this.lblScanWidth.Location = new System.Drawing.Point(15, 164);
            this.lblScanWidth.Name = "lblScanWidth";
            this.lblScanWidth.Size = new System.Drawing.Size(68, 13);
            this.lblScanWidth.TabIndex = 10;
            this.lblScanWidth.Text = "Video Width:";
            this.lblScanWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScanLines
            // 
            this.lblScanLines.AutoSize = true;
            this.lblScanLines.Location = new System.Drawing.Point(12, 194);
            this.lblScanLines.Name = "lblScanLines";
            this.lblScanLines.Size = new System.Drawing.Size(71, 13);
            this.lblScanLines.TabIndex = 12;
            this.lblScanLines.Text = "Video Height:";
            this.lblScanLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(89, 101);
            this.txtUserName.MaxLength = 32;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(162, 20);
            this.txtUserName.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(89, 131);
            this.txtPassword.MaxLength = 64;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(162, 20);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(267, 133);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(53, 17);
            this.chkShowPassword.TabIndex = 16;
            this.chkShowPassword.Text = "Show";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // txtScanWidth
            // 
            this.txtScanWidth.Location = new System.Drawing.Point(89, 161);
            this.txtScanWidth.MaxLength = 5;
            this.txtScanWidth.Name = "txtScanWidth";
            this.txtScanWidth.Size = new System.Drawing.Size(66, 20);
            this.txtScanWidth.TabIndex = 11;
            // 
            // txtScanLines
            // 
            this.txtScanLines.Location = new System.Drawing.Point(89, 191);
            this.txtScanLines.MaxLength = 5;
            this.txtScanLines.Name = "txtScanLines";
            this.txtScanLines.Size = new System.Drawing.Size(66, 20);
            this.txtScanLines.TabIndex = 13;
            // 
            // DlgEditCamConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(365, 224);
            this.Controls.Add(this.txtScanLines);
            this.Controls.Add(this.txtScanWidth);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblScanLines);
            this.Controls.Add(this.lblScanWidth);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIpAddr);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIpAddr);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgEditCamConfig";
            this.ShowInTaskbar = false;
            this.Text = "DlgEditCamConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblIpAddr;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtIpAddr;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblScanWidth;
        private System.Windows.Forms.Label lblScanLines;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.TextBox txtScanWidth;
        private System.Windows.Forms.TextBox txtScanLines;
    }
}