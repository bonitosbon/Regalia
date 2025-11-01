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
        private Guna2ShadowPanel properties3Pnl;

        public AnimationManager(Form form, Guna2ShadowPanel createPanel, Guna2ShadowPanel loginPanel)
        {
            parentForm = form;
            createAccountPnl = createPanel;
            loginAccountPnl = loginPanel;
        }

        public void SetProperties3Panel(Guna2ShadowPanel panel)
        {
            properties3Pnl = panel;
        }

        #region Create Account Panel Animation Methods

        public void StartCreateAccountAnimation()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("StartCreateAccountAnimation called");
                
                // Stop any existing animation first
                if (createAccountAnimationTimer != null)
                {
                    createAccountAnimationTimer.Stop();
                }
                
                // Reset animation state
                isCreateAccountAnimating = false;
                isCreateAccountClosing = false;

                // Set up for opening animation
                isCreateAccountClosing = false;

                // Calculate end position (centered on form)
                // Use current position as end position (already centered)
                createAccountEndPosition = createAccountPnl.Location;

                // Set start position (below the visible area, same X position)
                createAccountStartPosition = new Point(createAccountEndPosition.X, parentForm.ClientSize.Height + 50);

                // Reset animation
                createAccountAnimationStep = 0;
                isCreateAccountAnimating = true;

                // Set initial position
                createAccountPnl.Location = createAccountStartPosition;
                createAccountPnl.Visible = true;
                createAccountPnl.Show();
                createAccountPnl.BringToFront();
                createAccountPnl.Refresh();
                
                System.Diagnostics.Debug.WriteLine($"Animation - Start: {createAccountStartPosition}, End: {createAccountEndPosition}");
                System.Diagnostics.Debug.WriteLine($"Form location set to: {createAccountPnl.Location}");

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

                // Start the animation timer
                if (createAccountAnimationTimer == null)
                {
                    createAccountAnimationTimer = new System.Windows.Forms.Timer();
                    createAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
                    createAccountAnimationTimer.Tick += CreateAccountAnimationTimer_Tick;
                }
                else
                {
                    createAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
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
                
                System.Diagnostics.Debug.WriteLine($"Animation tick - Step: {createAccountAnimationStep}, Closing: {isCreateAccountClosing}");

                // Use different step counts for opening vs closing
                int totalSteps = isCreateAccountClosing ? 3 : createAccountTotalAnimationSteps; // Almost instant closing
                double progress = (double)createAccountAnimationStep / totalSteps;
                
                // Apply different easing for opening vs closing
                double easedProgress;
                if (isCreateAccountClosing)
                {
                    // For closing, use ease-in for smooth movement (same as property forms)
                    easedProgress = progress * progress;
                }
                else
                {
                    // For opening, use ease-out for smooth landing
                    easedProgress = 1 - Math.Pow(1 - progress, 3);
                }

                // Calculate current position
                int currentX = createAccountStartPosition.X + (int)((createAccountEndPosition.X - createAccountStartPosition.X) * easedProgress);
                int currentY = createAccountStartPosition.Y + (int)((createAccountEndPosition.Y - createAccountStartPosition.Y) * easedProgress);

                // Update position - handle both createAccountPnl and properties3Pnl
                if (properties3Pnl != null && createAccountPnl != properties3Pnl)
                {
                    // We're animating properties3Pnl, so update it directly
                    properties3Pnl.Location = new Point(currentX, currentY);
                }
                else
                {
                    // We're animating createAccountPnl
                    createAccountPnl.Location = new Point(currentX, currentY);
                }

                // Increment animation step
                createAccountAnimationStep++;

                // Check if animation is complete
                if (createAccountAnimationStep >= totalSteps)
                {
                    createAccountAnimationTimer.Stop();
                    isCreateAccountAnimating = false;
                    
                    // Ensure final position is exact
                    if (properties3Pnl != null && createAccountPnl != properties3Pnl)
                    {
                        properties3Pnl.Location = createAccountEndPosition;
                    }
                    else if (createAccountPnl != null)
                    {
                        createAccountPnl.Location = createAccountEndPosition;
                    }
                    
                    // Handle completion based on whether we're opening or closing
                    if (isCreateAccountClosing)
                    {
                        // Hide the panel when closing animation completes
                        if (properties3Pnl != null && createAccountPnl != properties3Pnl)
                        {
                            properties3Pnl.Visible = false;
                            // Reset position to original for next opening
                            int centerX = (parentForm.ClientSize.Width - properties3Pnl.Width) / 2;
                            int centerY = (parentForm.ClientSize.Height - properties3Pnl.Height) / 2;
                            properties3Pnl.Location = new Point(centerX, centerY);
                            // Restore shadow
                            properties3Pnl.ShadowDepth = 200;
                        }
                        else if (createAccountPnl != null)
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
                // Stop any existing animation first
                if (loginAccountAnimationTimer != null)
                {
                    loginAccountAnimationTimer.Stop();
                }
                
                // Reset animation state
                isLoginAccountAnimating = false;
                isLoginAccountClosing = false;

                // Set up for opening animation
                isLoginAccountClosing = false;

                // Calculate end position (centered on form)
                int centerX = (parentForm.ClientSize.Width - loginAccountPnl.Width) / 2;
                int centerY = (parentForm.ClientSize.Height - loginAccountPnl.Height) / 2;
                loginAccountEndPosition = new Point(centerX, centerY);

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

                // Start the animation timer
                if (loginAccountAnimationTimer == null)
                {
                    loginAccountAnimationTimer = new System.Windows.Forms.Timer();
                    loginAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
                    loginAccountAnimationTimer.Tick += LoginAccountAnimationTimer_Tick;
                }
                else
                {
                    loginAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
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
                int totalSteps = isLoginAccountClosing ? 3 : loginAccountTotalAnimationSteps; // Almost instant closing
                double progress = (double)loginAccountAnimationStep / totalSteps;
                
                // Apply different easing for opening vs closing
                double easedProgress;
                if (isLoginAccountClosing)
                {
                    // For closing, use ease-in for smooth movement (same as property forms)
                    easedProgress = progress * progress;
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

        #region Properties3 Panel Animation Methods

        public void StartProperties3Animation()
        {
            try
            {
                if (properties3Pnl == null) return;

                // Stop any existing animation first
                if (createAccountAnimationTimer != null)
                {
                    createAccountAnimationTimer.Stop();
                }
                
                // Reset animation state
                isCreateAccountAnimating = false;
                isCreateAccountClosing = false;

                // Set up for opening animation
                isCreateAccountClosing = false;

                // Calculate end position (centered on form)
                int centerX = (parentForm.ClientSize.Width - properties3Pnl.Width) / 2;
                int centerY = (parentForm.ClientSize.Height - properties3Pnl.Height) / 2;
                createAccountEndPosition = new Point(centerX, centerY);

                // Set start position (below the visible area, same X position)
                createAccountStartPosition = new Point(createAccountEndPosition.X, parentForm.ClientSize.Height);

                // Reset animation
                createAccountAnimationStep = 0;
                isCreateAccountAnimating = true;

                // Set initial position
                properties3Pnl.Location = createAccountStartPosition;
                properties3Pnl.Visible = true;
                properties3Pnl.BringToFront();

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
                MessageBox.Show($"Error starting properties3 animation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CloseProperties3()
        {
            try
            {
                if (properties3Pnl == null) return;

                // Stop any existing animation first
                if (createAccountAnimationTimer != null)
                {
                    createAccountAnimationTimer.Stop();
                }

                // Set up for closing animation
                isCreateAccountClosing = true;

                // Calculate start position (current position)
                createAccountStartPosition = properties3Pnl.Location;

                // Calculate end position (below the visible area, same X position)
                createAccountEndPosition = new Point(createAccountStartPosition.X, parentForm.ClientSize.Height + 50);

                // Reset animation
                createAccountAnimationStep = 0;
                isCreateAccountAnimating = true;

                // Start the animation timer
                if (createAccountAnimationTimer == null)
                {
                    createAccountAnimationTimer = new System.Windows.Forms.Timer();
                    createAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
                    createAccountAnimationTimer.Tick += CreateAccountAnimationTimer_Tick;
                }
                else
                {
                    createAccountAnimationTimer.Interval = 16; // ~60fps for smooth animation
                }
                createAccountAnimationTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing properties3 panel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
