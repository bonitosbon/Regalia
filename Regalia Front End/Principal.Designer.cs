namespace Regalia_Front_End
{
    partial class Principal
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
            
            // Cleanup custom managers
            if (disposing)
            {
                propertyFormManager?.Dispose();
                imageManager?.Dispose();
                animationManager?.Dispose();
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
            this.guna2GradientPanel1 = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.addPropertyBtn = new Guna.UI2.WinForms.Guna2Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.settingsBtn = new Guna.UI2.WinForms.Guna2Button();
            this.logOutBtn = new Guna.UI2.WinForms.Guna2Button();
            this.propertiesBtn = new Guna.UI2.WinForms.Guna2Button();
            this.bookingBtn = new Guna.UI2.WinForms.Guna2Button();
            this.dashboardBtn = new Guna.UI2.WinForms.Guna2Button();
            this.guna2GradientPanel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2GradientPanel1
            // 
            this.guna2GradientPanel1.Controls.Add(this.guna2Panel3);
            this.guna2GradientPanel1.Controls.Add(this.guna2Panel2);
            this.guna2GradientPanel1.Controls.Add(this.guna2Panel1);
            this.guna2GradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GradientPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(172)))), ((int)(((byte)(192)))));
            this.guna2GradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(208)))), ((int)(((byte)(119)))));
            this.guna2GradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2GradientPanel1.Name = "guna2GradientPanel1";
            this.guna2GradientPanel1.Size = new System.Drawing.Size(1171, 683);
            this.guna2GradientPanel1.TabIndex = 0;
            this.guna2GradientPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2GradientPanel1_Paint);
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel3.FillColor = System.Drawing.Color.Transparent;
            this.guna2Panel3.Location = new System.Drawing.Point(268, 100);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(903, 583);
            this.guna2Panel3.TabIndex = 2;
            this.guna2Panel3.UseTransparentBackground = true;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.Controls.Add(this.addPropertyBtn);
            this.guna2Panel2.Controls.Add(this.label3);
            this.guna2Panel2.Controls.Add(this.label2);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel2.FillColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.Location = new System.Drawing.Point(268, 0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(903, 100);
            this.guna2Panel2.TabIndex = 1;
            // 
            // addPropertyBtn
            // 
            this.addPropertyBtn.BorderRadius = 5;
            this.addPropertyBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.addPropertyBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.addPropertyBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.addPropertyBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.addPropertyBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.addPropertyBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.addPropertyBtn.ForeColor = System.Drawing.Color.White;
            this.addPropertyBtn.Location = new System.Drawing.Point(637, 22);
            this.addPropertyBtn.Name = "addPropertyBtn";
            this.addPropertyBtn.Size = new System.Drawing.Size(196, 45);
            this.addPropertyBtn.TabIndex = 2;
            this.addPropertyBtn.Text = "➕  Add Property";
            this.addPropertyBtn.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Welcome!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Owner Dashboard";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Controls.Add(this.settingsBtn);
            this.guna2Panel1.Controls.Add(this.logOutBtn);
            this.guna2Panel1.Controls.Add(this.propertiesBtn);
            this.guna2Panel1.Controls.Add(this.bookingBtn);
            this.guna2Panel1.Controls.Add(this.dashboardBtn);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(268, 683);
            this.guna2Panel1.TabIndex = 0;
            this.guna2Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(32, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 42);
            this.label1.TabIndex = 7;
            this.label1.Text = "Regalia";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // settingsBtn
            // 
            this.settingsBtn.Animated = true;
            this.settingsBtn.BorderRadius = 5;
            this.settingsBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.settingsBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.settingsBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.settingsBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.settingsBtn.FillColor = System.Drawing.Color.Transparent;
            this.settingsBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.settingsBtn.ForeColor = System.Drawing.Color.White;
            this.settingsBtn.Location = new System.Drawing.Point(12, 575);
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(241, 45);
            this.settingsBtn.TabIndex = 6;
            this.settingsBtn.Text = "⚙️    Settings";
            this.settingsBtn.UseTransparentBackground = true;
            // 
            // logOutBtn
            // 
            this.logOutBtn.Animated = true;
            this.logOutBtn.BorderRadius = 5;
            this.logOutBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.logOutBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.logOutBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.logOutBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.logOutBtn.FillColor = System.Drawing.Color.Transparent;
            this.logOutBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.logOutBtn.ForeColor = System.Drawing.Color.White;
            this.logOutBtn.Location = new System.Drawing.Point(12, 626);
            this.logOutBtn.Name = "logOutBtn";
            this.logOutBtn.Size = new System.Drawing.Size(241, 45);
            this.logOutBtn.TabIndex = 5;
            this.logOutBtn.Text = "❌   Log out";
            this.logOutBtn.UseTransparentBackground = true;
            this.logOutBtn.Click += new System.EventHandler(this.logOutBtn_Click);
            // 
            // propertiesBtn
            // 
            this.propertiesBtn.Animated = true;
            this.propertiesBtn.BorderRadius = 5;
            this.propertiesBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.propertiesBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.propertiesBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.propertiesBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.propertiesBtn.FillColor = System.Drawing.Color.Transparent;
            this.propertiesBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.propertiesBtn.ForeColor = System.Drawing.Color.White;
            this.propertiesBtn.Location = new System.Drawing.Point(12, 224);
            this.propertiesBtn.Name = "propertiesBtn";
            this.propertiesBtn.Size = new System.Drawing.Size(241, 45);
            this.propertiesBtn.TabIndex = 2;
            this.propertiesBtn.Text = "🏢    Properties";
            this.propertiesBtn.UseTransparentBackground = true;
            this.propertiesBtn.Click += new System.EventHandler(this.PropertiesBtn_Click);
            // 
            // bookingBtn
            // 
            this.bookingBtn.Animated = true;
            this.bookingBtn.BorderRadius = 5;
            this.bookingBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bookingBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bookingBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bookingBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bookingBtn.FillColor = System.Drawing.Color.Transparent;
            this.bookingBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bookingBtn.ForeColor = System.Drawing.Color.White;
            this.bookingBtn.Location = new System.Drawing.Point(12, 173);
            this.bookingBtn.Name = "bookingBtn";
            this.bookingBtn.Size = new System.Drawing.Size(241, 45);
            this.bookingBtn.TabIndex = 1;
            this.bookingBtn.Text = "📄    Booking";
            this.bookingBtn.UseTransparentBackground = true;
            // 
            // dashboardBtn
            // 
            this.dashboardBtn.Animated = true;
            this.dashboardBtn.BorderColor = System.Drawing.Color.Transparent;
            this.dashboardBtn.BorderRadius = 5;
            this.dashboardBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.dashboardBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.dashboardBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.dashboardBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.dashboardBtn.FillColor = System.Drawing.Color.Transparent;
            this.dashboardBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dashboardBtn.ForeColor = System.Drawing.Color.White;
            this.dashboardBtn.IndicateFocus = true;
            this.dashboardBtn.Location = new System.Drawing.Point(12, 122);
            this.dashboardBtn.Name = "dashboardBtn";
            this.dashboardBtn.Size = new System.Drawing.Size(241, 45);
            this.dashboardBtn.TabIndex = 0;
            this.dashboardBtn.Text = "🖥️    Dashboard";
            this.dashboardBtn.UseTransparentBackground = true;
            this.dashboardBtn.Click += new System.EventHandler(this.dashboardBtn_Click);
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(1171, 683);
            this.Controls.Add(this.guna2GradientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Principal";
            this.Text = "Principal";
            this.guna2GradientPanel1.ResumeLayout(false);
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientPanel guna2GradientPanel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button dashboardBtn;
        private Guna.UI2.WinForms.Guna2Button settingsBtn;
        private Guna.UI2.WinForms.Guna2Button logOutBtn;
        private Guna.UI2.WinForms.Guna2Button propertiesBtn;
        private Guna.UI2.WinForms.Guna2Button bookingBtn;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2Button addPropertyBtn;
    }
}