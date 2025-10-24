using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Regalia_Front_End
{
    public class AnimationManager
    {
        private Form parentForm;
        
        // Create Account Panel Animation Variables
        private System.Windows.Forms.Timer createAccountAnimationTimer;
        private bool isCreateAccountAnimating = false;
        private bool isCreateAccountClosing = false;
        private Point createAccountStartPosition;
        private Point createAccountEndPosition;
        private int createAccountAnimationStep = 0;
        private int createAccountTotalAnimationSteps = 40;
        
        // Login Account Panel Animation Variables
        private System.Windows.Forms.Timer loginAccountAnimationTimer;
        private bool isLoginAccountAnimating = false;
        private bool isLoginAccountClosing = false;
        private Point loginAccountStartPosition;
        private Point loginAccountEndPosition;
        private int loginAccountAnimationStep = 0;
        private int loginAccountTotalAnimationSteps = 40;
        
        // Panel References
        private Guna2ShadowPanel createAccountPnl;
        private Guna2ShadowPanel loginAccountPnl;

        public AnimationManager(Form form, Guna2ShadowPanel createPanel, Guna2ShadowPanel loginPanel)
        {
            parentForm = form;
            createAccountPnl = createPanel;
            loginAccountPnl = loginPanel;
        }

        #region Create Account Panel Animation Methods

        public void StartCreateAccountAnimation()
        {
            try
            {
                if (isCreateAccountAnimating) return;

                // Stop any existing animation
                if (createAccountAnimationTimer != null)
                {
                    createAccountAnimationTimer.Stop();
                }

                // Set up for opening animation
                isCreateAccountClosing = false;

                // Calculate end position (original position)
                createAccountEndPosition = new Point(298, 130);

                // Set start position (below the visible area, same X position)
                createAccountStartPosition = new Point(createAccountEndPosition.X, parentForm.ClientSize.Height);

                // Reset animation
                createAccountAnimationStep = 0;
                isCreateAccountAnimating = true;

                // Set initial position
                createAccountPnl.Location = createAccountStartPosition;
                createAccountPnl.Visible = true;
                createAccountPnl.BringToFront();

                // Start the animation timer
                if (createAccountAnimationTimer == null)
                {
                    createAccountAnimationTimer = new System.Windows.Forms.Timer();
                    createAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
                    createAccountAnimationTimer.Tick += CreateAccountAnimationTimer_Tick;
                }
                createAccountAnimationTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting create account animation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CloseCreateAccount()
        {
            try
            {
                if (isCreateAccountAnimating) return;

                // Stop any existing animation
                if (createAccountAnimationTimer != null)
                {
                    createAccountAnimationTimer.Stop();
                }

                // Set up for closing animation
                isCreateAccountClosing = true;

                // Calculate start position (current position)
                createAccountStartPosition = createAccountPnl.Location;

                // Calculate end position (below the visible area, same X position)
                createAccountEndPosition = new Point(createAccountStartPosition.X, parentForm.ClientSize.Height);

                // Reset animation
                createAccountAnimationStep = 0;
                isCreateAccountAnimating = true;

                // Start the animation timer with faster settings
                if (createAccountAnimationTimer == null)
                {
                    createAccountAnimationTimer = new System.Windows.Forms.Timer();
                    createAccountAnimationTimer.Interval = 1; // Instant for closing
                    createAccountAnimationTimer.Tick += CreateAccountAnimationTimer_Tick;
                }
                else
                {
                    createAccountAnimationTimer.Interval = 1; // Instant for closing
                }
                createAccountAnimationTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing panel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateAccountAnimationTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!isCreateAccountAnimating) return;

                // Use different step counts for opening vs closing
                int totalSteps = isCreateAccountClosing ? 2 : createAccountTotalAnimationSteps; // Instant closing
                double progress = (double)createAccountAnimationStep / totalSteps;
                
                // Apply different easing for opening vs closing
                double easedProgress;
                if (isCreateAccountClosing)
                {
                    // For closing, use linear for instant movement (no trail)
                    easedProgress = progress;
                }
                else
                {
                    // For opening, use ease-out for smooth landing
                    easedProgress = 1 - Math.Pow(1 - progress, 3);
                }

                // Calculate current position
                int currentX = createAccountStartPosition.X + (int)((createAccountEndPosition.X - createAccountStartPosition.X) * easedProgress);
                int currentY = createAccountStartPosition.Y + (int)((createAccountEndPosition.Y - createAccountStartPosition.Y) * easedProgress);

                // Update position
                createAccountPnl.Location = new Point(currentX, currentY);

                // Increment animation step
                createAccountAnimationStep++;

                // Check if animation is complete
                if (createAccountAnimationStep >= totalSteps)
                {
                    createAccountAnimationTimer.Stop();
                    isCreateAccountAnimating = false;
                    
                    if (createAccountPnl != null)
                    {
                        createAccountPnl.Location = createAccountEndPosition; // Ensure final position is exact
                    }
                    
                    // Handle completion based on whether we're opening or closing
                    if (isCreateAccountClosing)
                    {
                        // Hide the panel when closing animation completes
                        if (createAccountPnl != null)
                        {
                            createAccountPnl.Visible = false;
                            // Reset position to original for next opening
                            createAccountPnl.Location = new Point(298, 130);
                            // Restore shadow
                            createAccountPnl.ShadowDepth = 200;
                        }
                        isCreateAccountClosing = false; // Reset closing flag
                    }
                }
            }
            catch (Exception ex)
            {
                if (createAccountAnimationTimer != null)
                {
                    createAccountAnimationTimer.Stop();
                }
                isCreateAccountAnimating = false;
                MessageBox.Show($"Error during animation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Login Account Panel Animation Methods

        public void StartLoginAccountAnimation()
        {
            try
            {
                if (isLoginAccountAnimating) return;

                // Stop any existing animation
                if (loginAccountAnimationTimer != null)
                {
                    loginAccountAnimationTimer.Stop();
                }

                // Set up for opening animation
                isLoginAccountClosing = false;

                // Calculate end position (original position)
                loginAccountEndPosition = new Point(298, 130);

                // Set start position (below the visible area, same X position)
                loginAccountStartPosition = new Point(loginAccountEndPosition.X, parentForm.ClientSize.Height);

                // Reset animation
                loginAccountAnimationStep = 0;
                isLoginAccountAnimating = true;

                // Set initial position
                loginAccountPnl.Location = loginAccountStartPosition;
                loginAccountPnl.Visible = true;
                loginAccountPnl.BringToFront();

                // Start the animation timer
                if (loginAccountAnimationTimer == null)
                {
                    loginAccountAnimationTimer = new System.Windows.Forms.Timer();
                    loginAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
                    loginAccountAnimationTimer.Tick += LoginAccountAnimationTimer_Tick;
                }
                loginAccountAnimationTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting login account animation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CloseLoginAccount()
        {
            try
            {
                if (isLoginAccountAnimating) return;

                // Stop any existing animation
                if (loginAccountAnimationTimer != null)
                {
                    loginAccountAnimationTimer.Stop();
                }

                // Set up for closing animation
                isLoginAccountClosing = true;

                // Calculate start position (current position)
                loginAccountStartPosition = loginAccountPnl.Location;

                // Calculate end position (below the visible area, same X position)
                loginAccountEndPosition = new Point(loginAccountStartPosition.X, parentForm.ClientSize.Height);

                // Reset animation
                loginAccountAnimationStep = 0;
                isLoginAccountAnimating = true;

                // Start the animation timer with faster settings
                if (loginAccountAnimationTimer == null)
                {
                    loginAccountAnimationTimer = new System.Windows.Forms.Timer();
                    loginAccountAnimationTimer.Interval = 1; // Instant for closing
                    loginAccountAnimationTimer.Tick += LoginAccountAnimationTimer_Tick;
                }
                else
                {
                    loginAccountAnimationTimer.Interval = 1; // Instant for closing
                }
                loginAccountAnimationTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing login panel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginAccountAnimationTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!isLoginAccountAnimating) return;

                // Use different step counts for opening vs closing
                int totalSteps = isLoginAccountClosing ? 2 : loginAccountTotalAnimationSteps; // Instant closing
                double progress = (double)loginAccountAnimationStep / totalSteps;
                
                // Apply different easing for opening vs closing
                double easedProgress;
                if (isLoginAccountClosing)
                {
                    // For closing, use linear for instant movement (no trail)
                    easedProgress = progress;
                }
                else
                {
                    // For opening, use ease-out for smooth landing
                    easedProgress = 1 - Math.Pow(1 - progress, 3);
                }

                // Calculate current position
                int currentX = loginAccountStartPosition.X + (int)((loginAccountEndPosition.X - loginAccountStartPosition.X) * easedProgress);
                int currentY = loginAccountStartPosition.Y + (int)((loginAccountEndPosition.Y - loginAccountStartPosition.Y) * easedProgress);

                // Update position
                loginAccountPnl.Location = new Point(currentX, currentY);

                // Increment animation step
                loginAccountAnimationStep++;

                // Check if animation is complete
                if (loginAccountAnimationStep >= totalSteps)
                {
                    loginAccountAnimationTimer.Stop();
                    isLoginAccountAnimating = false;
                    
                    if (loginAccountPnl != null)
                    {
                        loginAccountPnl.Location = loginAccountEndPosition; // Ensure final position is exact
                    }
                    
                    // Handle completion based on whether we're opening or closing
                    if (isLoginAccountClosing)
                    {
                        // Hide the panel when closing animation completes
                        if (loginAccountPnl != null)
                        {
                            loginAccountPnl.Visible = false;
                            // Reset position to original for next opening
                            loginAccountPnl.Location = new Point(298, 130);
                            // Restore shadow
                            loginAccountPnl.ShadowDepth = 200;
                        }
                        isLoginAccountClosing = false; // Reset closing flag
                    }
                }
            }
            catch (Exception ex)
            {
                if (loginAccountAnimationTimer != null)
                {
                    loginAccountAnimationTimer.Stop();
                }
                isLoginAccountAnimating = false;
                MessageBox.Show($"Error during login animation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Cleanup Methods

        public void Dispose()
        {
            // Clean up timers
            if (createAccountAnimationTimer != null)
            {
                createAccountAnimationTimer.Stop();
                createAccountAnimationTimer.Dispose();
            }
            
            if (loginAccountAnimationTimer != null)
            {
                loginAccountAnimationTimer.Stop();
                loginAccountAnimationTimer.Dispose();
            }
        }

        #endregion
    }
}
