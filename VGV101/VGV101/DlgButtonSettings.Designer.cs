namespace VGV101
{
    partial class DlgButtonSettings
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button18 = new System.Windows.Forms.Button();
            this.buttonsData = new System.Windows.Forms.DataGridView();
            this.enableEditingButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.buttonsData)).BeginInit();
            this.SuspendLayout();
            // 
            // button18
            // 
            this.button18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button18.Location = new System.Drawing.Point(1653, 713);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(219, 35);
            this.button18.TabIndex = 63;
            this.button18.Text = "Update Buttons (Even more dangerous)";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // buttonsData
            // 
            this.buttonsData.AllowUserToAddRows = false;
            this.buttonsData.AllowUserToDeleteRows = false;
            this.buttonsData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonsData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.buttonsData.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.buttonsData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.buttonsData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.buttonsData.DefaultCellStyle = dataGridViewCellStyle4;
            this.buttonsData.EnableHeadersVisualStyles = false;
            this.buttonsData.Location = new System.Drawing.Point(10, 16);
            this.buttonsData.Name = "buttonsData";
            this.buttonsData.ReadOnly = true;
            this.buttonsData.RowHeadersVisible = false;
            this.buttonsData.RowTemplate.Height = 25;
            this.buttonsData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.buttonsData.Size = new System.Drawing.Size(1862, 679);
            this.buttonsData.TabIndex = 62;
            // 
            // enableEditingButton
            // 
            this.enableEditingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.enableEditingButton.Location = new System.Drawing.Point(1464, 713);
            this.enableEditingButton.Name = "enableEditingButton";
            this.enableEditingButton.Size = new System.Drawing.Size(170, 35);
            this.enableEditingButton.TabIndex = 64;
            this.enableEditingButton.Text = "Enable Editing (Dangerous!)";
            this.enableEditingButton.UseVisualStyleBackColor = true;
            this.enableEditingButton.Click += new System.EventHandler(this.enableEditingButton_Click);
            // 
            // DlgButtonSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1884, 761);
            this.Controls.Add(this.enableEditingButton);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.buttonsData);
            this.Name = "DlgButtonSettings";
            this.Text = "Button Settings";
            ((System.ComponentModel.ISupportInitialize)(this.buttonsData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.DataGridView buttonsData;
        private System.Windows.Forms.Button enableEditingButton;
    }
}