using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Regalia_Front_End.Helpers
{
    /// <summary>
    /// Helper class for converting images to and from base64 format
    /// </summary>
    public static class ImageBase64Helper
    {
        /// <summary>
        /// Converts an image file to a base64 data URI string
        /// </summary>
        /// <param name="imagePath">Path to the image file</param>
        /// <returns>Base64 data URI string (e.g., "data:image/jpeg;base64,...") or empty string if conversion fails</returns>
        public static string ConvertImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return string.Empty;

            // If already a base64 data URI, return as is
            if (imagePath.StartsWith("data:image/"))
                return imagePath;

            // If it's a URL (http/https), return as is (don't convert URLs)
            if (imagePath.StartsWith("http://") || imagePath.StartsWith("https://"))
                return imagePath;

            try
            {
                // Check if file exists
                if (!File.Exists(imagePath))
                {
                    System.Diagnostics.Debug.WriteLine($"Image file not found: {imagePath}");
                    return string.Empty;
                }

                // Read the image file
                using (Image image = Image.FromFile(imagePath))
                {
                    // Determine the image format
                    ImageFormat format = image.RawFormat;
                    string mimeType = "image/jpeg"; // Default
                    
                    if (format.Equals(ImageFormat.Png))
                        mimeType = "image/png";
                    else if (format.Equals(ImageFormat.Gif))
                        mimeType = "image/gif";
                    else if (format.Equals(ImageFormat.Bmp))
                        mimeType = "image/bmp";
                    else if (format.Equals(ImageFormat.Jpeg))
                        mimeType = "image/jpeg";

                    // Convert to base64
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Save image to memory stream in the original format
                        image.Save(ms, format);
                        byte[] imageBytes = ms.ToArray();
                        
                        // Convert to base64 string
                        string base64String = Convert.ToBase64String(imageBytes);
                        
                        // Return as data URI
                        return $"data:{mimeType};base64,{base64String}";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error converting image to base64: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts a base64 data URI to an Image object
        /// </summary>
        /// <param name="base64String">Base64 data URI string</param>
        /// <returns>Image object or null if conversion fails</returns>
        public static Image ConvertBase64ToImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                // If it's a data URI, extract the base64 part
                string base64Data = base64String;
                if (base64String.Contains(","))
                {
                    base64Data = base64String.Substring(base64String.IndexOf(",") + 1);
                }

                // Convert base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                // Convert byte array to Image
                // Note: We need to create a new MemoryStream and keep it alive, or clone the image
                // For now, we'll create a new MemoryStream and clone the image
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image originalImage = Image.FromStream(ms);
                    // Clone the image so it doesn't depend on the stream
                    Image clonedImage = new Bitmap(originalImage);
                    originalImage.Dispose();
                    return clonedImage;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error converting base64 to image: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Checks if a string is a base64 data URI
        /// </summary>
        /// <param name="imageString">Image string to check</param>
        /// <returns>True if it's a base64 data URI, false otherwise</returns>
        public static bool IsBase64DataUri(string imageString)
        {
            if (string.IsNullOrEmpty(imageString))
                return false;

            return imageString.StartsWith("data:image/");
        }
    }
}

