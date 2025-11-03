using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Regalia_Front_End.Owner_Dashboard;

namespace Regalia_Front_End
{
    public class UpdatePropertyFormManager
    {
        private Form parentForm;
        private PropertiesUpdateControl updateControl;
        private PropertyCardManager cardManager;
        private UpdateImageManager imageManager;
        private PropertyStatusCardManager statusCardManager;
        private System.Windows.Forms.Timer animationTimer;
        private bool isAnimating = false;
        private bool isClosing = false;
        private int animationStep = 0;
        private int totalSteps = 30;
        private Point startPosition;
        private Point endPosition;
        private Guna2ShadowPanel currentForm;
        private PropertyCard currentPropertyCard;
        private PropertyData originalPropertyData;
        private bool isLoadingData = false;
        
        public bool IsLoadingData => isLoadingData;

        public UpdatePropertyFormManager(Form parent, PropertiesUpdateControl updateCtrl, PropertyCardManager cardMgr, PropertyStatusCardManager statusCardMgr = null)
        {
            parentForm = parent;
            updateControl = updateCtrl;
            cardManager = cardMgr;
            statusCardManager = statusCardMgr;
            imageManager = new UpdateImageManager(updateCtrl);
        }

        #region Public Methods

        public void ShowUpdatePropertyForm(PropertyCard card)
        {
            if (card == null || card.PropertyData == null) return;

            currentPropertyCard = card;
            originalPropertyData = card.PropertyData;

            // Load property data into the update form
            LoadPropertyDataIntoForm(originalPropertyData);

            // Ensure update control is visible
            if (updateControl.Parent != null)
            {
                updateControl.Visible = true;
            }

            // Show updPropertiesFirst first with animation
            ShowFormWithAnimation(updateControl.updPropertiesFirst);
        }

        public void ShowUpdateProperties2()
        {
            updateControl.updPropertiesFirst.Visible = false;
            updateControl.updProperties1.Visible = false;
            updateControl.updProperties3.Visible = false;
            updateControl.updProperties4.Visible = false;
            ShowFormInstant(updateControl.updProperties2);
        }

        public void ShowUpdateProperties3()
        {
            updateControl.updPropertiesFirst.Visible = false;
            updateControl.updProperties1.Visible = false;
            updateControl.updProperties2.Visible = false;
            updateControl.updProperties4.Visible = false;
            ShowFormInstant(updateControl.updProperties3);
        }

        public void ShowUpdateProperties4()
        {
            updateControl.updPropertiesFirst.Visible = false;
            updateControl.updProperties1.Visible = false;
            updateControl.updProperties2.Visible = false;
            updateControl.updProperties3.Visible = false;
            ShowFormInstant(updateControl.updProperties4);
        }

        public void ShowUpdatePropertiesFirst()
        {
            updateControl.updProperties1.Visible = false;
            updateControl.updProperties2.Visible = false;
            updateControl.updProperties3.Visible = false;
            updateControl.updProperties4.Visible = false;
            ShowFormInstant(updateControl.updPropertiesFirst);
        }

        public void ShowUpdateProperties1()
        {
            updateControl.updPropertiesFirst.Visible = false;
            updateControl.updProperties2.Visible = false;
            updateControl.updProperties3.Visible = false;
            updateControl.updProperties4.Visible = false;
            ShowFormInstant(updateControl.updProperties1);
        }

        public void CloseForm(Guna2ShadowPanel form)
        {
            HideFormWithAnimation(form);
        }

