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
using Regalia_Front_End.Models;
using Regalia_Front_End.Services;

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

        private async void submitLoginBtn_Click(object sender, EventArgs e)
        {
            string username = loginUsernameTxtBox.Text.Trim();
            string password = loginPasswordTxtBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password!", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Disable button during login attempt
            submitLoginBtn.Enabled = false;
            
            try
            {
                bool loginSuccess = false;
                string welcomeMessage = "Welcome!";

                // Try API login first
                try
                {
                    using (var apiService = new ApiService())
                    {
                        // The input could be either username or email - backend handles both
                        var loginDto = new LoginDto
                        {
                            EmailOrUsername = username, // Backend checks both username and email
                            Password = password
                        };

                        System.Diagnostics.Debug.WriteLine($"Attempting login with EmailOrUsername: {username}");
                        var loginResponse = await apiService.LoginAsync(loginDto);

                        if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                        {
                            loginSuccess = true;
                            welcomeMessage = $"Login successful! Welcome, {loginResponse.User.Username ?? username}!";
                            
                            System.Diagnostics.Debug.WriteLine($"Login successful for user: {loginResponse.User.Username ?? username}");
                            
                            // Check user roles if needed
                            if (loginResponse.User.Roles != null && loginResponse.User.Roles.Count > 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"User roles: {string.Join(", ", loginResponse.User.Roles)}");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Login failed: No token received from API");
                        }
                    }
                }
                catch (Exception apiEx)
                {
                    // API call failed - log the error
                    System.Diagnostics.Debug.WriteLine($"API login failed: {apiEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"Exception type: {apiEx.GetType().Name}");
                    if (apiEx.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner exception: {apiEx.InnerException.Message}");
                    }
                    
                    // If it's an actual authentication error (not network error), show the message
                    string errorMsg = apiEx.Message;
                    if (errorMsg.Contains("Invalid") || errorMsg.Contains("Unauthorized"))
                    {
                        // This is an authentication error, don't fall back to hardcoded
                        MessageBox.Show($"Login failed!\n\n{errorMsg}\n\nPlease check:\n- Username/Email is correct\n- Password is correct\n- Account was created successfully",
                            "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        loginPasswordTxtBox.Clear();
                        loginUsernameTxtBox.Focus();
                        return; // Exit early, don't try hardcoded
                    }
                    // Otherwise, it might be a network error, so we can fall back to hardcoded
                }

                // Fallback to hardcoded login if API login failed (for testing)
                if (!loginSuccess)
                {
                    if (username == "test" && password == "kiko")
                    {
                        loginSuccess = true;
                        welcomeMessage = "Login successful! Welcome! (Using test credentials)";
                    }
                }

                if (loginSuccess)
                {
                    // Login successful
                    MessageBox.Show(welcomeMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Close the login form and show Principal form
                    this.Hide();
                    
                    // Open Principal form
                    Principal principalForm = new Principal();
                    principalForm.ShowDialog();
                    
                    // Close the login form completely
                    this.Close();
                }
                else
                {
                    // Login failed - show detailed error
                    MessageBox.Show($"Login failed!\n\nInvalid username/email or password.\n\nPlease check:\n- You entered the correct username ('{username}') or email\n- Password matches what you used during registration\n- The account was created successfully\n\nTry using your email address if username doesn't work.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    // Clear the password field
                    loginPasswordTxtBox.Clear();
                    loginUsernameTxtBox.Focus();
                }
            }
            finally
            {
                // Re-enable button
                submitLoginBtn.Enabled = true;
            }
        }

        private async void submitCreateBtn_Click(object sender, EventArgs e)
        {
            string username = usernameTxtBox.Text.Trim();
            string email = emailTxtBox.Text.Trim();
            string password = passwordTxtBox.Text.Trim();
            string confirmPassword = confirmPwdTxtBox.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                usernameTxtBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter an email address!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                emailTxtBox.Focus();
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                emailTxtBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a password!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passwordTxtBox.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passwordTxtBox.Focus();
                return;
            }

            // Check for ASP.NET Identity password requirements
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (!hasUpper || !hasLower || !hasDigit)
            {
                MessageBox.Show("Password must contain at least:\n- One uppercase letter\n- One lowercase letter\n- One number\n\nOptional: Special characters (!@#$%^&*)",
                    "Password Requirements", MessageBoxButtons.OK, MessageBoxIcon.Information);
                passwordTxtBox.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                confirmPwdTxtBox.Clear();
                confirmPwdTxtBox.Focus();
                return;
            }

            // Disable button during registration attempt
            submitCreateBtn.Enabled = false;

            try
            {
                // Try API registration
                using (var apiService = new ApiService())
                {
                    var registerDto = new RegisterDto
                    {
                        Username = username,
                        Email = email,
                        Password = password
                    };

                    await apiService.RegisterAsync(registerDto);

                    // Verify account was created by attempting login
                    System.Diagnostics.Debug.WriteLine("Verifying account by attempting login...");
                    try
                    {
                        using (var loginApiService = new ApiService())
                        {
                            var loginDto = new LoginDto
                            {
                                EmailOrUsername = username, // Try username first
                                Password = password
                            };

                            var loginResponse = await loginApiService.LoginAsync(loginDto);
                            
                            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
                            {
                                // Try with email instead
                                loginDto.EmailOrUsername = email;
                                loginResponse = await loginApiService.LoginAsync(loginDto);
                            }

                            if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                            {
                                System.Diagnostics.Debug.WriteLine("Account verified - login successful immediately after registration.");
                                // Registration successful
                                MessageBox.Show($"Account created and verified successfully!\n\nUsername: {username}\nEmail: {email}\n\nYou can now login with your credentials.",
                                    "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("WARNING: Account created but login verification failed!");
                                MessageBox.Show($"Account may have been created, but login verification failed.\n\nUsername: {username}\nEmail: {email}\n\nPlease try logging in. If it fails, the account may not have been created properly.",
                                    "Registration Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception verifyEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Login verification error: {verifyEx.Message}");
                        // Still show success but with warning
                        MessageBox.Show($"Account may have been created.\n\nUsername: {username}\nEmail: {email}\n\nVerification failed. Please try logging in manually.",
                            "Registration (Unverified)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Clear form fields
                    usernameTxtBox.Clear();
                    emailTxtBox.Clear();
                    passwordTxtBox.Clear();
                    confirmPwdTxtBox.Clear();

                    // Close create account panel and show login panel
                    animationManager.CloseCreateAccount();
                    
                    // Focus on login form
                    loginUsernameTxtBox.Focus();
                }
            }
            catch (Exception ex)
            {
                // Error message should already be formatted by ApiService
                string errorMessage = ex.Message;
                
                MessageBox.Show($"Registration failed!\n\n{errorMessage}",
                    "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Log for debugging
                System.Diagnostics.Debug.WriteLine($"Registration exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                // Re-enable button
                submitCreateBtn.Enabled = true;
            }
        }

    }
}