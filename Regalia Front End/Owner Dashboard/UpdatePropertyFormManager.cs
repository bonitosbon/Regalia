using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Regalia_Front_End.Owner_Dashboard;
using Regalia_Front_End.Models;
using Regalia_Front_End.Services;
using Regalia_Front_End.Helpers;
using System.Threading.Tasks;

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
            if (card == null || card.PropertyData == null)
            {
                System.Diagnostics.Debug.WriteLine($"ShowUpdatePropertyForm: card is null or PropertyData is null");
                return;
            }

            currentPropertyCard = card;
            originalPropertyData = card.PropertyData;
            
            // Ensure CondoId is set in PropertyData
            if (originalPropertyData.CondoId == 0 && card.CondoId > 0)
            {
                originalPropertyData.CondoId = card.CondoId;
                System.Diagnostics.Debug.WriteLine($"ShowUpdatePropertyForm: Set CondoId from card: {card.CondoId}");
            }
            
            System.Diagnostics.Debug.WriteLine($"ShowUpdatePropertyForm: currentPropertyCard.CondoId = {currentPropertyCard.CondoId}, PropertyData.CondoId = {originalPropertyData.CondoId}");

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

        public async void UpdateProperty()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"UpdateProperty called: currentPropertyCard = {currentPropertyCard != null}, CondoId = {currentPropertyCard?.CondoId ?? 0}, originalPropertyData.CondoId = {originalPropertyData?.CondoId ?? 0}");
                
                // Check if we have a valid property card
                if (currentPropertyCard == null)
                {
                    MessageBox.Show("No property selected for update. Please click on a property card first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Get CondoId from card or PropertyData
                int condoId = currentPropertyCard.CondoId;
                if (condoId == 0 && originalPropertyData != null)
                {
                    condoId = originalPropertyData.CondoId;
                }
                
                if (condoId == 0)
                {
                    MessageBox.Show("Property ID is missing. Please refresh the properties list and try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Collect form data
                PropertyData updatedData = CollectFormData();

                // Validate required fields
                if (string.IsNullOrWhiteSpace(updatedData.Title))
                {
                    MessageBox.Show("Please enter a property title.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(updatedData.Location))
                {
                    MessageBox.Show("Please enter a property location.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(updatedData.Price))
                {
                    MessageBox.Show("Please enter a property price.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse price
                decimal pricePerNight;
                if (!decimal.TryParse(updatedData.Price, out pricePerNight))
                {
                    MessageBox.Show("Please enter a valid price.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get status from availabilityCmb
                string newStatus = "Available";
                if (updateControl.availabilityCmb.SelectedIndex >= 0)
                {
                    newStatus = updateControl.availabilityCmb.SelectedItem?.ToString() ?? 
                               updateControl.availabilityCmb.Items[updateControl.availabilityCmb.SelectedIndex]?.ToString() ?? 
                               "Available";
                }

                // Get Room Type
                string roomType = updateControl.updTypeCmb.SelectedItem?.ToString() ?? updateControl.updTypeCmb.Text?.Trim();
                
                // Collect images and convert to base64 for web display
                List<string> imageList = new List<string>();
                if (!string.IsNullOrEmpty(updatedData.Image1Path)) 
                {
                    string base64Image1 = ImageBase64Helper.ConvertImageToBase64(updatedData.Image1Path);
                    if (!string.IsNullOrEmpty(base64Image1)) imageList.Add(base64Image1);
                }
                if (!string.IsNullOrEmpty(updatedData.Image2Path)) 
                {
                    string base64Image2 = ImageBase64Helper.ConvertImageToBase64(updatedData.Image2Path);
                    if (!string.IsNullOrEmpty(base64Image2)) imageList.Add(base64Image2);
                }
                if (!string.IsNullOrEmpty(updatedData.Image3Path)) 
                {
                    string base64Image3 = ImageBase64Helper.ConvertImageToBase64(updatedData.Image3Path);
                    if (!string.IsNullOrEmpty(base64Image3)) imageList.Add(base64Image3);
                }
                if (!string.IsNullOrEmpty(updatedData.Image4Path)) 
                {
                    string base64Image4 = ImageBase64Helper.ConvertImageToBase64(updatedData.Image4Path);
                    if (!string.IsNullOrEmpty(base64Image4)) imageList.Add(base64Image4);
                }
                
                string primaryImageUrl = imageList.Count > 0 ? imageList[0] : string.Empty;
                
                // Build full description
                string fullDescription = updatedData.Bathrooms ?? "";
                if (imageList.Count > 1)
                {
                    string additionalImages = string.Join(", ", imageList.Skip(1));
                    if (!string.IsNullOrEmpty(fullDescription))
                        fullDescription += $" | Additional Images: {additionalImages}";
                    else
                        fullDescription = $"Additional Images: {additionalImages}";
                }

                // Create UpdateCondoDto
                UpdateCondoDto updateDto = new UpdateCondoDto
                {
                    Name = updatedData.Title,
                    Location = updatedData.Location,
                    Description = fullDescription,
                    Amenities = updatedData.Bedrooms ?? string.Empty,
                    MaxGuests = 4, // Default
                    PricePerNight = pricePerNight,
                    ImageUrl = primaryImageUrl,
                    Status = newStatus
                };

                // Call API to update condo
                using (var apiService = new ApiService())
                {
                    await apiService.UpdateCondoAsync(condoId, updateDto);
                    
                    // Update status separately if changed
                    if (newStatus != (originalPropertyData?.Status ?? "Available"))
                    {
                        await apiService.UpdateCondoStatusAsync(condoId, newStatus);
                    }
                }

                // Reload all properties from API to ensure consistency
                // Access PropertyFormManager through parentForm if needed
                if (parentForm is Principal principal && principal.propertyFormManager != null)
                {
                    await principal.propertyFormManager.LoadPropertiesFromApiAsync();
                }

                MessageBox.Show("Property updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form
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
                System.Diagnostics.Debug.WriteLine($"Exception in UpdateProperty: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error updating property: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task DeletePropertyAsync()
        {
            try
            {
                if (currentPropertyCard == null || currentPropertyCard.CondoId == 0)
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

                int condoId = currentPropertyCard.CondoId;
                string unitName = currentPropertyCard.PropertyData?.Title ?? "";

                // Call API to delete condo
                using (var apiService = new ApiService())
                {
                    await apiService.DeleteCondoAsync(condoId);
                }

                // Reload all properties from API to ensure consistency
                if (parentForm is Principal principal && principal.propertyFormManager != null)
                {
                    await principal.propertyFormManager.LoadPropertiesFromApiAsync();
                }

                // Clear current property card reference
                currentPropertyCard = null;
                originalPropertyData = null;

                MessageBox.Show("Property deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the update form
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
                System.Diagnostics.Debug.WriteLine($"Exception in DeleteProperty: {ex.Message}\n{ex.StackTrace}");
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
            isLoadingData = true; // Prevent event handler from firing during load
            
            try
            {
                // Load data into updPropertiesFirst (status)
                string status = data.Status ?? currentPropertyCard?.Status ?? "Available";
                int statusIndex = updateControl.availabilityCmb.Items.IndexOf(status);
                if (statusIndex >= 0)
                {
                    updateControl.availabilityCmb.SelectedIndex = statusIndex;
                }
                else
                {
                    updateControl.availabilityCmb.SelectedIndex = 0; // Default to Available
                }
                
                // Load booking link
                updateControl.txtBookingLink.Text = data.BookingLink ?? "";
                
                // Load data into updProperties1 (basic info)
                updateControl.updNameTxt.Text = data.Title ?? ""; // Unit Name/No.
                updateControl.updLocationTxt.Text = data.Location ?? ""; // Location
                updateControl.updPriceTxt.Text = data.Price ?? ""; // Price

                // Extract Room Type, Check-in, Check-out from Description
                string description = data.Bathrooms ?? ""; // Description is stored in Bathrooms field
                string roomType = "";
                string checkIn = "";
                string checkOut = "";
                
                // Parse description: "Room Type: Studio Type - Test | Check-in: 9:30am | Check-out: 12:30pm | Additional Images: ..."
                if (!string.IsNullOrEmpty(description))
                {
                    // Extract Room Type
                    if (description.Contains("Room Type:"))
                    {
                        int roomTypeStart = description.IndexOf("Room Type:") + 10;
                        int roomTypeEnd = description.IndexOf(" - ", roomTypeStart);
                        if (roomTypeEnd < 0) roomTypeEnd = description.IndexOf(" | ", roomTypeStart);
                        if (roomTypeEnd < 0) roomTypeEnd = description.Length;
                        if (roomTypeEnd > roomTypeStart)
                        {
                            roomType = description.Substring(roomTypeStart, roomTypeEnd - roomTypeStart).Trim();
                        }
                    }
                    
                    // Extract Check-in
                    if (description.Contains("Check-in:"))
                    {
                        int checkInStart = description.IndexOf("Check-in:") + 9;
                        int checkInEnd = description.IndexOf(" | ", checkInStart);
                        if (checkInEnd < 0) checkInEnd = description.IndexOf("Check-out:", checkInStart);
                        if (checkInEnd < 0) checkInEnd = description.Length;
                        if (checkInEnd > checkInStart)
                        {
                            checkIn = description.Substring(checkInStart, checkInEnd - checkInStart).Trim();
                        }
                    }
                    
                    // Extract Check-out
                    if (description.Contains("Check-out:"))
                    {
                        int checkOutStart = description.IndexOf("Check-out:") + 10;
                        int checkOutEnd = description.IndexOf(" | ", checkOutStart);
                        if (checkOutEnd < 0) checkOutEnd = description.IndexOf("Additional Images:", checkOutStart);
                        if (checkOutEnd < 0) checkOutEnd = description.Length;
                        if (checkOutEnd > checkOutStart)
                        {
                            checkOut = description.Substring(checkOutStart, checkOutEnd - checkOutStart).Trim();
                        }
                    }
                    
                    // Extract main description (after Room Type and before Check-in)
                    string mainDesc = description;
                    if (description.Contains("Room Type:"))
                    {
                        int descStart = description.IndexOf(" - ", description.IndexOf("Room Type:"));
                        if (descStart >= 0)
                        {
                            descStart += 3; // Skip " - "
                            int descEnd = description.IndexOf(" | ", descStart);
                            if (descEnd < 0) descEnd = description.IndexOf("Check-in:", descStart);
                            if (descEnd < 0) descEnd = description.IndexOf("Additional Images:", descStart);
                            if (descEnd < 0) descEnd = description.Length;
                            if (descEnd > descStart)
                            {
                                mainDesc = description.Substring(descStart, descEnd - descStart).Trim();
                            }
                        }
                    }
                    else if (description.Contains("Check-in:"))
                    {
                        int descEnd = description.IndexOf("Check-in:");
                        if (descEnd > 0)
                        {
                            mainDesc = description.Substring(0, descEnd).Trim();
                        }
                    }
                    
                    updateControl.updDescTxt.Text = mainDesc;
                }
                else
                {
                    updateControl.updDescTxt.Text = "";
                }

                // Set Room Type combo box
                if (!string.IsNullOrEmpty(roomType))
                {
                    int index = updateControl.updTypeCmb.Items.IndexOf(roomType);
                    if (index >= 0)
                    {
                        updateControl.updTypeCmb.SelectedIndex = index;
                    }
                    else
                    {
                        updateControl.updTypeCmb.Text = roomType;
                    }
                }

                // Set Check-in and Check-out
                updateControl.updInTxt.Text = checkIn;
                updateControl.updOutTxt.Text = checkOut;

                // Load data into updProperties2 (additional details)
                updateControl.updRuleTxt.Text = data.Bedrooms ?? ""; // Rules/Amenities field

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
            finally
            {
                isLoadingData = false;
            }
        }

        private PropertyData CollectFormData()
        {
            PropertyData data = new PropertyData();

            // Collect data from updProperties1 (basic info)
            data.Title = updateControl.updNameTxt.Text?.Trim(); // Unit Name/No.
            data.Location = updateControl.updLocationTxt.Text?.Trim(); // Location
            data.Price = updateControl.updPriceTxt.Text?.Trim(); // Price
            
            // Room Type from combo box
            string roomType = updateControl.updTypeCmb.SelectedItem?.ToString() ?? updateControl.updTypeCmb.Text?.Trim();
            
            // Check-in and Check-out times
            string checkIn = updateControl.updInTxt.Text?.Trim() ?? string.Empty;
            string checkOut = updateControl.updOutTxt.Text?.Trim() ?? string.Empty;

            // Collect data from updProperties2 (additional details)
            string mainDescription = updateControl.updDescTxt.Text?.Trim() ?? ""; // Description
            data.Bedrooms = updateControl.updRuleTxt.Text?.Trim() ?? ""; // Rules/Amenities field

            // Build full description with Room Type, Check-in, Check-out
            string fullDescription = mainDescription;
            if (!string.IsNullOrEmpty(roomType))
            {
                if (string.IsNullOrEmpty(fullDescription))
                    fullDescription = $"Room Type: {roomType}";
                else
                    fullDescription = $"Room Type: {roomType} - {fullDescription}";
            }
            
            if (!string.IsNullOrEmpty(checkIn) || !string.IsNullOrEmpty(checkOut))
            {
                string checkTimes = string.Empty;
                if (!string.IsNullOrEmpty(checkIn))
                    checkTimes += $"Check-in: {checkIn}";
                if (!string.IsNullOrEmpty(checkOut))
                    checkTimes += (!string.IsNullOrEmpty(checkTimes) ? " | " : "") + $"Check-out: {checkOut}";
                
                if (string.IsNullOrEmpty(fullDescription))
                    fullDescription = checkTimes;
                else
                    fullDescription += $" | {checkTimes}";
            }
            
            data.Bathrooms = fullDescription; // Store full description in Bathrooms field

            // Collect image paths from ImageManager
            data.Image1Path = imageManager.GetImagePath(1);
            data.Image2Path = imageManager.GetImagePath(2);
            data.Image3Path = imageManager.GetImagePath(3);
            data.Image4Path = imageManager.GetImagePath(4);

            // Set default area if not provided
            data.Area = originalPropertyData?.Area ?? "N/A";
            
            // Preserve CondoId and Status
            data.CondoId = originalPropertyData?.CondoId ?? 0;
            data.Status = originalPropertyData?.Status ?? "Available";

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


