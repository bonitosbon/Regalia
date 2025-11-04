namespace Regalia_Front_End.Front_Desk_Dashboard
{
    partial class BookingCard
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
            this.frontGuestName = new System.Windows.Forms.Label();
            this.frontUnitName = new System.Windows.Forms.Label();
            this.frontTime = new System.Windows.Forms.Label();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.scannedStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // frontGuestName
            // 
            this.frontGuestName.AutoSize = true;
            this.frontGuestName.BackColor = System.Drawing.Color.Transparent;
            this.frontGuestName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frontGuestName.ForeColor = System.Drawing.Color.White;
            this.frontGuestName.Location = new System.Drawing.Point(14, 7);
            this.frontGuestName.Name = "frontGuestName";
            this.frontGuestName.Size = new System.Drawing.Size(152, 29);
            this.frontGuestName.TabIndex = 0;
            this.frontGuestName.Text = "Guest name";
            // 
            // frontUnitName
            // 
            this.frontUnitName.AutoSize = true;
            this.frontUnitName.BackColor = System.Drawing.Color.Transparent;
            this.frontUnitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frontUnitName.ForeColor = System.Drawing.Color.White;
            this.frontUnitName.Location = new System.Drawing.Point(15, 50);
            this.frontUnitName.Name = "frontUnitName";
            this.frontUnitName.Size = new System.Drawing.Size(82, 20);
            this.frontUnitName.TabIndex = 1;
            this.frontUnitName.Text = "unit name";
            this.frontUnitName.Click += new System.EventHandler(this.frontUnitName_Click);
            // 
            // frontTime
            // 
            this.frontTime.AutoSize = true;
            this.frontTime.BackColor = System.Drawing.Color.Transparent;
            this.frontTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frontTime.ForeColor = System.Drawing.Color.White;
            this.frontTime.Location = new System.Drawing.Point(242, 36);
            this.frontTime.Name = "frontTime";
            this.frontTime.Size = new System.Drawing.Size(131, 20);
            this.frontTime.TabIndex = 2;
            this.frontTime.Text = "arrival/departure";
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // scannedStatus
            // 
            this.scannedStatus.AutoSize = true;
            this.scannedStatus.BackColor = System.Drawing.Color.Transparent;
            this.scannedStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scannedStatus.ForeColor = System.Drawing.Color.White;
            this.scannedStatus.Location = new System.Drawing.Point(570, 36);
            this.scannedStatus.Name = "scannedStatus";
            this.scannedStatus.Size = new System.Drawing.Size(57, 20);
            this.scannedStatus.TabIndex = 3;
            this.scannedStatus.Text = "Status";
            // 
            // BookingCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.scannedStatus);
            this.Controls.Add(this.frontTime);
            this.Controls.Add(this.frontUnitName);
            this.Controls.Add(this.frontGuestName);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "BookingCard";
            this.Size = new System.Drawing.Size(830, 86);
            this.Load += new System.EventHandler(this.BookingCard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label frontGuestName;
        private System.Windows.Forms.Label frontUnitName;
        private System.Windows.Forms.Label frontTime;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.Label scannedStatus;
    }
}
