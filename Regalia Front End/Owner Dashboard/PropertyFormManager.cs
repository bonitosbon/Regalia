using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Regalia_Front_End.Models;
using Regalia_Front_End.Services;
using Regalia_Front_End.Helpers;
using System.Threading.Tasks;

namespace Regalia_Front_End
{
    public class PropertyFormManager
    {
        private Form parentForm;
        private PropertiesControl propertiesControl;
        public PropertyCardManager cardManager;
        private ImageManager imageManager;
        private PropertyStatusCardManager statusCardManager;
        private System.Windows.Forms.Timer animationTimer;
        private bool isAnimating = false;
        private bool isClosing = false;
        private int animationStep = 0;
        private int totalSteps = 30;
        private Point startPosition;
        private Point endPosition;
        private Guna2ShadowPanel currentForm;

        public PropertyFormManager(Form parent, PropertiesControl propertiesCtrl, ImageManager imgManager, PropertyStatusCardManager statusCardMgr = null)
        {
            parentForm = parent;
            propertiesControl = propertiesCtrl;
            imageManager = imgManager;
            cardManager = new PropertyCardManager(propertiesCtrl);
            statusCardManager = statusCardMgr;
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
                
                // Store property data temporarily (we'll save to DB when front desk account is created)
                // The property card will be created after successful API call in SubmitFrontDeskAccount
                
                if (navigateToPage4)
                {
                    // Navigate to page 4 (front desk account creation)
                    ShowProperties4();
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

        public async void SubmitFrontDeskAccount()
        {
            try
            {
                // Collect property data from pages 1-3
                PropertyData propertyData = CollectFormData();
                
                // Validate property data
                if (string.IsNullOrWhiteSpace(propertyData.Title))
                {
                    MessageBox.Show("Please enter a property title (Unit Name/No.).", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(propertyData.Location))
                {
                    MessageBox.Show("Please enter a property location.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(propertyData.Price))
                {
                    MessageBox.Show("Please enter a property price.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Collect front desk account data from addProperties4
                string frontDeskUsername = propertiesControl.frontDeskUsername.Text?.Trim() ?? string.Empty;
                string frontDeskPassword = propertiesControl.frontDeskPwd.Text?.Trim() ?? string.Empty;
                string confirmPassword = propertiesControl.confirmFrontDeskPwd.Text?.Trim() ?? string.Empty;
                
                // Validate front desk data
                if (string.IsNullOrEmpty(frontDeskUsername))
                {
                    MessageBox.Show("Please enter a front desk username.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrEmpty(frontDeskPassword))
                {
                    MessageBox.Show("Please enter a front desk password.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (frontDeskPassword != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match. Please confirm the password.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (frontDeskPassword.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Validate username format
                // Username should be alphanumeric with optional underscores/dots/hyphens
                if (!System.Text.RegularExpressions.Regex.IsMatch(frontDeskUsername, @"^[a-zA-Z0-9_.-]+$"))
                {
                    MessageBox.Show("Username can only contain letters, numbers, underscores, dots, and hyphens.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Parse price
                decimal pricePerNight;
                if (!decimal.TryParse(propertyData.Price, out pricePerNight))
                {
                    MessageBox.Show("Please enter a valid price.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Combine all images into a comma-separated string (or use first image if only one)
                // Convert images to base64 for web display
                List<string> imageList = new List<string>();
                if (!string.IsNullOrEmpty(propertyData.Image1Path)) 
                {
                    string base64Image1 = ImageBase64Helper.ConvertImageToBase64(propertyData.Image1Path);
                    if (!string.IsNullOrEmpty(base64Image1)) imageList.Add(base64Image1);
                }
                if (!string.IsNullOrEmpty(propertyData.Image2Path)) 
                {
                    string base64Image2 = ImageBase64Helper.ConvertImageToBase64(propertyData.Image2Path);
                    if (!string.IsNullOrEmpty(base64Image2)) imageList.Add(base64Image2);
                }
                if (!string.IsNullOrEmpty(propertyData.Image3Path)) 
                {
                    string base64Image3 = ImageBase64Helper.ConvertImageToBase64(propertyData.Image3Path);
                    if (!string.IsNullOrEmpty(base64Image3)) imageList.Add(base64Image3);
                }
                if (!string.IsNullOrEmpty(propertyData.Image4Path)) 
                {
                    string base64Image4 = ImageBase64Helper.ConvertImageToBase64(propertyData.Image4Path);
                    if (!string.IsNullOrEmpty(base64Image4)) imageList.Add(base64Image4);
                }
                
                // Use first image as primary ImageUrl, store all in Description or Amenities if needed
                string primaryImageUrl = imageList.Count > 0 ? imageList[0] : string.Empty;
                
                // Combine all images into Description or add to Amenities
                string fullDescription = propertyData.Bathrooms ?? string.Empty;
                if (imageList.Count > 1)
                {
                    string additionalImages = string.Join(", ", imageList.Skip(1));
                    if (!string.IsNullOrEmpty(fullDescription))
                        fullDescription += $" | Additional Images: {additionalImages}";
                    else
                        fullDescription = $"Additional Images: {additionalImages}";
                }
                
                // Create DTO for API
                CreateCondoDto condoDto = new CreateCondoDto
                {
                    Name = propertyData.Title,
                    Location = propertyData.Location,
                    Description = fullDescription, // Description includes room type, check-in/out, and additional images
                    Amenities = propertyData.Bedrooms ?? string.Empty, // Rules/Amenities from form
                    MaxGuests = 4, // Default value
                    PricePerNight = pricePerNight,
                    ImageUrl = primaryImageUrl, // Primary (first) image
                    FrontDeskUsername = frontDeskUsername,
                    FrontDeskPassword = frontDeskPassword
                };
                
                // Call API to create condo (this also creates front desk account)
                using (var apiService = new ApiService())
                {
                    await apiService.CreateCondoAsync(condoDto);
                }
                
                // If we get here, the API call was successful
                // Reload all properties from API to ensure consistency
                await LoadPropertiesFromApiAsync();
                
                // Clear all form data after successful submission
                ClearFormData();
                ClearFrontDeskFormData();
                
                // Close the form first
                HideFormWithAnimation(propertiesControl.addProperties4);
                
                // Show success message
                MessageBox.Show("Property and front desk account created successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Ensure cards are visible and brought to front
                if (cardManager != null)
                {
                    cardManager.BringCardContainerToFront();
                    propertiesControl.Refresh();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in SubmitFrontDeskAccount: {ex.Message}\n{ex.StackTrace}");
                
                // Check if it's a duplicate front desk username error
                string errorMessage = ex.Message;
                if (errorMessage.Contains("already taken") || errorMessage.Contains("already exists"))
                {
                    errorMessage = "This front desk username is already taken. Please choose a different username.";
                }
                else if (errorMessage.Contains("already in use"))
                {
                    errorMessage = "This front desk username is already in use. Please choose a different username.";
                }
                
                MessageBox.Show($"Error creating property: {errorMessage}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads all properties from the API and displays them as cards
        /// </summary>
        public async Task LoadPropertiesFromApiAsync()
        {
            try
            {
                using (var apiService = new ApiService())
                {
                    List<CondoResponse> condos = await apiService.GetOwnerCondosAsync();
                    
                    // Clear existing cards and status cards first
                    if (statusCardManager != null)
                    {
                        statusCardManager.ClearAllCards();
                    }
                    cardManager.ClearAllCards();
                    
                    if (condos == null || condos.Count == 0)
                    {
                        // No properties yet - that's fine, already cleared
                        return;
                    }
                    
                    // Suspend layout for smooth loading
                    propertiesControl.SuspendLayout();
                    if (cardManager.cardContainer != null)
                    {
                        cardManager.cardContainer.SuspendLayout();
                        cardManager.cardContainer.Visible = false; // Hide during load to prevent flicker
                    }
                    if (statusCardManager?.statusCardContainer != null)
                    {
                        statusCardManager.statusCardContainer.SuspendLayout();
                    }
                    
                    try
                    {
                        // Collect valid property data first
                        List<PropertyData> validProperties = new List<PropertyData>();
                        
                        foreach (var condo in condos)
                        {
                            // Skip if condo data is invalid
                            if (string.IsNullOrWhiteSpace(condo.Name))
                            {
                                continue;
                            }
                            
                            PropertyData propertyData = ConvertCondoToPropertyData(condo);
                            
                            // Validate property data before creating card
                            if (!propertyData.IsValid())
                            {
                                continue;
                            }
                            
                            validProperties.Add(propertyData);
                        }
                        
                        // Add all property cards at once (smooth loading)
                        foreach (var propertyData in validProperties)
                        {
                            cardManager.AddPropertyCard(propertyData);
                        }
                        
                        // Add all status cards at once
                        if (statusCardManager != null)
                        {
                            foreach (var propertyData in validProperties)
                            {
                                if (!string.IsNullOrWhiteSpace(propertyData.Title) && !string.IsNullOrWhiteSpace(propertyData.Location))
                                {
                                    statusCardManager.AddPropertyStatusCard(propertyData.Title, propertyData.Location, propertyData.Status);
                                }
                            }
                        }
                    }
                    finally
                    {
                        // Resume layout and refresh UI
                        if (statusCardManager?.statusCardContainer != null)
                        {
                            statusCardManager.statusCardContainer.ResumeLayout(false);
                            statusCardManager.statusCardContainer.PerformLayout();
                        }
                        if (cardManager.cardContainer != null)
                        {
                            cardManager.cardContainer.ResumeLayout(false);
                            cardManager.cardContainer.PerformLayout();
                            cardManager.cardContainer.Visible = true; // Show after loading
                        }
                        propertiesControl.ResumeLayout(false);
                        propertiesControl.PerformLayout();
                    }
                    
                    // Ensure cards are visible and refresh the UI
                    if (cardManager != null)
                    {
                        cardManager.BringCardContainerToFront();
                    }
                    
                    propertiesControl.Refresh();
                    propertiesControl.Invalidate();
                    
                    // Refresh dashboard counts if parent form is Principal and has dashboard control
                    if (parentForm is Principal principal && principal.dashboardControl != null)
                    {
                        _ = principal.dashboardControl.LoadDashboardCountsAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading properties from API: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error loading properties: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        /// <summary>
        /// Loads all properties from the API and displays them as cards (fire-and-forget version)
        /// </summary>
        public void LoadPropertiesFromApi()
        {
            // Fire and forget - don't await
            _ = LoadPropertiesFromApiAsync();
        }

        /// <summary>
        /// Converts a CondoResponse from API to PropertyData for display
        /// </summary>
        private PropertyData ConvertCondoToPropertyData(CondoResponse condo)
        {
            PropertyData data = new PropertyData
            {
                CondoId = condo.Id, // Store condo ID for update/delete operations
                Title = !string.IsNullOrWhiteSpace(condo.Name) ? condo.Name : "Unnamed Property",
                Location = !string.IsNullOrWhiteSpace(condo.Location) ? condo.Location : "Unknown Location",
                Price = condo.PricePerNight > 0 ? condo.PricePerNight.ToString("F2") : "0.00",
                Bathrooms = condo.Description ?? string.Empty, // Description
                Bedrooms = condo.Amenities ?? string.Empty, // Amenities/Rules
                Image1Path = condo.ImageUrl ?? string.Empty, // Primary image
                Area = "N/A",
                Status = condo.Status ?? "Available",
                BookingLink = condo.BookingLink ?? string.Empty, // Booking link
                CreatedDate = DateTime.Now // Could parse CreatedAt if needed
            };
            
            // Extract additional images (2-4) from description
            // Images are stored as: "Description text | Additional Images: data:image/xxx, data:image/yyy, data:image/zzz"
            if (!string.IsNullOrEmpty(condo.Description))
            {
                string description = condo.Description;
                string additionalImagesPrefix = "Additional Images:";
                int additionalImagesIndex = description.IndexOf(additionalImagesPrefix, StringComparison.OrdinalIgnoreCase);
                
                if (additionalImagesIndex >= 0)
                {
                    // Extract the additional images part
                    string additionalImagesText = description.Substring(additionalImagesIndex + additionalImagesPrefix.Length).Trim();
                    
                    // Split by ", data:image/" pattern to separate Base64 image strings
                    string[] imageParts = System.Text.RegularExpressions.Regex.Split(additionalImagesText, @",\s*data:image/");
                    
                    if (imageParts.Length > 0)
                    {
                        // First image part needs "data:image/" prefix added back
                        if (imageParts[0].Trim().StartsWith("data:image/"))
                        {
                            data.Image2Path = imageParts[0].Trim();
                        }
                        else
                        {
                            data.Image2Path = "data:image/" + imageParts[0].Trim();
                        }
                        
                        // Remaining images already have the prefix in the split
                        if (imageParts.Length > 1)
                        {
                            data.Image3Path = "data:image/" + imageParts[1].Trim();
                        }
                        if (imageParts.Length > 2)
                        {
                            data.Image4Path = "data:image/" + imageParts[2].Trim();
                        }
                    }
                }
            }
            
            return data;
        }

        #endregion

        #region Private Data Collection Methods

        private PropertyData CollectFormData()
        {
            PropertyData data = new PropertyData();
            
            // Collect data from addProperties1 (basic info)
            data.Title = propertiesControl.addUsernameTxt.Text?.Trim(); // Unit Name/No.
            data.Price = propertiesControl.addPriceTxt.Text?.Trim(); // Price
            data.Location = propertiesControl.addLocationTxt.Text?.Trim(); // Location
            
            // Room Type from combo box - store it for later use
            string roomType = propertiesControl.addTypeCmb.SelectedItem?.ToString() ?? propertiesControl.addTypeCmb.Text?.Trim();
            
            // Check-in and Check-out times
            string checkIn = propertiesControl.addInTxt.Text?.Trim() ?? string.Empty;
            string checkOut = propertiesControl.addOutTxt.Text?.Trim() ?? string.Empty;
            
            // Collect data from addProperties2 (additional details)
            data.Bedrooms = propertiesControl.addRulesTxt.Text?.Trim(); // Rules (Bedrooms field)
            data.Bathrooms = propertiesControl.addDescTxt.Text?.Trim(); // Description (Bathrooms field)
            
            // Store Room Type, Check-in, and Check-out in a temporary field (we'll combine into Description or Amenities)
            // Store them in the PropertyData for later use
            if (!string.IsNullOrEmpty(roomType))
            {
                // If Description exists, prepend room type
                if (string.IsNullOrEmpty(data.Bathrooms))
                    data.Bathrooms = $"Room Type: {roomType}";
                else
                    data.Bathrooms = $"Room Type: {roomType} - {data.Bathrooms}";
            }
            
            if (!string.IsNullOrEmpty(checkIn) || !string.IsNullOrEmpty(checkOut))
            {
                string checkTimes = string.Empty;
                if (!string.IsNullOrEmpty(checkIn))
                    checkTimes += $"Check-in: {checkIn}";
                if (!string.IsNullOrEmpty(checkOut))
                    checkTimes += (!string.IsNullOrEmpty(checkTimes) ? " | " : "") + $"Check-out: {checkOut}";
                
                // Add to Description (use separator instead of newline for better JSON compatibility)
                if (string.IsNullOrEmpty(data.Bathrooms))
                    data.Bathrooms = checkTimes;
                else
                    data.Bathrooms += $" | {checkTimes}";
            }
            
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
            propertiesControl.addUsernameTxt.Text = ""; // Unit Name/No.
            propertiesControl.addLocationTxt.Text = ""; // Location
            propertiesControl.addPriceTxt.Text = ""; // Price
            propertiesControl.addTypeCmb.SelectedIndex = -1; // Room Type
            propertiesControl.addTypeCmb.Text = "";
            propertiesControl.addInTxt.Text = ""; // Check-in
            propertiesControl.addOutTxt.Text = ""; // Check-out
            
            // Clear addProperties2 fields
            propertiesControl.addDescTxt.Text = ""; // Description
            propertiesControl.addRulesTxt.Text = ""; // Rules
            
            // Clear images using ImageManager
            imageManager.ClearImage(1);
            imageManager.ClearImage(2);
            imageManager.ClearImage(3);
            imageManager.ClearImage(4);
        }

        private void ClearFrontDeskFormData()
        {
            // Clear all textboxes in addProperties4
            propertiesControl.frontDeskUsername.Text = "";
            propertiesControl.frontDeskPwd.Text = "";
            propertiesControl.confirmFrontDeskPwd.Text = "";
        }

        #endregion

        #region Private Animation Methods

        private void ShowFormWithAnimation(Guna2ShadowPanel form)
        {
            if (isAnimating) return;

            currentForm = form;
            isAnimating = true;
            isClosing = false;

            // Enable and restore containerPanel so forms can be shown
            if (propertiesControl.containerPanel != null)
            {
                propertiesControl.RestoreContainerPanelPosition();
                propertiesControl.containerPanel.Enabled = true;
                propertiesControl.containerPanel.Visible = true;
            }

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
                    
                    // CRITICAL: Move containerPanel out of way when form is closed so cards can be clicked
                    if (propertiesControl.containerPanel != null)
                    {
                        // Force remove from Controls collection so it cannot intercept mouse events
                        if (propertiesControl.Controls.Contains(propertiesControl.containerPanel))
                        {
                            System.Diagnostics.Debug.WriteLine("HideFormWithAnimation: Removing containerPanel from Controls");
                            propertiesControl.Controls.Remove(propertiesControl.containerPanel);
                        }
                        propertiesControl.containerPanel.Enabled = false;
                        propertiesControl.containerPanel.Visible = false;
                        propertiesControl.MoveContainerPanelOutOfWay();
                    }
                    
                    // Bring cardContainer to front so cards are clickable
                    if (cardManager != null)
                    {
                        System.Diagnostics.Debug.WriteLine("HideFormWithAnimation: Calling BringCardContainerToFront");
                        cardManager.BringCardContainerToFront();
                    }
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
