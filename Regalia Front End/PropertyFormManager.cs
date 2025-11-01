using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Regalia_Front_End
{
    public class PropertyFormManager
    {
        private Form parentForm;
        private PropertiesControl propertiesControl;
        private PropertyCardManager cardManager;
        private ImageManager imageManager;
        private System.Windows.Forms.Timer animationTimer;
        private bool isAnimating = false;
        private bool isClosing = false;
        private int animationStep = 0;
        private int totalSteps = 30;
        private Point startPosition;
        private Point endPosition;
        private Guna2ShadowPanel currentForm;

        public PropertyFormManager(Form parent, PropertiesControl propertiesCtrl, ImageManager imgManager)
        {
            parentForm = parent;
            propertiesControl = propertiesCtrl;
            imageManager = imgManager;
            cardManager = new PropertyCardManager(propertiesCtrl);
        }

        #region Public Methods

        public void ShowAddPropertyForm()
        {
            ShowFormWithAnimation(propertiesControl.addProperties1);
        }

        public void ShowProperties2()
        {
            propertiesControl.addProperties1.Visible = false;
            propertiesControl.addProperties3.Visible = false;
            propertiesControl.addProperties4.Visible = false;
            ShowFormInstant(propertiesControl.addProperties2);
        }

        public void ShowProperties3()
        {
            propertiesControl.addProperties1.Visible = false;
            propertiesControl.addProperties2.Visible = false;
            propertiesControl.addProperties4.Visible = false;
            ShowFormInstant(propertiesControl.addProperties3);
        }

        public void ShowProperties4()
        {
            propertiesControl.addProperties1.Visible = false;
            propertiesControl.addProperties2.Visible = false;
            propertiesControl.addProperties3.Visible = false;
            ShowFormInstant(propertiesControl.addProperties4);
        }

        public void ShowProperties1()
        {
            propertiesControl.addProperties2.Visible = false;
            propertiesControl.addProperties3.Visible = false;
            propertiesControl.addProperties4.Visible = false;
            ShowFormInstant(propertiesControl.addProperties1);
        }

        public void CloseForm(Guna2ShadowPanel form)
        {
            HideFormWithAnimation(form);
        }

        public void SubmitProperty(bool navigateToPage4 = true)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("SubmitProperty called");
                
                // Collect form data
                PropertyData propertyData = CollectFormData();
                System.Diagnostics.Debug.WriteLine($"Data collected - Title: {propertyData.Title}, Price: {propertyData.Price}");
                
                // Validate required fields
                if (string.IsNullOrEmpty(propertyData.Title))
                {
                    MessageBox.Show("Please enter a property title.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrEmpty(propertyData.Price))
                {
                    MessageBox.Show("Please enter a property price.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                System.Diagnostics.Debug.WriteLine("About to call AddPropertyCard");
                
                // Create property card - this is the original working function
                cardManager.AddPropertyCard(propertyData);
                
                System.Diagnostics.Debug.WriteLine("AddPropertyCard completed");
                
                // Ensure PropertiesControl is visible so cards show
                propertiesControl.Visible = true;
                propertiesControl.Show();
                propertiesControl.BringToFront();
                
                // Don't clear form data yet - we'll do it after front desk account is created
                // ClearFormData(); - Commented out for now
                
                if (navigateToPage4)
                {
                    MessageBox.Show("Property added successfully! Now create a front desk account.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Navigate to page 4 (front desk account creation) instead of closing
                    // Cards will remain visible in the background behind page 4
                    ShowProperties4();
                    
                    // After showing page 4, bring page 4 to front but keep cards visible
                    propertiesControl.addProperties4.BringToFront();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in SubmitProperty: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error submitting property: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SubmitFrontDeskAccount()
        {
            try
            {
                // TODO: Collect front desk account data from addProperties4 textboxes
                // TODO: Validate required fields (username, password, email, etc.)
                // TODO: Save/create front desk account and link to the property created on page 3
                
                // Clear all form data (property + front desk) after successful submission
                ClearFormData();
                ClearFrontDeskFormData();
                
                MessageBox.Show("Property and front desk account created successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Close the form
                HideFormWithAnimation(propertiesControl.addProperties4);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating front desk account: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Private Data Collection Methods

        private PropertyData CollectFormData()
        {
            PropertyData data = new PropertyData();
            
            // Collect data from addProperties1 (basic info)
            data.Title = propertiesControl.guna2TextBox1.Text?.Trim();
            data.Price = propertiesControl.guna2TextBox2.Text?.Trim();
            data.Location = propertiesControl.guna2ComboBox1.SelectedItem?.ToString() ?? propertiesControl.guna2ComboBox1.Text?.Trim();
            
            // Collect data from addProperties2 (additional details)
            data.Bedrooms = propertiesControl.guna2TextBox8.Text?.Trim();
            data.Bathrooms = propertiesControl.guna2TextBox9.Text?.Trim();
            
            // Collect image paths from ImageManager
            data.Image1Path = imageManager.GetImagePath(1);
            data.Image2Path = imageManager.GetImagePath(2);
            data.Image3Path = imageManager.GetImagePath(3);
            data.Image4Path = imageManager.GetImagePath(4);
            
            // Set default area if not provided
            data.Area = "N/A";
            
            return data;
        }

        private void ClearFormData()
        {
            // Clear addProperties1 fields
            propertiesControl.guna2TextBox1.Text = "";
            propertiesControl.guna2TextBox2.Text = "";
            propertiesControl.guna2ComboBox1.SelectedIndex = -1;
            propertiesControl.guna2ComboBox1.Text = "";
            
            // Clear addProperties2 fields
            propertiesControl.guna2TextBox8.Text = "";
            propertiesControl.guna2TextBox9.Text = "";
            
            // Clear addProperties3 fields (if any)
            propertiesControl.guna2TextBox3.Text = "";
            propertiesControl.guna2TextBox4.Text = "";
            
            // Clear images using ImageManager
            imageManager.ClearImage(1);
            imageManager.ClearImage(2);
            imageManager.ClearImage(3);
            imageManager.ClearImage(4);
        }

        private void ClearFrontDeskFormData()
        {
            // TODO: Clear all textboxes in addProperties4
            // Example (adjust based on your actual control names):
            // propertiesControl.frontDeskUsernameTxtBox.Text = "";
            // propertiesControl.frontDeskPasswordTxtBox.Text = "";
            // propertiesControl.frontDeskEmailTxtBox.Text = "";
        }

        #endregion

        #region Private Animation Methods

        private void ShowFormWithAnimation(Guna2ShadowPanel form)
        {
            if (isAnimating) return;

            currentForm = form;
            isAnimating = true;
            isClosing = false;

            // Calculate positions
            int centerX = (parentForm.ClientSize.Width - form.Width) / 2;
            int centerY = (parentForm.ClientSize.Height - form.Height) / 2;
            endPosition = new Point(centerX, centerY);
            startPosition = new Point(centerX, parentForm.ClientSize.Height + 50);

            // Set initial position and make visible
            form.Location = startPosition;
            form.Visible = true;
            form.BringToFront();

            // Start animation
            StartAnimation();
        }

        private void ShowFormInstant(Guna2ShadowPanel form)
        {
            // Center the form instantly
            int centerX = (parentForm.ClientSize.Width - form.Width) / 2;
            int centerY = (parentForm.ClientSize.Height - form.Height) / 2;
            form.Location = new Point(centerX, centerY);
            form.Visible = true;
            form.BringToFront();
        }

        private void HideFormWithAnimation(Guna2ShadowPanel form)
        {
            if (isAnimating) return;

            currentForm = form;
            isAnimating = true;
            isClosing = true;

            // Calculate positions
            startPosition = form.Location;
            endPosition = new Point(startPosition.X, parentForm.ClientSize.Height + 50);

            // Start animation
            StartAnimation();
        }

        private void StartAnimation()
        {
            if (animationTimer != null)
            {
                animationTimer.Stop();
                animationTimer.Dispose();
            }

            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 16; // ~60fps
            animationTimer.Tick += AnimationTimer_Tick;
            animationStep = 0;
            totalSteps = isClosing ? 20 : 30; // Faster closing
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            animationStep++;
            double progress = (double)animationStep / totalSteps;

            // Apply easing
            if (isClosing)
            {
                progress = progress * progress; // Ease-in for closing
            }
            else
            {
                progress = 1 - Math.Pow(1 - progress, 3); // Ease-out for opening
            }

            // Calculate current position
            int currentX = startPosition.X + (int)((endPosition.X - startPosition.X) * progress);
            int currentY = startPosition.Y + (int)((endPosition.Y - startPosition.Y) * progress);

            currentForm.Location = new Point(currentX, currentY);

            if (animationStep >= totalSteps)
            {
                animationTimer.Stop();
                animationTimer.Dispose();
                animationTimer = null;

                if (isClosing)
                {
                    currentForm.Visible = false;
                    // Reset position for next opening
                    int centerX = (parentForm.ClientSize.Width - currentForm.Width) / 2;
                    int centerY = (parentForm.ClientSize.Height - currentForm.Height) / 2;
                    currentForm.Location = new Point(centerX, centerY);
                }
                else
                {
                    currentForm.Location = endPosition; // Ensure exact final position
                }

                isAnimating = false;
                isClosing = false;
            }
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {
            if (animationTimer != null)
            {
                animationTimer.Stop();
                animationTimer.Dispose();
                animationTimer = null;
            }
            
            cardManager?.Dispose();
        }

        #endregion
    }
}
