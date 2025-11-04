namespace Regalia_Front_End.Owner_Dashboard
{
    partial class BookingControlCards
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bookingName = new System.Windows.Forms.Label();
            this.bookingUnitName = new System.Windows.Forms.Label();
            this.bookingIcon = new System.Windows.Forms.Label();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.bookingDate = new System.Windows.Forms.Label();
            this.statusDot = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bookingName
            // 
            this.bookingName.AutoSize = true;
            this.bookingName.BackColor = System.Drawing.Color.Transparent;
            this.bookingName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookingName.ForeColor = System.Drawing.Color.White;
            this.bookingName.Location = new System.Drawing.Point(32, 12);
            this.bookingName.Name = "bookingName";
            this.bookingName.Size = new System.Drawing.Size(72, 25);
            this.bookingName.TabIndex = 0;
            this.bookingName.Text = "Name";
            // 
            // bookingUnitName
            // 
            this.bookingUnitName.AutoSize = true;
            this.bookingUnitName.BackColor = System.Drawing.Color.Transparent;
            this.bookingUnitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookingUnitName.ForeColor = System.Drawing.Color.White;
            this.bookingUnitName.Location = new System.Drawing.Point(33, 39);
            this.bookingUnitName.Name = "bookingUnitName";
            this.bookingUnitName.Size = new System.Drawing.Size(46, 18);
            this.bookingUnitName.TabIndex = 0;
            this.bookingUnitName.Text = "Unit #";
            // 
            // bookingIcon
            // 
            this.bookingIcon.AutoSize = true;
            this.bookingIcon.BackColor = System.Drawing.Color.Transparent;
            this.bookingIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookingIcon.ForeColor = System.Drawing.Color.White;
            this.bookingIcon.Location = new System.Drawing.Point(190, 28);
            this.bookingIcon.Name = "bookingIcon";
            this.bookingIcon.Size = new System.Drawing.Size(25, 18);
            this.bookingIcon.TabIndex = 0;
            this.bookingIcon.Text = "📆";
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // bookingDate
            // 
            this.bookingDate.AutoSize = true;
            this.bookingDate.BackColor = System.Drawing.Color.Transparent;
            this.bookingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookingDate.ForeColor = System.Drawing.Color.White;
            this.bookingDate.Location = new System.Drawing.Point(219, 28);
            this.bookingDate.Name = "bookingDate";
            this.bookingDate.Size = new System.Drawing.Size(39, 18);
            this.bookingDate.TabIndex = 0;
            this.bookingDate.Text = "Date";
            // 
            // statusDot
            // 
            this.statusDot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusDot.AutoSize = true;
            this.statusDot.BackColor = System.Drawing.Color.Transparent;
            this.statusDot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusDot.Location = new System.Drawing.Point(671, 27);
            this.statusDot.Name = "statusDot";
            this.statusDot.Size = new System.Drawing.Size(20, 20);
            this.statusDot.TabIndex = 1;
            this.statusDot.Text = "●";
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusLabel.AutoSize = true;
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.White;
            this.statusLabel.Location = new System.Drawing.Point(696, 28);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(50, 18);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Status";
            // 
            // BookingControlCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.statusDot);
            this.Controls.Add(this.bookingDate);
            this.Controls.Add(this.bookingIcon);
            this.Controls.Add(this.bookingUnitName);
            this.Controls.Add(this.bookingName);
            this.Name = "BookingControlCards";
            this.Size = new System.Drawing.Size(868, 83);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label bookingName;
        private System.Windows.Forms.Label bookingUnitName;
        private System.Windows.Forms.Label bookingIcon;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.Label bookingDate;
        private System.Windows.Forms.Label statusDot;
        private System.Windows.Forms.Label statusLabel;
    }
}
