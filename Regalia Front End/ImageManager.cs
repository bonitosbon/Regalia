using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Regalia_Front_End
{
    public class ImageManager
    {
        private PropertiesControl propertiesControl;
        private string[] imagePaths = new string[4]; // Store paths for image1, image2, image3, image4

        public ImageManager(PropertiesControl propertiesCtrl)
        {
            propertiesControl = propertiesCtrl;
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
            propertiesControl.image1.Click += (s, e) => HandleImageClick(1);
            propertiesControl.image2.Click += (s, e) => HandleImageClick(2);
            propertiesControl.image3.Click += (s, e) => HandleImageClick(3);
            propertiesControl.image4.Click += (s, e) => HandleImageClick(4);

            // Also wire up click events for the labels inside the panels
            propertiesControl.label13.Click += (s, e) => HandleImageClick(1);
            propertiesControl.label14.Click += (s, e) => HandleImageClick(2);
            propertiesControl.label15.Click += (s, e) => HandleImageClick(3);
            propertiesControl.label16.Click += (s, e) => HandleImageClick(4);

            // Set cursor to hand for better UX
            propertiesControl.image1.Cursor = Cursors.Hand;
            propertiesControl.image2.Cursor = Cursors.Hand;
            propertiesControl.image3.Cursor = Cursors.Hand;
            propertiesControl.image4.Cursor = Cursors.Hand;
            propertiesControl.label13.Cursor = Cursors.Hand;
            propertiesControl.label14.Cursor = Cursors.Hand;
            propertiesControl.label15.Cursor = Cursors.Hand;
            propertiesControl.label16.Cursor = Cursors.Hand;
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
                    pictureBox.Image = Image.FromFile(imagePath);
                    
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
                label.Location = new Point(30, 46);
            }
        }

        private Guna2Panel GetImagePanel(int imageIndex)
        {
            switch (imageIndex)
            {
                case 1: return propertiesControl.image1;
                case 2: return propertiesControl.image2;
                case 3: return propertiesControl.image3;
                case 4: return propertiesControl.image4;
                default: return null;
            }
        }

        private Label GetImageLabel(int imageIndex)
        {
            switch (imageIndex)
            {
                case 1: return propertiesControl.label13;
                case 2: return propertiesControl.label14;
                case 3: return propertiesControl.label15;
                case 4: return propertiesControl.label16;
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
