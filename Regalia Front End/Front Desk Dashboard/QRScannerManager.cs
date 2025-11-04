using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Common;

namespace Regalia_Front_End.Front_Desk_Dashboard
{
    public class QRScannerManager : IDisposable
    {
        #region Private Fields
        private VideoCaptureDevice videoSource;
        private PictureBox cameraPreview;
        private Label statusLabel;
        private System.Windows.Forms.Timer scanTimer;
        private int frameCount = 0;
        private const int SCAN_INTERVAL = 8; // Scan every 8th frame (reduce CPU usage)
        private bool isScanning = false;
        private string lastScannedCode = string.Empty;
        #endregion

        #region Public Events
        public event EventHandler<string> OnQRCodeDetected;
        #endregion

        #region Constructor
        public QRScannerManager(PictureBox preview, Label status)
        {
            cameraPreview = preview ?? throw new ArgumentNullException(nameof(preview));
            statusLabel = status ?? throw new ArgumentNullException(nameof(status));
            
            // Initialize scan timer
            scanTimer = new System.Windows.Forms.Timer();
            scanTimer.Interval = 100; // Check every 100ms
            scanTimer.Tick += ScanTimer_Tick;
        }
        #endregion

        #region Public Methods
        public bool StartCamera()
        {
            try
            {
                // Get available video devices
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                
                System.Diagnostics.Debug.WriteLine($"Found {videoDevices.Count} video device(s)");
                
                if (videoDevices.Count == 0)
                {
                    UpdateStatus("No camera found. Please connect a camera.");
                    System.Diagnostics.Debug.WriteLine("No video devices found");
                    return false;
                }

                // Log available cameras
                for (int i = 0; i < videoDevices.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine($"Camera {i}: {videoDevices[i].Name}");
                }

                // Find the best camera (skip virtual cameras like OBS)
                int selectedCameraIndex = -1;
                string[] virtualCameraKeywords = { "OBS", "Virtual", "ManyCam", "XSplit", "DroidCam" };
                
                // First, try to find a real webcam (skip virtual ones)
                for (int i = 0; i < videoDevices.Count; i++)
                {
                    string cameraName = videoDevices[i].Name.ToUpper();
                    bool isVirtual = false;
                    
                    foreach (string keyword in virtualCameraKeywords)
                    {
                        if (cameraName.Contains(keyword.ToUpper()))
                        {
                            isVirtual = true;
                            System.Diagnostics.Debug.WriteLine($"Skipping virtual camera: {videoDevices[i].Name}");
                            break;
                        }
                    }
                    
                    if (!isVirtual)
                    {
                        selectedCameraIndex = i;
                        break;
                    }
                }
                
                // If no real camera found, use the last one (usually a real webcam if OBS is first)
                if (selectedCameraIndex == -1)
                {
                    selectedCameraIndex = videoDevices.Count > 1 ? videoDevices.Count - 1 : 0;
                    System.Diagnostics.Debug.WriteLine($"No non-virtual camera found, using camera index: {selectedCameraIndex}");
                }

                // Use selected camera
                videoSource = new VideoCaptureDevice(videoDevices[selectedCameraIndex].MonikerString);
                System.Diagnostics.Debug.WriteLine($"Using camera {selectedCameraIndex}: {videoDevices[selectedCameraIndex].Name}");
                
                // Set video resolution to reduce CPU usage (640x480 is enough for QR scanning)
                videoSource.VideoResolution = SelectLowestResolution(videoSource.VideoCapabilities);
                
                // Ensure PictureBox is configured for display
                if (cameraPreview != null && !cameraPreview.IsDisposed)
                {
                    if (cameraPreview.InvokeRequired)
                    {
                        cameraPreview.Invoke(new Action(() =>
                        {
                            cameraPreview.SizeMode = PictureBoxSizeMode.Zoom;
                        }));
                    }
                    else
                    {
                        cameraPreview.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                
                // Wire up new frame event
                videoSource.NewFrame += VideoSource_NewFrame;
                
                // Start camera
                videoSource.Start();
                
                // Wait a moment and check if camera actually started
                System.Threading.Thread.Sleep(500);
                
                if (!videoSource.IsRunning)
                {
                    UpdateStatus("Camera failed to start. Check permissions.");
                    System.Diagnostics.Debug.WriteLine("Camera did not start after 500ms");
                    videoSource.SignalToStop();
                    videoSource = null;
                    return false;
                }
                
                isScanning = true;
                scanTimer.Start();
                
                System.Diagnostics.Debug.WriteLine("Camera started successfully");
                UpdateStatus("Scanning...");
                return true;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Camera error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error starting camera: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public void StopCamera()
        {
            try
            {
                isScanning = false;
                scanTimer?.Stop();
                
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource.NewFrame -= VideoSource_NewFrame;
                }
                
                // Clear preview
                if (cameraPreview != null && cameraPreview.InvokeRequired)
                {
                    cameraPreview.Invoke(new Action(() => cameraPreview.Image = null));
                }
                else if (cameraPreview != null)
                {
                    cameraPreview.Image = null;
                }
                
                UpdateStatus("Camera stopped");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping camera: {ex.Message}");
            }
        }

        public void UpdateStatus(string message)
        {
            if (statusLabel == null) return;
            
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action(() => statusLabel.Text = message));
            }
            else
            {
                statusLabel.Text = message;
            }
        }

        public void ResetScanning()
        {
            lastScannedCode = string.Empty;
            isScanning = true;
        }
        #endregion

        #region Private Methods
        private VideoCapabilities SelectLowestResolution(VideoCapabilities[] capabilities)
        {
            if (capabilities == null || capabilities.Length == 0)
                return null;

            // Find resolution closest to 640x480 for optimal performance
            VideoCapabilities best = capabilities[0];
            int targetWidth = 640;
            int targetHeight = 480;
            int bestDiff = int.MaxValue;

            foreach (var cap in capabilities)
            {
                int diff = Math.Abs(cap.FrameSize.Width - targetWidth) + 
                          Math.Abs(cap.FrameSize.Height - targetHeight);
                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    best = cap;
                }
            }

            return best;
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                if (eventArgs?.Frame == null)
                {
                    System.Diagnostics.Debug.WriteLine("Received null frame from camera");
                    return;
                }

                // Get frame from camera
                Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
                
                // Update preview on UI thread
                if (cameraPreview != null && !cameraPreview.IsDisposed)
                {
                    if (cameraPreview.InvokeRequired)
                    {
                        cameraPreview.Invoke(new Action(() =>
                        {
                            try
                            {
                                if (cameraPreview.Image != null)
                                {
                                    cameraPreview.Image.Dispose();
                                }
                                cameraPreview.Image = (Bitmap)frame.Clone();
                                cameraPreview.Refresh(); // Force refresh
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error updating PictureBox: {ex.Message}");
                            }
                        }));
                    }
                    else
                    {
                        try
                        {
                            if (cameraPreview.Image != null)
                            {
                                cameraPreview.Image.Dispose();
                            }
                            cameraPreview.Image = (Bitmap)frame.Clone();
                            cameraPreview.Refresh(); // Force refresh
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error updating PictureBox: {ex.Message}");
                        }
                    }
                }
                
                // Increment frame counter for scanning
                frameCount++;
                
                // Log first few frames to confirm camera is working
                if (frameCount <= 3)
                {
                    System.Diagnostics.Debug.WriteLine($"Received frame {frameCount}, size: {frame.Width}x{frame.Height}");
                }
                
                // Dispose original frame to prevent memory leak
                frame.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing camera frame: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private void ScanTimer_Tick(object sender, EventArgs e)
        {
            if (!isScanning || cameraPreview == null || cameraPreview.Image == null)
                return;

            // Only scan every Nth frame to reduce CPU usage
            if (frameCount % SCAN_INTERVAL != 0)
                return;

            try
            {
                Bitmap imageToScan = null;
                
                // Get image from preview
                if (cameraPreview.InvokeRequired)
                {
                    cameraPreview.Invoke(new Action(() =>
                    {
                        if (cameraPreview.Image != null)
                        {
                            imageToScan = (Bitmap)cameraPreview.Image.Clone();
                        }
                    }));
                }
                else
                {
                    if (cameraPreview.Image != null)
                    {
                        imageToScan = (Bitmap)cameraPreview.Image.Clone();
                    }
                }

                if (imageToScan == null)
                    return;

                // Use ZXing to decode QR code
                BarcodeReader reader = new BarcodeReader
                {
                    Options = new DecodingOptions
                    {
                        TryHarder = false, // Don't use too much CPU
                        PossibleFormats = new[] { BarcodeFormat.QR_CODE }
                    }
                };

                Result result = reader.Decode(imageToScan);
                imageToScan.Dispose();

                if (result != null && !string.IsNullOrEmpty(result.Text))
                {
                    // Avoid duplicate scans
                    if (result.Text == lastScannedCode)
                        return;

                    lastScannedCode = result.Text;
                    isScanning = false; // Stop scanning once QR is found
                    
                    UpdateStatus("QR Code detected!");
                    
                    // Fire event
                    OnQRCodeDetected?.Invoke(this, result.Text);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning QR code: {ex.Message}");
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            StopCamera();
            scanTimer?.Dispose();
            // VideoCaptureDevice doesn't implement IDisposable, but StopCamera() already handles cleanup
            videoSource = null;
        }
        #endregion
    }
}