        public void UpdateProperty()
        {
            try
            {
                if (currentPropertyCard == null)
                {
                    MessageBox.Show("No property selected for update.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Collect form data
                PropertyData updatedData = CollectFormData();

                // Validate required fields
                if (string.IsNullOrEmpty(updatedData.Title))
                {
                    MessageBox.Show("Please enter a property title.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(updatedData.Price))
                {
                    MessageBox.Show("Please enter a property price.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update the property card with new data
                currentPropertyCard.UpdatePropertyData(updatedData);
                
                // Update status from availabilityCmb
                if (updateControl.availabilityCmb.SelectedIndex >= 0)
                {
                    string newStatus = updateControl.availabilityCmb.SelectedItem?.ToString() ?? 
                                      updateControl.availabilityCmb.Items[updateControl.availabilityCmb.SelectedIndex]?.ToString() ?? 
                                      "Available";
                    
                    // Update PropertyCard status
                    currentPropertyCard.Status = newStatus;
                    
                    // Update PropertyStatusCard status in dashboard
                    if (statusCardManager != null && currentPropertyCard.PropertyData != null)
                    {
                        statusCardManager.UpdatePropertyStatus(currentPropertyCard.PropertyData.Title, newStatus);
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Status updated to: {newStatus}");
                }
                
                // Force card to refresh its display
                currentPropertyCard.Invalidate();
                currentPropertyCard.Refresh();

                MessageBox.Show("Property updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form (close updPropertiesFirst if still open, otherwise updProperties4)
                if (updateControl.updPropertiesFirst.Visible)
                {
                    HideFormWithAnimation(updateControl.updPropertiesFirst);
                }
                else
                {
                    HideFormWithAnimation(updateControl.updProperties4);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating property: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteProperty()
        {
            try
            {
                if (currentPropertyCard == null)
                {
                    MessageBox.Show("No property selected for deletion.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{currentPropertyCard.PropertyData?.Title ?? "this property"}'?\n\nThis action cannot be undone.",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Get a reference to the card before removing it
                PropertyCard cardToDelete = currentPropertyCard;
                string unitName = cardToDelete.PropertyData?.Title ?? "";

                // Remove the property card from the manager
                cardManager.RemovePropertyCard(cardToDelete);

                // Remove the property status card from dashboard
                if (statusCardManager != null && !string.IsNullOrEmpty(unitName))
                {
                    statusCardManager.RemovePropertyStatusCardByUnitName(unitName);
                }

                // Clear current property card reference
                currentPropertyCard = null;
                originalPropertyData = null;

                MessageBox.Show("Property deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the update form (close updPropertiesFirst if it's still open)
                if (updateControl.updPropertiesFirst.Visible)
                {
                    HideFormWithAnimation(updateControl.updPropertiesFirst);
                }
                else if (updateControl.updProperties1.Visible)
                {
                    HideFormWithAnimation(updateControl.updProperties1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting property: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdatePropertyStatus(string status)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"UpdatePropertyStatus called with: {status}");
                
                if (currentPropertyCard != null)
                {
                    System.Diagnostics.Debug.WriteLine($"CurrentPropertyCard found: {currentPropertyCard.PropertyData?.Title}");
                    currentPropertyCard.Status = status;
                    System.Diagnostics.Debug.WriteLine($"PropertyCard.Status set to: {currentPropertyCard.Status}");
                    
                    // Also update the status card in dashboard if it exists
                    if (statusCardManager != null && currentPropertyCard.PropertyData != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Updating PropertyStatusCard for: {currentPropertyCard.PropertyData.Title}");
                        // Find and update the status card
                        statusCardManager.UpdatePropertyStatus(currentPropertyCard.PropertyData.Title, status);
                        System.Diagnostics.Debug.WriteLine($"PropertyStatusCard updated");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"statusCardManager is null or PropertyData is null");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"currentPropertyCard is null!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdatePropertyStatus: {ex.Message}");
            }
        }

        #endregion

        #region Private Data Methods

        private void LoadPropertyDataIntoForm(PropertyData data)
        {
            // Load data into updPropertiesFirst (status)
            isLoadingData = true; // Prevent event handler from firing during load
            if (currentPropertyCard != null)
            {
                string status = currentPropertyCard.Status ?? "Available";
                int statusIndex = updateControl.availabilityCmb.Items.IndexOf(status);
                if (statusIndex >= 0)
                {
                    updateControl.availabilityCmb.SelectedIndex = statusIndex;
                }
                else
                {
                    // If status not found, default to Available
                    updateControl.availabilityCmb.SelectedIndex = 0;
                }
            }
            else
            {
                updateControl.availabilityCmb.SelectedIndex = 0; // Default to Available
            }
            isLoadingData = false;
            
            // Load data into updProperties1 (basic info)
            updateControl.updNameTxt.Text = data.Title ?? ""; // Unit Name/No.
            updateControl.updLocationTxt.Text = data.Location ?? ""; // Location
            updateControl.updPriceTxt.Text = data.Price ?? ""; // Price

            // Set combo box selection for Room Type
            if (!string.IsNullOrEmpty(data.Location))
            {
                int index = updateControl.updTypeCmb.Items.IndexOf(data.Location);
                if (index >= 0)
                {
                    updateControl.updTypeCmb.SelectedIndex = index;
                }
                else
                {
                    updateControl.updTypeCmb.Text = data.Location;
                }
            }

            // Load data into updProperties2 (additional details)
            updateControl.updDescTxt.Text = data.Bedrooms ?? ""; // Description field (Bedrooms data)
            updateControl.updRuleTxt.Text = data.Bathrooms ?? ""; // Rules field (Bathrooms data)

            // Load images into ImageManager
            if (!string.IsNullOrEmpty(data.Image1Path))
                imageManager.LoadImage(1, data.Image1Path);
            if (!string.IsNullOrEmpty(data.Image2Path))
                imageManager.LoadImage(2, data.Image2Path);
            if (!string.IsNullOrEmpty(data.Image3Path))
                imageManager.LoadImage(3, data.Image3Path);
            if (!string.IsNullOrEmpty(data.Image4Path))
                imageManager.LoadImage(4, data.Image4Path);
        }

        private PropertyData CollectFormData()
        {
            PropertyData data = new PropertyData();

            // Collect data from updProperties1 (basic info)
            data.Title = updateControl.updNameTxt.Text?.Trim(); // Unit Name/No.
            data.Location = updateControl.updLocationTxt.Text?.Trim(); // Location
            data.Price = updateControl.updPriceTxt.Text?.Trim(); // Price
            
            // Room Type from combo box (if needed)
            string roomType = updateControl.updTypeCmb.SelectedItem?.ToString() ?? updateControl.updTypeCmb.Text?.Trim();

            // Collect data from updProperties2 (additional details)
            data.Bedrooms = updateControl.updDescTxt.Text?.Trim(); // Description field (Bedrooms data)
            data.Bathrooms = updateControl.updRuleTxt.Text?.Trim(); // Rules field (Bathrooms data)

            // Collect image paths from ImageManager
            data.Image1Path = imageManager.GetImagePath(1);
            data.Image2Path = imageManager.GetImagePath(2);
            data.Image3Path = imageManager.GetImagePath(3);
            data.Image4Path = imageManager.GetImagePath(4);

            // Set default area if not provided
            data.Area = originalPropertyData?.Area ?? "N/A";

            return data;
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

        public void HideFormWithAnimation(Guna2ShadowPanel form)
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

            imageManager?.Dispose();
        }

        #endregion
    }
}


