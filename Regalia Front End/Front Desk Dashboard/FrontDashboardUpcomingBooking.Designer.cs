namespace Regalia_Front_End.Front_Desk_Dashboard
{
    partial class FrontDashboardUpcomingBooking
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
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.frontBookingDate = new System.Windows.Forms.Label();
            this.frontBookingIcon = new System.Windows.Forms.Label();
            this.frontUnitName = new System.Windows.Forms.Label();
            this.frontBookingName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // frontBookingDate
            // 
            this.frontBookingDate.AutoSize = true;
            this.frontBookingDate.BackColor = System.Drawing.Color.Transparent;
            this.frontBookingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frontBookingDate.ForeColor = System.Drawing.Color.White;
            this.frontBookingDate.Location = new System.Drawing.Point(169, 25);
            this.frontBookingDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frontBookingDate.Name = "frontBookingDate";
            this.frontBookingDate.Size = new System.Drawing.Size(48, 24);
            this.frontBookingDate.TabIndex = 1;
            this.frontBookingDate.Text = "Date";
            // 
            // frontBookingIcon
            // 
            this.frontBookingIcon.AutoSize = true;
            this.frontBookingIcon.BackColor = System.Drawing.Color.Transparent;
            this.frontBookingIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frontBookingIcon.ForeColor = System.Drawing.Color.White;
            this.frontBookingIcon.Location = new System.Drawing.Point(143, 25);
            this.frontBookingIcon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frontBookingIcon.Name = "frontBookingIcon";
            this.frontBookingIcon.Size = new System.Drawing.Size(31, 24);
            this.frontBookingIcon.TabIndex = 2;
            this.frontBookingIcon.Text = "📅";
            // 
            // frontUnitName
            // 
            this.frontUnitName.AutoSize = true;
            this.frontUnitName.BackColor = System.Drawing.Color.Transparent;
            this.frontUnitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frontUnitName.ForeColor = System.Drawing.Color.White;
            this.frontUnitName.Location = new System.Drawing.Point(13, 68);
            this.frontUnitName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frontUnitName.Name = "frontUnitName";
            this.frontUnitName.Size = new System.Drawing.Size(57, 24);
            this.frontUnitName.TabIndex = 3;
            this.frontUnitName.Text = "Unit #";
            // 
            // frontBookingName
            // 
            this.frontBookingName.AutoSize = true;
            this.frontBookingName.BackColor = System.Drawing.Color.Transparent;
            this.frontBookingName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frontBookingName.ForeColor = System.Drawing.Color.White;
            this.frontBookingName.Location = new System.Drawing.Point(11, 18);
            this.frontBookingName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frontBookingName.Name = "frontBookingName";
            this.frontBookingName.Size = new System.Drawing.Size(90, 31);
            this.frontBookingName.TabIndex = 4;
            this.frontBookingName.Text = "Name";
            // 
            // FrontDashboardUpcomingBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.frontBookingDate);
            this.Controls.Add(this.frontBookingIcon);
            this.Controls.Add(this.frontUnitName);
            this.Controls.Add(this.frontBookingName);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrontDashboardUpcomingBooking";
            this.Size = new System.Drawing.Size(316, 117);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.Label frontBookingDate;
        private System.Windows.Forms.Label frontBookingIcon;
        private System.Windows.Forms.Label frontUnitName;
        private System.Windows.Forms.Label frontBookingName;
    }
}
