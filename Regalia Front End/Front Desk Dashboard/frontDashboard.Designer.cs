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
            this.components = new System.ComponentModel.Container();
            this.upcoming = new Guna.UI2.WinForms.Guna2Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.departure = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
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
            this.upcoming.FillColor = System.Drawing.Color.Transparent;
            this.upcoming.Location = new System.Drawing.Point(26, 15);
            this.upcoming.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.upcoming.Name = "upcoming";
            this.upcoming.Size = new System.Drawing.Size(280, 1086);
            this.upcoming.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(46, 11);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(179, 26);
            this.label6.TabIndex = 0;
            this.label6.Text = "Upcoming Arrival";
            // 
            // departure
            // 
            this.departure.AutoScroll = true;
            this.departure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.departure.BorderRadius = 10;
            this.departure.Controls.Add(this.label1);
            this.departure.FillColor = System.Drawing.Color.Transparent;
            this.departure.Location = new System.Drawing.Point(327, 15);
            this.departure.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.departure.Name = "departure";
            this.departure.Size = new System.Drawing.Size(280, 1086);
            this.departure.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(38, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Upcoming Departure";
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this.upcoming;
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.TargetControl = this.departure;
            // 
            // frontDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.departure);
            this.Controls.Add(this.upcoming);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frontDashboard";
            this.Size = new System.Drawing.Size(637, 1115);
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
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse2;
    }
}
