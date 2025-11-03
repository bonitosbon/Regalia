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
            this.SuspendLayout();
            // 
            // bookingName
            // 
            this.bookingName.AutoSize = true;
            this.bookingName.BackColor = System.Drawing.Color.Transparent;
            this.bookingName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookingName.ForeColor = System.Drawing.Color.White;
            this.bookingName.Location = new System.Drawing.Point(31, 17);
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
            this.bookingUnitName.Location = new System.Drawing.Point(33, 51);
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
            this.bookingIcon.Location = new System.Drawing.Point(187, 38);
            this.bookingIcon.Name = "bookingIcon";
            this.bookingIcon.Size = new System.Drawing.Size(25, 18);
            this.bookingIcon.TabIndex = 0;
            this.bookingIcon.Text = "📅";
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
            this.bookingDate.Location = new System.Drawing.Point(212, 38);
            this.bookingDate.Name = "bookingDate";
            this.bookingDate.Size = new System.Drawing.Size(39, 18);
            this.bookingDate.TabIndex = 0;
            this.bookingDate.Text = "Date";
            // 
            // BookingControlCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.bookingDate);
            this.Controls.Add(this.bookingIcon);
            this.Controls.Add(this.bookingUnitName);
            this.Controls.Add(this.bookingName);
            this.Name = "BookingControlCards";
            this.Size = new System.Drawing.Size(671, 95);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label bookingName;
        private System.Windows.Forms.Label bookingUnitName;
        private System.Windows.Forms.Label bookingIcon;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.Label bookingDate;
    }
}
