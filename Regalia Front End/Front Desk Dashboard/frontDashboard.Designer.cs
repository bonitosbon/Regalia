namespace Regalia_Front_End.Front_Desk_Dashboard
{
    partial class frontDashboard
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
            this.upcoming = new Guna.UI2.WinForms.Guna2Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.departure = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.upcoming.SuspendLayout();
            this.departure.SuspendLayout();
            this.SuspendLayout();
            // 
            // upcoming
            // 
            this.upcoming.AutoScroll = true;
            this.upcoming.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.upcoming.BorderRadius = 10;
            this.upcoming.Controls.Add(this.label6);
            this.upcoming.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.upcoming.Location = new System.Drawing.Point(35, 18);
            this.upcoming.Name = "upcoming";
            this.upcoming.Size = new System.Drawing.Size(374, 1336);
            this.upcoming.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(61, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(230, 32);
            this.label6.TabIndex = 0;
            this.label6.Text = "Upcoming Arrival";
            // 
            // departure
            // 
            this.departure.AutoScroll = true;
            this.departure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.departure.BorderRadius = 10;
            this.departure.Controls.Add(this.label1);
            this.departure.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.departure.Location = new System.Drawing.Point(436, 18);
            this.departure.Name = "departure";
            this.departure.Size = new System.Drawing.Size(373, 1336);
            this.departure.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(50, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Upcoming Departure";
            // 
            // frontDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.departure);
            this.Controls.Add(this.upcoming);
            this.Name = "frontDashboard";
            this.Size = new System.Drawing.Size(849, 1372);
            this.upcoming.ResumeLayout(false);
            this.upcoming.PerformLayout();
            this.departure.ResumeLayout(false);
            this.departure.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel upcoming;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2Panel departure;
        private System.Windows.Forms.Label label1;
    }
}
