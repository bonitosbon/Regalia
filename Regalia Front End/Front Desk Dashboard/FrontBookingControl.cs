using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Regalia_Front_End.Models;
using Regalia_Front_End.Services;

namespace Regalia_Front_End.Front_Desk_Dashboard
{
    public partial class FrontBookingControl : UserControl
    {
        private const int CARD_SPACING = 15;
        private FlowLayoutPanel confirmedBookingsPanel;
        private QRScannerManager qrScannerManager;
        private Form parentForm;
        private BookingResponse currentBookingForCheckIn;
        
        // Animation variables
        private System.Windows.Forms.Timer scannerAnimationTimer;
        private bool isScannerAnimating = false;
        private bool isScannerClosing = false;
        private Point scannerStartPosition;
        private Point scannerEndPosition;
        private int scannerAnimationStep = 0;
        private int scannerTotalSteps = 30;

        public FrontBookingControl()
        {
            System.Diagnostics.Debug.WriteLine("FrontBookingControl constructor called!");
            
            InitializeComponent();
            InitializeConfirmedBookingsPanel();
            InitializeScanner();
            Load += FrontBookingControl_Load;
            VisibleChanged += FrontBookingControl_VisibleChanged;
        }
        
        private void InitializeScanner()
        {
            // Remove scanner panel from this control if it's here (it will be added to parent form)
            if (this.Controls.Contains(cameraPanel))
            {
                this.Controls.Remove(cameraPanel);
                System.Diagnostics.Debug.WriteLine("Scanner panel removed from FrontBookingControl");
            }
            
            // Initialize QR scanner manager
            qrScannerManager = new QRScannerManager(camerScanner, scannerStatus);
            qrScannerManager.OnQRCodeDetected += QRScannerManager_OnQRCodeDetected;
            
            // Wire up close button
            closeScanner.Click += CloseScanner_Click;
            
            // Initially hide scanner panel
            cameraPanel.Visible = false;
        }
        
        public void SetParentForm(Form form)
        {
            parentForm = form;
            // Remove from this control if it's here
            if (this.Controls.Contains(cameraPanel))
            {
                this.Controls.Remove(cameraPanel);
            }
            
            // Add scanner panel to parent form for proper animation
            if (parentForm != null && !parentForm.Controls.Contains(cameraPanel))
            {
                parentForm.Controls.Add(cameraPanel);
                cameraPanel.BringToFront();
                cameraPanel.Visible = false;
                System.Diagnostics.Debug.WriteLine("Scanner panel added to parent form");
            }
        }
        
        public void WireUpBookingCardClick(BookingCard card)
        {
            // Wire up OnCardClicked event to open scanner
            System.Diagnostics.Debug.WriteLine($"Wiring up BookingCard click for: {card.BookingData?.FullName}, Status: {card.BookingData?.Status}");
            
            // Remove existing handler first to avoid duplicates
            card.OnCardClicked -= BookingCard_OnCardClicked;
            
            // Add handler
            card.OnCardClicked += BookingCard_OnCardClicked;
            
            // Event wired successfully
            System.Diagnostics.Debug.WriteLine($"Card {card.BookingData?.FullName} event handler wired");
        }
        
        private void BookingCard_OnCardClicked(object sender, BookingResponse bookingData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"BookingCard_OnCardClicked received - Name: {bookingData?.FullName}, Status: {bookingData?.Status}");
                
