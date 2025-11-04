using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Regalia_Front_End.Owner_Dashboard;
using Regalia_Front_End.Helpers;

namespace Regalia_Front_End
{
    public class UpdateImageManager
    {
        private PropertiesUpdateControl updateControl;
        private string[] imagePaths = new string[4]; // Store paths for image1, image2, image3, image4

        public UpdateImageManager(PropertiesUpdateControl updateCtrl)
        {
            updateControl = updateCtrl;
            InitializeImagePanels();
        }

        #region Public Methods

        public void HandleImageClick(int imageIndex)
        {
            if (imageIndex < 1 || imageIndex > 4)
            {
                MessageBox.Show("Invalid image index!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenImageDialog(imageIndex);
        }

        public string GetImagePath(int imageIndex)
        {
            if (imageIndex < 1 || imageIndex > 4) return null;
            return imagePaths[imageIndex - 1];
        }

        public bool HasImage(int imageIndex)
        {
            if (imageIndex < 1 || imageIndex > 4) return false;
            return !string.IsNullOrEmpty(imagePaths[imageIndex - 1]);
        }

        public void LoadImage(int imageIndex, string imagePath)
        {
            if (imageIndex < 1 || imageIndex > 4) return;

            if (!string.IsNullOrEmpty(imagePath))
            {
                // Check if it's a base64 data URI or a file path
                bool isValidPath = false;
                if (imagePath.StartsWith("data:image/"))
                {
                    // It's a base64 image, we can display it
                    isValidPath = true;
                }
                else if (File.Exists(imagePath))
                {
                    // It's a valid file path
                    isValidPath = true;
                }

                if (isValidPath)
                {
                    imagePaths[imageIndex - 1] = imagePath;
                    DisplayImageInPanel(imageIndex, imagePath);
                }
            }
        }

        public void ClearImage(int imageIndex)
        {
            if (imageIndex < 1 || imageIndex > 4) return;

            imagePaths[imageIndex - 1] = null;
            ResetImagePanel(imageIndex);
        }

        #endregion

        #region Private Methods

        private void InitializeImagePanels()
        {
            // Wire up click events for all image panels
            updateControl.updateImage1.Click += (s, e) => HandleImageClick(1);
            updateControl.updateImage2.Click += (s, e) => HandleImageClick(2);
            updateControl.updateImage3.Click += (s, e) => HandleImageClick(3);
            updateControl.updateImage4.Click += (s, e) => HandleImageClick(4);

            // Also wire up click events for the labels inside the panels
            updateControl.label13.Click += (s, e) => HandleImageClick(1);
            updateControl.label14.Click += (s, e) => HandleImageClick(2);
            updateControl.label15.Click += (s, e) => HandleImageClick(3);
            updateControl.label16.Click += (s, e) => HandleImageClick(4);

            // Set cursor to hand for better UX
            updateControl.updateImage1.Cursor = Cursors.Hand;
            updateControl.updateImage2.Cursor = Cursors.Hand;
            updateControl.updateImage3.Cursor = Cursors.Hand;
            updateControl.updateImage4.Cursor = Cursors.Hand;
            updateControl.label13.Cursor = Cursors.Hand;
            updateControl.label14.Cursor = Cursors.Hand;
            updateControl.label15.Cursor = Cursors.Hand;
            updateControl.label16.Cursor = Cursors.Hand;
        }

        private void OpenImageDialog(int imageIndex)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                openFileDialog.Title = $"Select Image {imageIndex}";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string imagePath = openFileDialog.FileName;

                        // Validate image file
                        if (IsValidImageFile(imagePath))
                        {
                            // Store the image path
                            imagePaths[imageIndex - 1] = imagePath;

                            // Display the image in the panel
                            DisplayImageInPanel(imageIndex, imagePath);

                            MessageBox.Show($"Image {imageIndex} uploaded successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Please select a valid image file!", "Invalid File",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool IsValidImageFile(string filePath)
        {
            try
            {
                using (Image img = Image.FromFile(filePath))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void DisplayImageInPanel(int imageIndex, string imagePath)
        {
            try
            {
                Guna2Panel panel = GetImagePanel(imageIndex);
                Label label = GetImageLabel(imageIndex);

                if (panel != null && label != null)
                {
                    // Clear existing controls
                    panel.Controls.Clear();

                    // Create PictureBox to display the image
                    PictureBox pictureBox = new PictureBox
                    {
                        Size = panel.Size,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Fill,
                        BackColor = Color.White
                    };

                    // Load and display the image
                    if (imagePath.StartsWith("data:image/"))
                    {
                        // Convert base64 to Image
                        pictureBox.Image = ImageBase64Helper.ConvertBase64ToImage(imagePath);
                    }
                    else
                    {
                        // Load from file path
                        pictureBox.Image = Image.FromFile(imagePath);
                    }

                    // Add PictureBox to the panel
                    panel.Controls.Add(pictureBox);

                    // Add a small "X" button to remove the image
                    Button removeButton = new Button
                    {
                        Text = "×",
                        Size = new Size(20, 20),
                        Location = new Point(panel.Width - 25, 5),
                        BackColor = Color.Red,
                        ForeColor = Color.White,
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };

                    removeButton.FlatAppearance.BorderSize = 0;
                    removeButton.Click += (s, e) => ClearImage(imageIndex);

                    panel.Controls.Add(removeButton);
                    removeButton.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying image: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetImagePanel(int imageIndex)
        {
            Guna2Panel panel = GetImagePanel(imageIndex);
            Label label = GetImageLabel(imageIndex);

            if (panel != null && label != null)
            {
                // Clear existing controls
                panel.Controls.Clear();

                // Add back the original label
                panel.Controls.Add(label);

                // Reset label properties
                label.Text = "Add image";
                label.AutoSize = true;
                label.ForeColor = Color.FromArgb(69, 183, 135);
                label.Location = new Point(22, 37);
            }
        }

        private Guna2Panel GetImagePanel(int imageIndex)
        {
            switch (imageIndex)
            {
                case 1: return updateControl.updateImage1;
                case 2: return updateControl.updateImage2;
                case 3: return updateControl.updateImage3;
                case 4: return updateControl.updateImage4;
                default: return null;
            }
        }

        private Label GetImageLabel(int imageIndex)
        {
            switch (imageIndex)
            {
                case 1: return updateControl.label13;
                case 2: return updateControl.label14;
                case 3: return updateControl.label15;
                case 4: return updateControl.label16;
                default: return null;
            }
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {
            // Dispose of any loaded images
            for (int i = 0; i < imagePaths.Length; i++)
            {
                imagePaths[i] = null;
            }
        }

        #endregion
    }
}



