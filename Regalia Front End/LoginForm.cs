using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;

namespace Regalia_Front_End
{
    public partial class LoginForm : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection();
        
        public LoginForm()
        {
            InitializeComponent();
            // Initialize the animation manager with the panels
            animationManager = new AnimationManager(this, createAccountPnl, loginAccountPnl);
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Start the create account animation
            animationManager.StartCreateAccountAnimation();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            // Start the login account animation
            animationManager.StartLoginAccountAnimation();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fontPath = Path.Combine(Application.StartupPath, "Fonts", "Roca Two Bold.ttf");
            pfc.AddFontFile(fontPath);
            Regalia.Font = new Font(pfc.Families[0], 48, FontStyle.Regular);
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void Regalia_Click(object sender, EventArgs e)
        {

        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ShadowPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void createAccountLbl_Click(object sender, EventArgs e)
        {

        }

        private void usernameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void confirmPwdTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void emailTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createCloseBtn_Click(object sender, EventArgs e)
        {
            animationManager.CloseCreateAccount();
        }

        private void loginCloseBtn_Click(object sender, EventArgs e)
        {
            animationManager.CloseLoginAccount();
        }

        // Event handlers for login panel controls
        private void loginAccountLbl_Click(object sender, EventArgs e)
        {

        }

        private void loginUsernameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginPasswordTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginShowPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void submitLoginBtn_Click(object sender, EventArgs e)
        {
            // Hardcoded login validation
            string username = loginUsernameTxtBox.Text.Trim();
            string password = loginPasswordTxtBox.Text.Trim();

            if (username == "test" && password == "kiko")
            {
                // Login successful
                MessageBox.Show("Login successful! Welcome!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Close the login form and show Principle form
                this.Hide();
                
                // Open Principal form
                Principal principalForm = new Principal();
                principalForm.ShowDialog();
                
                // Close the login form completely
                this.Close();
            }
            else
            {
                // Login failed
                MessageBox.Show("Invalid username or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Clear the password field
                loginPasswordTxtBox.Clear();
                loginUsernameTxtBox.Focus();
            }
        }

    }
}