                // Open scanner for "Approved" or "CheckedIn" bookings
                if (bookingData != null && (bookingData.Status == "Approved" || bookingData.Status == "CheckedIn"))
                {
                    System.Diagnostics.Debug.WriteLine($"Opening scanner for {bookingData.Status} booking");
                    OpenScannerForBooking(bookingData);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Booking status is '{bookingData?.Status}', not 'Approved' or 'CheckedIn'. Scanner not opened.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in BookingCard_OnCardClicked: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error opening scanner: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void OpenScannerForBooking(BookingResponse booking)
        {
            System.Diagnostics.Debug.WriteLine($"OpenScannerForBooking called for: {booking?.FullName}");
            System.Diagnostics.Debug.WriteLine($"cameraPanel is null: {cameraPanel == null}");
            if (cameraPanel != null)
            {
                System.Diagnostics.Debug.WriteLine($"cameraPanel.Name: {cameraPanel.Name}, Visible: {cameraPanel.Visible}");
            }
            currentBookingForCheckIn = booking;
            ShowScannerWithAnimation();
        }
        
        private void ShowScannerWithAnimation()
        {
            System.Diagnostics.Debug.WriteLine("ShowScannerWithAnimation called");
            if (isScannerAnimating)
            {
                System.Diagnostics.Debug.WriteLine("Scanner is already animating, skipping");
                return;
            }
            
            // Check if cameraPanel is null
            if (cameraPanel == null)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: cameraPanel is null!");
                MessageBox.Show("Camera panel not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            isScannerAnimating = true;
            isScannerClosing = false;
            
            // Ensure parent form is set
            if (parentForm == null)
            {
                parentForm = this.FindForm();
                System.Diagnostics.Debug.WriteLine($"Parent form found: {parentForm != null}");
            }
            
            if (parentForm == null)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Parent form is null! Cannot show scanner.");
                MessageBox.Show("Cannot show scanner: Parent form not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isScannerAnimating = false;
                return;
            }
            
            // Remove from current control if it's here (shouldn't be, but just in case)
            if (this.Controls.Contains(cameraPanel))
            {
                this.Controls.Remove(cameraPanel);
                System.Diagnostics.Debug.WriteLine("Removed cameraPanel from FrontBookingControl");
            }
            
            // Add to parent form if not already there
            if (!parentForm.Controls.Contains(cameraPanel))
            {
                parentForm.Controls.Add(cameraPanel);
                System.Diagnostics.Debug.WriteLine("Scanner panel added to parent form");
            }
            
            // Calculate positions - center the panel both horizontally and vertically
            int centerX = (parentForm.ClientSize.Width - cameraPanel.Width) / 2;
            int centerY = (parentForm.ClientSize.Height - cameraPanel.Height) / 2;
            // Ensure positive coordinates
            if (centerX < 0) centerX = 0;
            if (centerY < 0) centerY = 0;
            scannerEndPosition = new Point(centerX, centerY);
            scannerStartPosition = new Point(centerX, parentForm.ClientSize.Height + 50);
            
            // Set initial position and make visible
            cameraPanel.Location = scannerStartPosition;
            cameraPanel.Visible = true;
            cameraPanel.BringToFront();
            
            // Reset scanner
            scannerStatus.Text = "Initializing camera...";
            qrScannerManager.ResetScanning();
            
            // Start camera
            bool cameraStarted = qrScannerManager.StartCamera();
            if (!cameraStarted)
            {
                scannerStatus.Text = "Camera not available";
            }
            
            // Start animation
            StartScannerAnimation();
        }
        
        private void HideScannerWithAnimation()
        {
            if (isScannerAnimating) return;
            
            isScannerAnimating = true;
            isScannerClosing = true;
            
            // Stop camera
            qrScannerManager.StopCamera();
            
            // Calculate positions
            scannerStartPosition = cameraPanel.Location;
            scannerEndPosition = new Point(scannerStartPosition.X, parentForm.ClientSize.Height + 50);
            
            // Start animation
            StartScannerAnimation();
        }
        
        private void StartScannerAnimation()
        {
            if (scannerAnimationTimer != null)
            {
                scannerAnimationTimer.Stop();
                scannerAnimationTimer.Dispose();
            }
            
            scannerAnimationTimer = new System.Windows.Forms.Timer();
            scannerAnimationTimer.Interval = 16; // ~60fps
            scannerAnimationTimer.Tick += ScannerAnimationTimer_Tick;
            scannerAnimationStep = 0;
            scannerTotalSteps = isScannerClosing ? 20 : 30; // Faster closing
            scannerAnimationTimer.Start();
        }
        
        private void ScannerAnimationTimer_Tick(object sender, EventArgs e)
        {
            scannerAnimationStep++;
            double progress = (double)scannerAnimationStep / scannerTotalSteps;
            
            // Apply easing
            if (isScannerClosing)
            {
                progress = progress * progress; // Ease-in for closing
            }
            else
            {
                progress = 1 - Math.Pow(1 - progress, 3); // Ease-out for opening
            }
            
            // Calculate current position
            int currentX = scannerStartPosition.X + (int)((scannerEndPosition.X - scannerStartPosition.X) * progress);
            int currentY = scannerStartPosition.Y + (int)((scannerEndPosition.Y - scannerStartPosition.Y) * progress);
            
            cameraPanel.Location = new Point(currentX, currentY);
            
            if (scannerAnimationStep >= scannerTotalSteps)
            {
                scannerAnimationTimer.Stop();
                scannerAnimationTimer.Dispose();
                scannerAnimationTimer = null;
                
                if (isScannerClosing)
                {
                    cameraPanel.Visible = false;
                    // Reset position for next opening
                    int centerX = (parentForm.ClientSize.Width - cameraPanel.Width) / 2;
                    int centerY = (parentForm.ClientSize.Height - cameraPanel.Height) / 2;
                    // Ensure panel is centered both horizontally and vertically
                    if (centerX < 0) centerX = 0;
                    if (centerY < 0) centerY = 0;
                    cameraPanel.Location = new Point(centerX, centerY);
                }
                else
                {
                    cameraPanel.Location = scannerEndPosition; // Ensure exact final position
                }
                
                isScannerAnimating = false;
            }
        }
        
        private async void QRScannerManager_OnQRCodeDetected(object sender, string qrCodeData)
        {
            if (currentBookingForCheckIn == null)
            {
                MessageBox.Show("No booking selected for check-in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                // Check if already checked in (second scan = checkout)
                bool isAlreadyCheckedIn = currentBookingForCheckIn.Status == "CheckedIn";
                
                if (isAlreadyCheckedIn)
                {
                    // Second scan - checkout/remove booking
                    scannerStatus.Text = "Processing checkout...";
                    
                    // Call API to checkout
                    using (var apiService = new ApiService())
                    {
                        await apiService.CheckOutGuestAsync(currentBookingForCheckIn.Id, qrCodeData);
                    }
                    
                    // Update the booking status locally to CheckedOut (unavailable)
                    currentBookingForCheckIn.Status = "CheckedOut";
                    
                    scannerStatus.Text = "Checkout successful!";
                    await Task.Delay(500);
                    
                    HideScannerWithAnimation();
                    
                    // Remove the booking card from the panel (CheckedOut bookings won't show)
                    RemoveBookingCard(currentBookingForCheckIn.Id);
                    
                    // Refresh bookings (CheckedOut will be filtered out)
                    await LoadConfirmedBookingsAsync();
                    
                    // Remove from departure and refresh dashboard
                    RefreshDashboardAndRemoveFromDeparture(currentBookingForCheckIn.Id);
                    
                    MessageBox.Show("Guest checked out successfully! Booking is now unavailable.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // First scan - check in
                    scannerStatus.Text = "Processing check-in...";
                    
                    // Validate booking status before check-in
                    if (currentBookingForCheckIn.Status != "Approved")
                    {
                        MessageBox.Show($"Booking must be approved before check-in. Current status: {currentBookingForCheckIn.Status}", 
                            "Check-in Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        scannerStatus.Text = "Check-in failed: Booking not approved";
                        return;
                    }
                    
                    // Validate QR code data
                    if (string.IsNullOrEmpty(qrCodeData))
                    {
                        MessageBox.Show("QR code data is empty. Please scan a valid QR code.", 
                            "Check-in Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        scannerStatus.Text = "Check-in failed: Invalid QR code";
                        return;
                    }
                    
                    // Debug: Log QR code comparison
                    System.Diagnostics.Debug.WriteLine($"Scanned QR Code: {qrCodeData}");
                    System.Diagnostics.Debug.WriteLine($"Booking QR Code: {currentBookingForCheckIn.QrCodeData}");
                    System.Diagnostics.Debug.WriteLine($"QR Codes Match: {qrCodeData == currentBookingForCheckIn.QrCodeData}");
                    
                    // Call API to check in the guest
                    using (var apiService = new ApiService())
                    {
                        await apiService.CheckInGuestAsync(currentBookingForCheckIn.Id, qrCodeData);
                    }
                    
                    // Update the booking status locally
                    currentBookingForCheckIn.Status = "CheckedIn";
                    
                    // Update the booking card immediately to show "Checked In" status
                    foreach (Control control in confirmedBookingsPanel.Controls)
                    {
                        if (control is BookingCard card && card.BookingData?.Id == currentBookingForCheckIn.Id)
                        {
                            card.UpdateStatus("CheckedIn");
                            break;
                        }
                    }
                    
                    // Success - close scanner and refresh bookings
                    scannerStatus.Text = "Check-in successful!";
                    await Task.Delay(500); // Brief delay to show success message
                    
                    HideScannerWithAnimation();
                    
                    // Refresh bookings to show updated status
                    await LoadConfirmedBookingsAsync();
                    
                    // Notify frontDashboard to move from upcoming to departure
                    RefreshDashboard();
                    
                    MessageBox.Show("Guest checked in successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (errorMessage.Contains("Invalid QR code"))
                {
                    errorMessage = "Invalid QR code. Please scan the correct QR code for this booking.";
                }
                else if (errorMessage.Contains("not approved"))
                {
                    errorMessage = "Booking must be approved before check-in.";
                }
                else if (errorMessage.Contains("not allowed before"))
                {
                    errorMessage = "Check-in is not allowed before the scheduled start time.";
                }
                
                scannerStatus.Text = "Check-in failed";
                MessageBox.Show($"Error during check-in: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Reset scanning to allow retry
                qrScannerManager.ResetScanning();
            }
        }

        private void RemoveBookingCard(int bookingId)
        {
            // Find and remove the booking card from the panel
            foreach (Control control in confirmedBookingsPanel.Controls)
            {
                if (control is BookingCard card && card.BookingData?.Id == bookingId)
                {
                    confirmedBookingsPanel.Controls.Remove(card);
                    card.Dispose();
                    System.Diagnostics.Debug.WriteLine($"Removed booking card for booking ID: {bookingId}");
                    break;
                }
            }
        }

        private void RefreshDashboard()
        {
            // Find the frontDashboard control and refresh it
            var parent = this.Parent;
            while (parent != null)
            {
                if (parent is PrincipalFront principalFront)
                {
                    // Find frontDashboard control
                    foreach (Control control in principalFront.Controls)
                    {
                        if (control is Guna.UI2.WinForms.Guna2Panel panel)
                        {
                            foreach (Control child in panel.Controls)
                            {
                                if (child is frontDashboard dashboard)
                                {
                                    _ = dashboard.LoadUpcomingBookingsAsync();
                                    System.Diagnostics.Debug.WriteLine("Refreshed frontDashboard");
                                    return;
                                }
                            }
                        }
                    }
                }
                parent = parent.Parent;
            }
        }

        private void RefreshDashboardAndRemoveFromDeparture(int bookingId)
        {
            // Find the frontDashboard control and remove booking from departure
            var parent = this.Parent;
            while (parent != null)
            {
                if (parent is PrincipalFront principalFront)
                {
                    // Find frontDashboard control
                    foreach (Control control in principalFront.Controls)
                    {
                        if (control is Guna.UI2.WinForms.Guna2Panel panel)
                        {
                            foreach (Control child in panel.Controls)
                            {
                                if (child is frontDashboard dashboard)
                                {
                                    dashboard.RemoveBookingFromDeparture(bookingId);
                                    _ = dashboard.LoadUpcomingBookingsAsync();
                                    System.Diagnostics.Debug.WriteLine($"Removed booking {bookingId} from departure and refreshed frontDashboard");
                                    return;
                                }
                            }
                        }
                    }
                }
                parent = parent.Parent;
            }
        }
        
        private void CloseScanner_Click(object sender, EventArgs e)
        {
            HideScannerWithAnimation();
        }

        private void InitializeConfirmedBookingsPanel()
        {
            // Create FlowLayoutPanel for confirmed bookings
            confirmedBookingsPanel = new FlowLayoutPanel();
            confirmedBookingsPanel.AutoScroll = true;
            confirmedBookingsPanel.Dock = DockStyle.Fill;
            confirmedBookingsPanel.FlowDirection = FlowDirection.TopDown;
            confirmedBookingsPanel.WrapContents = false;
            confirmedBookingsPanel.Padding = new Padding(CARD_SPACING);
            confirmedBookingsPanel.BackColor = Color.Transparent;
            
            // Ensure panel doesn't intercept mouse events
            confirmedBookingsPanel.Click += (s, e) => 
            {
                // Allow clicks to pass through to child controls
                System.Diagnostics.Debug.WriteLine("FlowLayoutPanel clicked - this should not happen if cards are working");
            };
            
            // Add to the control
            this.Controls.Add(confirmedBookingsPanel);
            confirmedBookingsPanel.BringToFront();
        }

        private async void FrontBookingControl_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("FrontBookingControl_Load called!");
            await LoadConfirmedBookingsAsync();
        }

        public async Task LoadConfirmedBookingsAsync()
        {
            System.Diagnostics.Debug.WriteLine("LoadConfirmedBookingsAsync called!");
            
            try
            {
                var apiService = new ApiService();
                var bookings = await apiService.GetFrontDeskBookingsAsync();
                
                // Clear existing cards
                confirmedBookingsPanel.Controls.Clear();
                
                // Filter for approved/confirmed bookings (exclude CheckedOut - they are unavailable)
                var confirmedBookings = bookings
                    .Where(b => b.Status == "Approved" || b.Status == "CheckedIn")
                    .Where(b => b.Status != "CheckedOut")
                    .OrderByDescending(b => b.CreatedAt)
                    .ToList();
                
                // Add cards for each confirmed booking
                foreach (var booking in confirmedBookings)
                {
                    var card = new BookingCard(booking);
                    // Make cards the same width as owner booking cards (1157px)
                    // Use the same calculation as owner booking cards - fill container width
                    int cardWidth = 1157; // Owner booking cards default width
                    if (confirmedBookingsPanel != null && confirmedBookingsPanel.Width > 100)
                    {
                        // Use container width minus padding (CARD_SPACING is padding on both sides = * 2)
                        cardWidth = confirmedBookingsPanel.Width - (CARD_SPACING * 2);
                    }
                    // Ensure reasonable width bounds (same as owner booking cards)
                    cardWidth = Math.Max(500, Math.Min(cardWidth, 1157));
                    card.Size = new Size(cardWidth, 86);
                    card.Margin = new Padding(CARD_SPACING / 2, 0, CARD_SPACING / 2, CARD_SPACING);
                    
                    // Wire up click event BEFORE adding to panel (important for proper event handling)
                    WireUpBookingCardClick(card);
                    
                    // Ensure card can receive mouse events BEFORE adding
                    card.Enabled = true;
                    card.Visible = true;
                    
                    // Add card to panel AFTER wiring and enabling
                    confirmedBookingsPanel.Controls.Add(card);
                    
                    // Force layout update
                    card.Invalidate();
                    card.Update();
                    
                    // Event wired and card added
                    System.Diagnostics.Debug.WriteLine($"Card {card.BookingData?.FullName} added to panel");
                }
                
                System.Diagnostics.Debug.WriteLine($"Loaded {confirmedBookings.Count} confirmed bookings");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading confirmed bookings: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error loading bookings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose custom resources
                qrScannerManager?.Dispose();
                scannerAnimationTimer?.Dispose();
                
                // Dispose designer components (components field is in Designer.cs partial class)
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private async void FrontBookingControl_VisibleChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"FrontBookingControl_VisibleChanged - Visible: {this.Visible}");
            
            // Refresh bookings when the control becomes visible
            if (this.Visible)
            {
                await LoadConfirmedBookingsAsync();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
