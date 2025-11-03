namespace Regalia_Front_End
{
    partial class LoginForm
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
            // Clean up animation manager
            animationManager?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.guna2CustomGradientPanel1 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.guna2PictureBox4 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox3 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.Regalia = new System.Windows.Forms.Label();
            this.createAccountBtn = new Guna.UI2.WinForms.Guna2GradientButton();
            this.loginBtn = new Guna.UI2.WinForms.Guna2GradientButton();
            this.createAccountPnl = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.createCloseBtn = new Guna.UI2.WinForms.Guna2CircleButton();
            this.submitCreateBtn = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2CheckBox1 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.emailTxtBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.confirmPwdTxtBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.passwordTxtBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.usernameTxtBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.createAccountLbl = new System.Windows.Forms.Label();
            this.loginAccountPnl = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.loginCloseBtn = new Guna.UI2.WinForms.Guna2CircleButton();
            this.submitLoginBtn = new Guna.UI2.WinForms.Guna2GradientButton();
            this.loginShowPasswordCheckBox = new Guna.UI2.WinForms.Guna2CheckBox();
            this.loginPasswordTxtBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.loginUsernameTxtBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.loginAccountLbl = new System.Windows.Forms.Label();
            this.guna2CustomGradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).BeginInit();
            this.createAccountPnl.SuspendLayout();
            this.loginAccountPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 10;
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2ShadowForm1
            // 
            this.guna2ShadowForm1.TargetForm = this;
            // 
            // guna2CustomGradientPanel1
            // 
            this.guna2CustomGradientPanel1.Controls.Add(this.guna2PictureBox4);
            this.guna2CustomGradientPanel1.Controls.Add(this.guna2PictureBox1);
            this.guna2CustomGradientPanel1.Controls.Add(this.guna2PictureBox2);
            this.guna2CustomGradientPanel1.Controls.Add(this.guna2PictureBox3);
            this.guna2CustomGradientPanel1.Controls.Add(this.Regalia);
            this.guna2CustomGradientPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(161)))), ((int)(((byte)(160)))));
            this.guna2CustomGradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(174)))), ((int)(((byte)(144)))));
            this.guna2CustomGradientPanel1.FillColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(188)))), ((int)(((byte)(125)))));
            this.guna2CustomGradientPanel1.FillColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(202)))), ((int)(((byte)(105)))));
            this.guna2CustomGradientPanel1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.guna2CustomGradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            this.guna2CustomGradientPanel1.Size = new System.Drawing.Size(979, 288);
            this.guna2CustomGradientPanel1.TabIndex = 0;
            // 
            // guna2PictureBox4
            // 
            this.guna2PictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox4.Image = global::Regalia_Front_End.Properties.Resources.GLOW;
            this.guna2PictureBox4.ImageRotate = 0F;
            this.guna2PictureBox4.Location = new System.Drawing.Point(161, -147);
            this.guna2PictureBox4.Name = "guna2PictureBox4";
            this.guna2PictureBox4.Size = new System.Drawing.Size(700, 493);
            this.guna2PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox4.TabIndex = 4;
            this.guna2PictureBox4.TabStop = false;
            this.guna2PictureBox4.UseTransparentBackground = true;
            this.guna2PictureBox4.Click += new System.EventHandler(this.guna2PictureBox4_Click);
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox1.Image = global::Regalia_Front_End.Properties.Resources.logo;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(-771, -227);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(1908, 905);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 0;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.UseTransparentBackground = true;
            this.guna2PictureBox1.Click += new System.EventHandler(this.guna2PictureBox1_Click);
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox2.Image = global::Regalia_Front_End.Properties.Resources.logo;
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(-98, -227);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(1844, 905);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox2.TabIndex = 1;
            this.guna2PictureBox2.TabStop = false;
            this.guna2PictureBox2.UseTransparentBackground = true;
            this.guna2PictureBox2.Click += new System.EventHandler(this.guna2PictureBox2_Click);
            // 
            // guna2PictureBox3
            // 
            this.guna2PictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox3.Image = global::Regalia_Front_End.Properties.Resources.logo;
            this.guna2PictureBox3.ImageRotate = 0F;
            this.guna2PictureBox3.Location = new System.Drawing.Point(-405, 12);
            this.guna2PictureBox3.Name = "guna2PictureBox3";
            this.guna2PictureBox3.Size = new System.Drawing.Size(1844, 484);
            this.guna2PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox3.TabIndex = 2;
            this.guna2PictureBox3.TabStop = false;
            this.guna2PictureBox3.UseTransparentBackground = true;
            this.guna2PictureBox3.Click += new System.EventHandler(this.guna2PictureBox3_Click);
            // 
            // Regalia
            // 
            this.Regalia.BackColor = System.Drawing.Color.Transparent;
            this.Regalia.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Regalia.ForeColor = System.Drawing.Color.White;
            this.Regalia.Location = new System.Drawing.Point(338, 12);
            this.Regalia.Name = "Regalia";
            this.Regalia.Size = new System.Drawing.Size(475, 124);
            this.Regalia.TabIndex = 3;
            this.Regalia.Text = "Regalia";
            this.Regalia.Click += new System.EventHandler(this.Regalia_Click);
            // 
            // createAccountBtn
            // 
            this.createAccountBtn.Animated = true;
            this.createAccountBtn.BorderRadius = 10;
            this.createAccountBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.createAccountBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.createAccountBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.createAccountBtn.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.createAccountBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.createAccountBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(161)))), ((int)(((byte)(160)))));
            this.createAccountBtn.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(202)))), ((int)(((byte)(105)))));
            this.createAccountBtn.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.createAccountBtn.ForeColor = System.Drawing.Color.White;
            this.createAccountBtn.Location = new System.Drawing.Point(369, 352);
            this.createAccountBtn.Name = "createAccountBtn";
            this.createAccountBtn.Size = new System.Drawing.Size(257, 46);
            this.createAccountBtn.TabIndex = 1;
            this.createAccountBtn.Text = "Create Account";
            this.createAccountBtn.Click += new System.EventHandler(this.guna2GradientButton1_Click);
            // 
            // loginBtn
            // 
            this.loginBtn.Animated = true;
            this.loginBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(202)))), ((int)(((byte)(105)))));
            this.loginBtn.BorderRadius = 10;
            this.loginBtn.BorderThickness = 2;
            this.loginBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.loginBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.loginBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.loginBtn.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.loginBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.loginBtn.FillColor = System.Drawing.Color.White;
            this.loginBtn.FillColor2 = System.Drawing.Color.White;
            this.loginBtn.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.loginBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginBtn.Location = new System.Drawing.Point(369, 426);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(257, 46);
            this.loginBtn.TabIndex = 2;
            this.loginBtn.Text = "Login";
            this.loginBtn.Click += new System.EventHandler(this.guna2GradientButton2_Click);
            // 
            // createAccountPnl
            // 
            this.createAccountPnl.BackColor = System.Drawing.Color.Transparent;
            this.createAccountPnl.Controls.Add(this.createCloseBtn);
            this.createAccountPnl.Controls.Add(this.submitCreateBtn);
            this.createAccountPnl.Controls.Add(this.guna2CheckBox1);
            this.createAccountPnl.Controls.Add(this.emailTxtBox);
            this.createAccountPnl.Controls.Add(this.confirmPwdTxtBox);
            this.createAccountPnl.Controls.Add(this.passwordTxtBox);
            this.createAccountPnl.Controls.Add(this.usernameTxtBox);
            this.createAccountPnl.Controls.Add(this.createAccountLbl);
            this.createAccountPnl.FillColor = System.Drawing.Color.White;
            this.createAccountPnl.Location = new System.Drawing.Point(298, 130);
            this.createAccountPnl.Name = "createAccountPnl";
            this.createAccountPnl.ShadowColor = System.Drawing.Color.Black;
            this.createAccountPnl.ShadowDepth = 200;
            this.createAccountPnl.ShadowShift = 7;
            this.createAccountPnl.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.Dropped;
            this.createAccountPnl.Size = new System.Drawing.Size(378, 414);
            this.createAccountPnl.TabIndex = 3;
            this.createAccountPnl.Visible = false;
            // 
            // createCloseBtn
            // 
            this.createCloseBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.createCloseBtn.BorderThickness = 2;
            this.createCloseBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.createCloseBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.createCloseBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.createCloseBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.createCloseBtn.FillColor = System.Drawing.Color.White;
            this.createCloseBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createCloseBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.createCloseBtn.Location = new System.Drawing.Point(331, 3);
            this.createCloseBtn.Name = "createCloseBtn";
            this.createCloseBtn.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.createCloseBtn.Size = new System.Drawing.Size(44, 40);
            this.createCloseBtn.TabIndex = 7;
            this.createCloseBtn.Text = "X";
            this.createCloseBtn.Click += new System.EventHandler(this.createCloseBtn_Click);
            // 
            // submitCreateBtn
            // 
            this.submitCreateBtn.Animated = true;
            this.submitCreateBtn.BorderRadius = 10;
            this.submitCreateBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.submitCreateBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.submitCreateBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.submitCreateBtn.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.submitCreateBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.submitCreateBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(161)))), ((int)(((byte)(160)))));
            this.submitCreateBtn.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(202)))), ((int)(((byte)(105)))));
            this.submitCreateBtn.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.submitCreateBtn.ForeColor = System.Drawing.Color.White;
            this.submitCreateBtn.Location = new System.Drawing.Point(85, 339);
            this.submitCreateBtn.Name = "submitCreateBtn";
            this.submitCreateBtn.Size = new System.Drawing.Size(216, 40);
            this.submitCreateBtn.TabIndex = 6;
            this.submitCreateBtn.Text = "Create Account";
            this.submitCreateBtn.Click += new System.EventHandler(this.submitCreateBtn_Click);
            // 
            // guna2CheckBox1
            // 
            this.guna2CheckBox1.AutoSize = true;
            this.guna2CheckBox1.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.guna2CheckBox1.CheckedState.BorderRadius = 1;
            this.guna2CheckBox1.CheckedState.BorderThickness = 1;
            this.guna2CheckBox1.CheckedState.FillColor = System.Drawing.Color.White;
            this.guna2CheckBox1.CheckMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.guna2CheckBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.guna2CheckBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(200)))), ((int)(((byte)(207)))));
            this.guna2CheckBox1.Location = new System.Drawing.Point(21, 248);
            this.guna2CheckBox1.Name = "guna2CheckBox1";
            this.guna2CheckBox1.Size = new System.Drawing.Size(125, 20);
            this.guna2CheckBox1.TabIndex = 5;
            this.guna2CheckBox1.Text = "Show Password";
            this.guna2CheckBox1.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.guna2CheckBox1.UncheckedState.BorderRadius = 1;
            this.guna2CheckBox1.UncheckedState.BorderThickness = 1;
            this.guna2CheckBox1.UncheckedState.FillColor = System.Drawing.Color.White;
            this.guna2CheckBox1.CheckedChanged += new System.EventHandler(this.guna2CheckBox1_CheckedChanged);
            // 
            // emailTxtBox
            // 
            this.emailTxtBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.emailTxtBox.BorderRadius = 5;
            this.emailTxtBox.BorderThickness = 2;
            this.emailTxtBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.emailTxtBox.DefaultText = "";
            this.emailTxtBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.emailTxtBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.emailTxtBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.emailTxtBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.emailTxtBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.emailTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.emailTxtBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.emailTxtBox.Location = new System.Drawing.Point(21, 284);
            this.emailTxtBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.emailTxtBox.Name = "emailTxtBox";
            this.emailTxtBox.PlaceholderText = "Enter Email";
            this.emailTxtBox.SelectedText = "";
            this.emailTxtBox.Size = new System.Drawing.Size(336, 48);
            this.emailTxtBox.TabIndex = 4;
            this.emailTxtBox.TextChanged += new System.EventHandler(this.emailTxtBox_TextChanged);
            // 
            // confirmPwdTxtBox
            // 
            this.confirmPwdTxtBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.confirmPwdTxtBox.BorderRadius = 5;
            this.confirmPwdTxtBox.BorderThickness = 2;
            this.confirmPwdTxtBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.confirmPwdTxtBox.DefaultText = "";
            this.confirmPwdTxtBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.confirmPwdTxtBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.confirmPwdTxtBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.confirmPwdTxtBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.confirmPwdTxtBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.confirmPwdTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.confirmPwdTxtBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.confirmPwdTxtBox.Location = new System.Drawing.Point(21, 198);
            this.confirmPwdTxtBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.confirmPwdTxtBox.Name = "confirmPwdTxtBox";
            this.confirmPwdTxtBox.PlaceholderText = "Confirm Password";
            this.confirmPwdTxtBox.SelectedText = "";
            this.confirmPwdTxtBox.Size = new System.Drawing.Size(336, 48);
            this.confirmPwdTxtBox.TabIndex = 3;
            this.confirmPwdTxtBox.TextChanged += new System.EventHandler(this.confirmPwdTxtBox_TextChanged);
            // 
            // passwordTxtBox
            // 
            this.passwordTxtBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.passwordTxtBox.BorderRadius = 5;
            this.passwordTxtBox.BorderThickness = 2;
            this.passwordTxtBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.passwordTxtBox.DefaultText = "";
            this.passwordTxtBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.passwordTxtBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.passwordTxtBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.passwordTxtBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.passwordTxtBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.passwordTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.passwordTxtBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.passwordTxtBox.Location = new System.Drawing.Point(21, 142);
            this.passwordTxtBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.passwordTxtBox.Name = "passwordTxtBox";
            this.passwordTxtBox.PlaceholderText = "Enter Password";
            this.passwordTxtBox.SelectedText = "";
            this.passwordTxtBox.Size = new System.Drawing.Size(336, 48);
            this.passwordTxtBox.TabIndex = 2;
            this.passwordTxtBox.TextChanged += new System.EventHandler(this.guna2TextBox1_TextChanged);
            // 
            // usernameTxtBox
            // 
            this.usernameTxtBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.usernameTxtBox.BorderRadius = 5;
            this.usernameTxtBox.BorderThickness = 2;
            this.usernameTxtBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.usernameTxtBox.DefaultText = "";
            this.usernameTxtBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.usernameTxtBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.usernameTxtBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.usernameTxtBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.usernameTxtBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.usernameTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.usernameTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.usernameTxtBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.usernameTxtBox.Location = new System.Drawing.Point(21, 76);
            this.usernameTxtBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.usernameTxtBox.Name = "usernameTxtBox";
            this.usernameTxtBox.PlaceholderText = "Enter Username";
            this.usernameTxtBox.SelectedText = "";
            this.usernameTxtBox.Size = new System.Drawing.Size(336, 48);
            this.usernameTxtBox.TabIndex = 1;
            this.usernameTxtBox.TextChanged += new System.EventHandler(this.usernameTxtBox_TextChanged);
            // 
            // createAccountLbl
            // 
            this.createAccountLbl.AutoSize = true;
            this.createAccountLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createAccountLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.createAccountLbl.Location = new System.Drawing.Point(80, 18);
            this.createAccountLbl.Name = "createAccountLbl";
            this.createAccountLbl.Size = new System.Drawing.Size(229, 25);
            this.createAccountLbl.TabIndex = 0;
            this.createAccountLbl.Text = "Enter Account Credential";
            this.createAccountLbl.Click += new System.EventHandler(this.createAccountLbl_Click);
            // 
            // loginAccountPnl
            // 
            this.loginAccountPnl.BackColor = System.Drawing.Color.Transparent;
            this.loginAccountPnl.Controls.Add(this.loginCloseBtn);
            this.loginAccountPnl.Controls.Add(this.submitLoginBtn);
            this.loginAccountPnl.Controls.Add(this.loginShowPasswordCheckBox);
            this.loginAccountPnl.Controls.Add(this.loginPasswordTxtBox);
            this.loginAccountPnl.Controls.Add(this.loginUsernameTxtBox);
            this.loginAccountPnl.Controls.Add(this.loginAccountLbl);
            this.loginAccountPnl.FillColor = System.Drawing.Color.White;
            this.loginAccountPnl.Location = new System.Drawing.Point(298, 130);
            this.loginAccountPnl.Name = "loginAccountPnl";
            this.loginAccountPnl.ShadowColor = System.Drawing.Color.Black;
            this.loginAccountPnl.ShadowDepth = 200;
            this.loginAccountPnl.ShadowShift = 7;
            this.loginAccountPnl.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.Dropped;
            this.loginAccountPnl.Size = new System.Drawing.Size(378, 342);
            this.loginAccountPnl.TabIndex = 4;
            this.loginAccountPnl.Visible = false;
            // 
            // loginCloseBtn
            // 
            this.loginCloseBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginCloseBtn.BorderThickness = 2;
            this.loginCloseBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.loginCloseBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.loginCloseBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.loginCloseBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.loginCloseBtn.FillColor = System.Drawing.Color.White;
            this.loginCloseBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginCloseBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginCloseBtn.Location = new System.Drawing.Point(331, 3);
            this.loginCloseBtn.Name = "loginCloseBtn";
            this.loginCloseBtn.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.loginCloseBtn.Size = new System.Drawing.Size(44, 40);
            this.loginCloseBtn.TabIndex = 7;
            this.loginCloseBtn.Text = "X";
            this.loginCloseBtn.Click += new System.EventHandler(this.loginCloseBtn_Click);
            // 
            // submitLoginBtn
            // 
            this.submitLoginBtn.Animated = true;
            this.submitLoginBtn.BorderRadius = 10;
            this.submitLoginBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.submitLoginBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.submitLoginBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.submitLoginBtn.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.submitLoginBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.submitLoginBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(161)))), ((int)(((byte)(160)))));
            this.submitLoginBtn.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(202)))), ((int)(((byte)(105)))));
            this.submitLoginBtn.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.submitLoginBtn.ForeColor = System.Drawing.Color.White;
            this.submitLoginBtn.Location = new System.Drawing.Point(85, 250);
            this.submitLoginBtn.Name = "submitLoginBtn";
            this.submitLoginBtn.Size = new System.Drawing.Size(216, 40);
            this.submitLoginBtn.TabIndex = 6;
            this.submitLoginBtn.Text = "Login";
            this.submitLoginBtn.Click += new System.EventHandler(this.submitLoginBtn_Click);
            // 
            // loginShowPasswordCheckBox
            // 
            this.loginShowPasswordCheckBox.AutoSize = true;
            this.loginShowPasswordCheckBox.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginShowPasswordCheckBox.CheckedState.BorderRadius = 1;
            this.loginShowPasswordCheckBox.CheckedState.BorderThickness = 1;
            this.loginShowPasswordCheckBox.CheckedState.FillColor = System.Drawing.Color.White;
            this.loginShowPasswordCheckBox.CheckMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginShowPasswordCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.loginShowPasswordCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(200)))), ((int)(((byte)(207)))));
            this.loginShowPasswordCheckBox.Location = new System.Drawing.Point(21, 196);
            this.loginShowPasswordCheckBox.Name = "loginShowPasswordCheckBox";
            this.loginShowPasswordCheckBox.Size = new System.Drawing.Size(125, 20);
            this.loginShowPasswordCheckBox.TabIndex = 5;
            this.loginShowPasswordCheckBox.Text = "Show Password";
            this.loginShowPasswordCheckBox.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginShowPasswordCheckBox.UncheckedState.BorderRadius = 1;
            this.loginShowPasswordCheckBox.UncheckedState.BorderThickness = 1;
            this.loginShowPasswordCheckBox.UncheckedState.FillColor = System.Drawing.Color.White;
            this.loginShowPasswordCheckBox.CheckedChanged += new System.EventHandler(this.loginShowPasswordCheckBox_CheckedChanged);
            // 
            // loginPasswordTxtBox
            // 
            this.loginPasswordTxtBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginPasswordTxtBox.BorderRadius = 5;
            this.loginPasswordTxtBox.BorderThickness = 2;
            this.loginPasswordTxtBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.loginPasswordTxtBox.DefaultText = "";
            this.loginPasswordTxtBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.loginPasswordTxtBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.loginPasswordTxtBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.loginPasswordTxtBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.loginPasswordTxtBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.loginPasswordTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.loginPasswordTxtBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.loginPasswordTxtBox.Location = new System.Drawing.Point(21, 141);
            this.loginPasswordTxtBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.loginPasswordTxtBox.Name = "loginPasswordTxtBox";
            this.loginPasswordTxtBox.PlaceholderText = "Enter Password";
            this.loginPasswordTxtBox.SelectedText = "";
            this.loginPasswordTxtBox.Size = new System.Drawing.Size(336, 48);
            this.loginPasswordTxtBox.TabIndex = 2;
            this.loginPasswordTxtBox.TextChanged += new System.EventHandler(this.loginPasswordTxtBox_TextChanged);
            // 
            // loginUsernameTxtBox
            // 
            this.loginUsernameTxtBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginUsernameTxtBox.BorderRadius = 5;
            this.loginUsernameTxtBox.BorderThickness = 2;
            this.loginUsernameTxtBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.loginUsernameTxtBox.DefaultText = "";
            this.loginUsernameTxtBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.loginUsernameTxtBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.loginUsernameTxtBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.loginUsernameTxtBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.loginUsernameTxtBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.loginUsernameTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.loginUsernameTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginUsernameTxtBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.loginUsernameTxtBox.Location = new System.Drawing.Point(21, 74);
            this.loginUsernameTxtBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.loginUsernameTxtBox.Name = "loginUsernameTxtBox";
            this.loginUsernameTxtBox.PlaceholderText = "Username or Email";
            this.loginUsernameTxtBox.SelectedText = "";
            this.loginUsernameTxtBox.Size = new System.Drawing.Size(336, 48);
            this.loginUsernameTxtBox.TabIndex = 1;
            this.loginUsernameTxtBox.TextChanged += new System.EventHandler(this.loginUsernameTxtBox_TextChanged);
            // 
            // loginAccountLbl
            // 
            this.loginAccountLbl.AutoSize = true;
            this.loginAccountLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginAccountLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(183)))), ((int)(((byte)(135)))));
            this.loginAccountLbl.Location = new System.Drawing.Point(119, 18);
            this.loginAccountLbl.Name = "loginAccountLbl";
            this.loginAccountLbl.Size = new System.Drawing.Size(137, 25);
            this.loginAccountLbl.TabIndex = 0;
            this.loginAccountLbl.Text = "Login Account";
            this.loginAccountLbl.Click += new System.EventHandler(this.loginAccountLbl_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 541);
            this.Controls.Add(this.loginAccountPnl);
            this.Controls.Add(this.createAccountPnl);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.guna2CustomGradientPanel1);
            this.Controls.Add(this.createAccountBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login Window";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.guna2CustomGradientPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).EndInit();
            this.createAccountPnl.ResumeLayout(false);
            this.createAccountPnl.PerformLayout();
            this.loginAccountPnl.ResumeLayout(false);
            this.loginAccountPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox3;
        private Guna.UI2.WinForms.Guna2GradientButton createAccountBtn;
        private Guna.UI2.WinForms.Guna2GradientButton loginBtn;
        private System.Windows.Forms.Label Regalia;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox4;
        private Guna.UI2.WinForms.Guna2ShadowPanel createAccountPnl;
        private System.Windows.Forms.Label createAccountLbl;
        private Guna.UI2.WinForms.Guna2TextBox usernameTxtBox;
        private Guna.UI2.WinForms.Guna2TextBox passwordTxtBox;
        private Guna.UI2.WinForms.Guna2TextBox confirmPwdTxtBox;
        private Guna.UI2.WinForms.Guna2CheckBox guna2CheckBox1;
        private Guna.UI2.WinForms.Guna2TextBox emailTxtBox;
        private Guna.UI2.WinForms.Guna2GradientButton submitCreateBtn;
        private Guna.UI2.WinForms.Guna2CircleButton createCloseBtn;
        private Guna.UI2.WinForms.Guna2ShadowPanel loginAccountPnl;
        private System.Windows.Forms.Label loginAccountLbl;
        private Guna.UI2.WinForms.Guna2TextBox loginUsernameTxtBox;
        private Guna.UI2.WinForms.Guna2TextBox loginPasswordTxtBox;
        private Guna.UI2.WinForms.Guna2CheckBox loginShowPasswordCheckBox;
        private Guna.UI2.WinForms.Guna2GradientButton submitLoginBtn;
        private Guna.UI2.WinForms.Guna2CircleButton loginCloseBtn;
        private AnimationManager animationManager;
    }
}

