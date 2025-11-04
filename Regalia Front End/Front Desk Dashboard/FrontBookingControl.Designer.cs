namespace Regalia_Front_End.Front_Desk_Dashboard
{
    partial class FrontBookingControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Dispose method is handled in FrontBookingControl.cs to manage custom resources

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cameraPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.qzz = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.camerScanner = new System.Windows.Forms.PictureBox();
            this.closeScanner = new Guna.UI2.WinForms.Guna2CircleButton();
            this.scannerStatus = new System.Windows.Forms.Label();
            this.cameraPanel.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.camerScanner)).BeginInit();
            this.SuspendLayout();
            // 
            // cameraPanel
            // 
            this.cameraPanel.BackColor = System.Drawing.Color.White;
            this.cameraPanel.Controls.Add(this.qzz);
            this.cameraPanel.Controls.Add(this.guna2Panel1);
            this.cameraPanel.Controls.Add(this.closeScanner);
            this.cameraPanel.Controls.Add(this.scannerStatus);
            this.cameraPanel.FillColor = System.Drawing.Color.White;
            this.cameraPanel.Location = new System.Drawing.Point(181, 3);
            this.cameraPanel.Name = "cameraPanel";
            this.cameraPanel.ShadowColor = System.Drawing.Color.Black;
            this.cameraPanel.ShadowDepth = 20;
            this.cameraPanel.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.Dropped;
            this.cameraPanel.Size = new System.Drawing.Size(592, 446);
            this.cameraPanel.TabIndex = 15;
            this.cameraPanel.Visible = false;
            // 
            // qzz
            // 
            this.qzz.AutoSize = true;
            this.qzz.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qzz.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.qzz.Location = new System.Drawing.Point(175, 359);
            this.qzz.Name = "qzz";
            this.qzz.Size = new System.Drawing.Size(257, 36);
            this.qzz.TabIndex = 17;
            this.qzz.Text = "Scan the QR code";
            this.qzz.Click += new System.EventHandler(this.label1_Click);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.guna2Panel1.BorderRadius = 5;
            this.guna2Panel1.BorderThickness = 5;
            this.guna2Panel1.Controls.Add(this.camerScanner);
            this.guna2Panel1.Location = new System.Drawing.Point(131, 61);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(342, 285);
            this.guna2Panel1.TabIndex = 16;
            // 
            // camerScanner
            // 
            this.camerScanner.Location = new System.Drawing.Point(3, 3);
            this.camerScanner.Name = "camerScanner";
            this.camerScanner.Size = new System.Drawing.Size(336, 279);
            this.camerScanner.TabIndex = 0;
            this.camerScanner.TabStop = false;
            // 
            // closeScanner
            // 
            this.closeScanner.Animated = true;
            this.closeScanner.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.closeScanner.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.closeScanner.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.closeScanner.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.closeScanner.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.closeScanner.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.closeScanner.ForeColor = System.Drawing.Color.White;
            this.closeScanner.Location = new System.Drawing.Point(545, 3);
            this.closeScanner.Name = "closeScanner";
            this.closeScanner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.closeScanner.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.closeScanner.Size = new System.Drawing.Size(44, 44);
            this.closeScanner.TabIndex = 15;
            this.closeScanner.Text = "X";
            // 
            // scannerStatus
            // 
            this.scannerStatus.AutoSize = true;
            this.scannerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scannerStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.scannerStatus.Location = new System.Drawing.Point(213, 11);
            this.scannerStatus.Name = "scannerStatus";
            this.scannerStatus.Size = new System.Drawing.Size(99, 36);
            this.scannerStatus.TabIndex = 0;
            this.scannerStatus.Text = "Status";
            // 
            // FrontBookingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            // Don't add cameraPanel here - it will be added to parent form at runtime for animation
            // this.Controls.Add(this.cameraPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrontBookingControl";
            this.Size = new System.Drawing.Size(1031, 657);
            this.cameraPanel.ResumeLayout(false);
            this.cameraPanel.PerformLayout();
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.camerScanner)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Guna.UI2.WinForms.Guna2ShadowPanel cameraPanel;
        public Guna.UI2.WinForms.Guna2CircleButton closeScanner;
        public System.Windows.Forms.Label scannerStatus;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label qzz;
        public System.Windows.Forms.PictureBox camerScanner;
    }
}
