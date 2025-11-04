using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using Regalia_Front_End.Owner_Dashboard;
using Regalia_Front_End.Services;

namespace Regalia_Front_End
{
    public partial class Principal : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection();
        private PropertiesControl propertiesControl;
        private PropertiesUpdateControl propertiesUpdateControl;
        public DashBoardControl dashboardControl;
        private BookingControl bookingControl;
        private AnimationManager animationManager;
        public PropertyFormManager propertyFormManager;
        private UpdatePropertyFormManager updatePropertyFormManager;
        private ImageManager imageManager;
        private PropertyStatusCardManager statusCardManager;
        private BookingCardManager bookingCardManager;
        
        // Current booking being viewed (for approve/reject)
        private Models.BookingResponse currentBookingData;
        
        // Booking panel animation variables
        private System.Windows.Forms.Timer bookingPanelAnimationTimer;
        private bool isBookingPanelAnimating = false;
        private bool isBookingPanelClosing = false;
        private Point bookingPanelStartPosition;
        private Point bookingPanelEndPosition;
        private int bookingPanelAnimationStep = 0;
        private int bookingPanelTotalSteps = 30;
        
        public Principal()
        {
            InitializeComponent();
            this.Load += Principal_Load;
            InitializeDashboardControl();
            InitializePropertiesControl();
            InitializePropertiesUpdateControl();
            InitializeBookingControl();
        }

        private async void dashboardBtn_Click(object sender, EventArgs e)
        {
            // Show dashboard control and hide properties control
            dashboardControl.Visible = true;
            dashboardControl.BringToFront();
            propertiesControl.Visible = false;
            bookingControl.Visible = false;
            addPropertyBtn.Visible = false;
            
            // Refresh dashboard counts when dashboard is shown
            if (dashboardControl != null)
            {
                await dashboardControl.LoadDashboardCountsAsync();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void Principal_Load(object sender, EventArgs e)
        {
            string fontPath = Path.Combine(Application.StartupPath, "Fonts", "Roca Two Bold.ttf");
            pfc.AddFontFile(fontPath);
            label1.Font = new Font(pfc.Families[0], 36, FontStyle.Regular);
            
            // Show dashboard control when form loads
            dashboardControl.Visible = true;
            dashboardControl.BringToFront();
            
            // Load properties from API on startup
            if (propertyFormManager != null)
            {
                await propertyFormManager.LoadPropertiesFromApiAsync();
            }
            
            // Load dashboard counts after properties are loaded
            if (dashboardControl != null)
            {
                await dashboardControl.LoadDashboardCountsAsync();
            }
        }

        private void InitializeDashboardControl()
        {
            // Initialize dashboard control
            dashboardControl = new DashBoardControl();
            dashboardControl.Dock = DockStyle.Fill;
            dashboardControl.Visible = false;
            guna2Panel3.Controls.Add(dashboardControl);
        }

        private void InitializePropertiesControl()
        {
            // Initialize properties control
            propertiesControl = new PropertiesControl();
            propertiesControl.Dock = DockStyle.Fill;
            propertiesControl.Visible = false;
            guna2Panel3.Controls.Add(propertiesControl);
            // Add property forms directly to the main form for correct centering/animation
            this.Controls.Add(propertiesControl.addProperties1);
            this.Controls.Add(propertiesControl.addProperties2);
            this.Controls.Add(propertiesControl.addProperties3);
            this.Controls.Add(propertiesControl.addProperties4);
            propertiesControl.addProperties1.BringToFront();
            propertiesControl.addProperties2.BringToFront();
            propertiesControl.addProperties3.BringToFront();
            propertiesControl.addProperties4.BringToFront();
            
            // Initialize animation manager for property forms
            animationManager = new AnimationManager(this, propertiesControl.addProperties1, propertiesControl.addProperties2);
            animationManager.SetProperties3Panel(propertiesControl.addProperties3);
            
            // Initialize image manager
            imageManager = new ImageManager(propertiesControl);
            
            // Initialize property status card manager for dashboard
            statusCardManager = null;
            if (dashboardControl != null && dashboardControl.propertyStatusOver != null && dashboardControl.totalProperty != null)
            {
                statusCardManager = new PropertyStatusCardManager(dashboardControl.propertyStatusOver, dashboardControl.totalProperty);
            }
            
            // Initialize property form manager
            propertyFormManager = new PropertyFormManager(this, propertiesControl, imageManager, statusCardManager);
            
            // Wire up button events
            propertiesBtn.Click += PropertiesBtn_Click;
            addPropertyBtn.Click += AddPropertyBtn_Click;
            
            // Wire up logout button with multiple events to ensure it works
            logOutBtn.Click -= logOutBtn_Click;
            logOutBtn.Click += logOutBtn_Click;
            MouseEventHandler principalMouseClickHandler = (s, e) => { System.Diagnostics.Debug.WriteLine("Logout button MouseClick fired (Principal)"); logOutBtn_Click(s, e); };
            MouseEventHandler principalMouseDownHandler = (s, e) => { System.Diagnostics.Debug.WriteLine("Logout button MouseDown fired (Principal)"); };
            logOutBtn.MouseClick += principalMouseClickHandler;
            logOutBtn.MouseDown += principalMouseDownHandler;
            
            // Ensure button is enabled and visible
            logOutBtn.Enabled = true;
            logOutBtn.Visible = true;
            logOutBtn.BringToFront();
            
            // Wire up property form navigation events
            propertiesControl.propertiesNext1.Click += Properties1Next_Click; // Next button in addProperties1
            propertiesControl.propertiesNext2.Click += Properties2Next_Click; // Next button in addProperties2
            propertiesControl.propertiesBack2.Click += Properties2Back_Click; // Back button in addProperties2
            propertiesControl.propertiesBack3.Click += Properties3Back_Click; // Back button in addProperties3
            propertiesControl.propertiesNext3.Click += Properties3Next_Click; // Next button in addProperties3 (navigates to page 4)
            propertiesControl.propertiesBack4.Click += Properties4Back_Click; // Back button in addProperties4
            // Note: submitLoginBtn is shared between page 3 and page 4 - will handle dynamically
            propertiesControl.submitLoginBtn.Click += SubmitLoginBtn_Click; // Submit button (page 3: goes to page 4, page 4: submits front desk)
            
            // Wire up close button events
            propertiesControl.propertiesClose1.Click += PropertiesClose1_Click; // Close button in addProperties1
            propertiesControl.propertiesClose2.Click += PropertiesClose2_Click; // Close button in addProperties2
            propertiesControl.propertiesClose3.Click += PropertiesClose3_Click; // Close button in addProperties3
            propertiesControl.propertiesClose4.Click += PropertiesClose4_Click; // Close button in addProperties4
            
            // Wire up property card click events
            propertyFormManager.cardManager.OnPropertyCardClicked += PropertyCard_Clicked;
        }

        private void InitializePropertiesUpdateControl()
        {
            // Initialize update control (hidden, used for updating existing properties)
            propertiesUpdateControl = new PropertiesUpdateControl();
            propertiesUpdateControl.Dock = DockStyle.Fill;
            propertiesUpdateControl.Visible = false;
            guna2Panel3.Controls.Add(propertiesUpdateControl);
            
            // Add update property forms directly to the main form for correct centering/animation
            this.Controls.Add(propertiesUpdateControl.updPropertiesFirst);
            this.Controls.Add(propertiesUpdateControl.updProperties1);
            this.Controls.Add(propertiesUpdateControl.updProperties2);
            this.Controls.Add(propertiesUpdateControl.updProperties3);
            this.Controls.Add(propertiesUpdateControl.updProperties4);
            propertiesUpdateControl.updPropertiesFirst.BringToFront();
            propertiesUpdateControl.updProperties1.BringToFront();
            propertiesUpdateControl.updProperties2.BringToFront();
            propertiesUpdateControl.updProperties3.BringToFront();
            propertiesUpdateControl.updProperties4.BringToFront();
            
            // Initialize update property form manager
            updatePropertyFormManager = new UpdatePropertyFormManager(this, propertiesUpdateControl, propertyFormManager.cardManager, statusCardManager);
            
            // Wire up update form navigation events
            propertiesUpdateControl.updatePropertiesNext1.Click += UpdateProperties1Next_Click;
            propertiesUpdateControl.updatePropertiesBack1.Click += UpdateProperties1Back_Click;
            propertiesUpdateControl.updatePropertiesNext2.Click += UpdateProperties2Next_Click;
            propertiesUpdateControl.updatePropertiesBack2.Click += UpdateProperties2Back_Click;
            propertiesUpdateControl.updatePropertiesBack3.Click += UpdateProperties3Back_Click;
            propertiesUpdateControl.updatePropertiesNext3.Click += UpdateProperties3Next_Click;
            propertiesUpdateControl.updatePropertiesBack4.Click += UpdateProperties4Back_Click;
            propertiesUpdateControl.updateSubmitLoginBtn.Click += UpdateSubmitLoginBtn_Click; // Update button
            
            // Wire up update form close button events
            propertiesUpdateControl.updatePropertiesClose1.Click += UpdatePropertiesClose1_Click;
            propertiesUpdateControl.updatePropertiesClose2.Click += UpdatePropertiesClose2_Click;
            propertiesUpdateControl.updatePropertiesClose3.Click += UpdatePropertiesClose3_Click;
            propertiesUpdateControl.updatePropertiesClose4.Click += UpdatePropertiesClose4_Click;
            
            // Wire up updPropertiesFirst buttons
            propertiesUpdateControl.nextProperties10.Click += NextProperties10_Click;
            propertiesUpdateControl.delBtnProperties.Click += DelBtnProperties_Click;
            propertiesUpdateControl.guna2CircleButton1.Click += UpdPropertiesFirstClose_Click;
            propertiesUpdateControl.copyClip.Click += CopyClip_Click;
            
            // Initialize combo box for update form
            InitializeUpdateComboBox();
            
            // Note: availabilityCmb changes are NOT wired - status only updates when Update button is clicked
        }

        private void InitializeBookingControl()
        {
            // Initialize booking control
            bookingControl = new BookingControl();
            bookingControl.Dock = DockStyle.Fill;
            bookingControl.Visible = false;
            guna2Panel3.Controls.Add(bookingControl);
            
            // Initialize booking card manager
            bookingCardManager = new BookingCardManager(bookingControl);
            
            // Wire up booking card click events
            bookingCardManager.OnBookingCardClicked += BookingCard_Clicked;
            
            // Add bookingInformationPnl directly to the main form for correct centering/animation
            // (similar to how addProperties forms are added)
            this.Controls.Add(bookingControl.bookingInformationPnl);
            bookingControl.bookingInformationPnl.BringToFront();
            bookingControl.bookingInformationPnl.Visible = false; // Hidden initially
            
            // Wire up booking button event
            bookingBtn.Click += BookingBtn_Click;
            
            // Wire up booking close button (bookingCls) - closes the detail panel
            if (bookingControl.bookingCls != null)
            {
                bookingControl.bookingCls.Click += BookingCls_Click;
            }
            
            // Wire up confirm and reject booking buttons
            if (bookingControl.confirmBooking != null)
            {
                bookingControl.confirmBooking.Click += ConfirmBooking_Click;
            }
            if (bookingControl.rejectBooking != null)
            {
                bookingControl.rejectBooking.Click += RejectBooking_Click;
            }
        }
        
        private void BookingCls_Click(object sender, EventArgs e)
        {
            // Close the booking detail panel with animation
            HideBookingPanelWithAnimation();
        }

        private void BookingCard_Clicked(object sender, Models.BookingResponse bookingData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"BookingCard_Clicked in Principal - Booking: {bookingData?.FullName}");
                // Show booking detail panel with animation
                ShowBookingDetailPanel(bookingData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in BookingCard_Clicked: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error opening booking details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowBookingDetailPanel(Models.BookingResponse bookingData)
        {
            // Store current booking data for approve/reject
            currentBookingData = bookingData;
            
            // Populate booking information panel with data
            if (bookingControl != null && bookingData != null)
            {
                // Populate labels
                bookingControl.ownerDashName.Text = bookingData.FullName ?? "N/A";
                bookingControl.ownerDashEmail.Text = bookingData.Email ?? "N/A";
                bookingControl.ownerDashContactNumber.Text = bookingData.Contact ?? "N/A";
                bookingControl.ownerDashNumOfGuests.Text = bookingData.GuestCount.ToString();
                bookingControl.ownerDashTitleUnitNum.Text = bookingData.Condo?.Name ?? "Unit";
                
                if (bookingData.StartDateTime != default(DateTime))
                {
                    bookingControl.ownerDashCheckIn.Text = bookingData.StartDateTime.ToString("MMM dd, yyyy");
                }
                if (bookingData.EndDateTime != default(DateTime))
                {
                    bookingControl.ownerDashCheckOut.Text = bookingData.EndDateTime.ToString("MMM dd, yyyy");
                }
                
                bookingControl.ownerDashSpecialRequests.Text = bookingData.Notes ?? "None";
                
                // Clear payment image first
                bookingControl.paymentProof.Image = null;
                
                // Load payment image if available
                if (!string.IsNullOrEmpty(bookingData.PaymentImageUrl))
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"Loading payment image. URL length: {bookingData.PaymentImageUrl.Length}");
                        System.Diagnostics.Debug.WriteLine($"PaymentImageUrl starts with: {bookingData.PaymentImageUrl.Substring(0, Math.Min(50, bookingData.PaymentImageUrl.Length))}...");
                        
                        if (bookingData.PaymentImageUrl.StartsWith("data:image/"))
                        {
                            // Base64 image
                            var image = Helpers.ImageBase64Helper.ConvertBase64ToImage(bookingData.PaymentImageUrl);
                            if (image != null)
                            {
                                bookingControl.paymentProof.Image = image;
                                bookingControl.paymentProof.SizeMode = PictureBoxSizeMode.Zoom;
                                System.Diagnostics.Debug.WriteLine("Payment image loaded successfully");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Failed to convert base64 to image");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"PaymentImageUrl doesn't start with 'data:image/' - format may be incorrect");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading payment image: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("PaymentImageUrl is null or empty");
                }
                
                // Show panel with animation
                ShowBookingPanelWithAnimation();
            }
        }
        
        private async void ConfirmBooking_Click(object sender, EventArgs e)
        {
            if (currentBookingData == null)
            {
                return;
            }
            
            // Confirm with user
            var result = MessageBox.Show(
                $"Are you sure you want to approve the booking for {currentBookingData.FullName}?",
                "Confirm Approval",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            
            if (result != DialogResult.Yes)
                return;
            
            try
            {
                // Disable button while processing
                bookingControl.confirmBooking.Enabled = false;
                bookingControl.rejectBooking.Enabled = false;
                
                // Store booking ID before API call
                int bookingId = currentBookingData.Id;
                
                // Call API to approve booking
                var apiService = new Services.ApiService();
                bool success = await apiService.ApproveBookingAsync(bookingId, true);
                
                if (success)
                {
                    // Close the panel
                    HideBookingPanelWithAnimation();
                    
                    // Remove the booking card from the list (don't refresh - just remove it)
                    bookingCardManager?.RemoveBookingCardById(bookingId);
                    
                    // Clear current booking data
                    currentBookingData = null;
                }
                else
                {
                    MessageBox.Show(
                        "Failed to approve booking. Please try again.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error approving booking: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                // Re-enable buttons
                bookingControl.confirmBooking.Enabled = true;
                bookingControl.rejectBooking.Enabled = true;
            }
        }
        
        private async void RejectBooking_Click(object sender, EventArgs e)
        {
            if (currentBookingData == null)
            {
                return;
            }
            
            // Ask for rejection reason
            var reasonForm = new Form
            {
                Text = "Reject Booking",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            
            var reasonLabel = new Label
            {
                Text = "Rejection Reason:",
                Location = new Point(10, 15),
                AutoSize = true
            };
            
            var reasonTextBox = new TextBox
            {
                Location = new Point(10, 35),
                Size = new Size(360, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            
            var okButton = new Button
            {
                Text = "Reject",
                DialogResult = DialogResult.OK,
                Location = new Point(205, 120),
                Size = new Size(80, 30)
            };
            
            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(290, 120),
                Size = new Size(80, 30)
            };
            
            reasonForm.Controls.Add(reasonLabel);
            reasonForm.Controls.Add(reasonTextBox);
            reasonForm.Controls.Add(okButton);
            reasonForm.Controls.Add(cancelButton);
            reasonForm.AcceptButton = okButton;
            reasonForm.CancelButton = cancelButton;
            
            if (reasonForm.ShowDialog(this) != DialogResult.OK)
                return;
            
            try
            {
                // Disable button while processing
                bookingControl.confirmBooking.Enabled = false;
                bookingControl.rejectBooking.Enabled = false;
                
                // Store booking ID before API call
                int bookingId = currentBookingData.Id;
                
                // Call API to reject booking
                var apiService = new Services.ApiService();
                bool success = await apiService.ApproveBookingAsync(bookingId, false, reasonTextBox.Text);
                
                if (success)
                {
                    // Close the panel
                    HideBookingPanelWithAnimation();
                    
                    // Remove the booking card from the list (don't refresh - just remove it)
                    bookingCardManager?.RemoveBookingCardById(bookingId);
                    
                    // Clear current booking data
                    currentBookingData = null;
                }
                else
                {
                    MessageBox.Show(
                        "Failed to reject booking. Please try again.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error rejecting booking: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                // Re-enable buttons
                bookingControl.confirmBooking.Enabled = true;
                bookingControl.rejectBooking.Enabled = true;
            }
        }
        
        private void ShowBookingPanelWithAnimation()
        {
            if (isBookingPanelAnimating) return;
            
            var panel = bookingControl.bookingInformationPnl;
            isBookingPanelAnimating = true;
            isBookingPanelClosing = false;
            
            // Calculate positions
            int centerX = (this.ClientSize.Width - panel.Width) / 2;
            int centerY = (this.ClientSize.Height - panel.Height) / 2;
            bookingPanelEndPosition = new Point(centerX, centerY);
            bookingPanelStartPosition = new Point(centerX, this.ClientSize.Height + 50);
            
            // Set initial position and make visible
            panel.Location = bookingPanelStartPosition;
            panel.Visible = true;
            panel.BringToFront();
            
            // Start animation
            StartBookingPanelAnimation();
        }
        
        private void HideBookingPanelWithAnimation()
        {
            if (isBookingPanelAnimating) return;
            
            var panel = bookingControl.bookingInformationPnl;
            isBookingPanelAnimating = true;
            isBookingPanelClosing = true;
            
            // Calculate positions
            bookingPanelStartPosition = panel.Location;
            bookingPanelEndPosition = new Point(bookingPanelStartPosition.X, this.ClientSize.Height + 50);
            
            // Start animation
            StartBookingPanelAnimation();
        }
        
        private void StartBookingPanelAnimation()
        {
            if (bookingPanelAnimationTimer != null)
            {
                bookingPanelAnimationTimer.Stop();
                bookingPanelAnimationTimer.Dispose();
            }
            
            bookingPanelAnimationTimer = new System.Windows.Forms.Timer();
            bookingPanelAnimationTimer.Interval = 16; // ~60fps
            bookingPanelAnimationTimer.Tick += BookingPanelAnimationTimer_Tick;
            bookingPanelAnimationStep = 0;
            bookingPanelTotalSteps = isBookingPanelClosing ? 20 : 30; // Faster closing
            bookingPanelAnimationTimer.Start();
        }
        
        private void BookingPanelAnimationTimer_Tick(object sender, EventArgs e)
        {
            bookingPanelAnimationStep++;
            double progress = (double)bookingPanelAnimationStep / bookingPanelTotalSteps;
            
            // Apply easing
            if (isBookingPanelClosing)
            {
                progress = progress * progress; // Ease-in for closing
            }
            else
            {
                progress = 1 - Math.Pow(1 - progress, 3); // Ease-out for opening
            }
            
            // Calculate current position
            int currentX = bookingPanelStartPosition.X + (int)((bookingPanelEndPosition.X - bookingPanelStartPosition.X) * progress);
            int currentY = bookingPanelStartPosition.Y + (int)((bookingPanelEndPosition.Y - bookingPanelStartPosition.Y) * progress);
            
            var panel = bookingControl.bookingInformationPnl;
            panel.Location = new Point(currentX, currentY);
            
            if (bookingPanelAnimationStep >= bookingPanelTotalSteps)
            {
                bookingPanelAnimationTimer.Stop();
                bookingPanelAnimationTimer.Dispose();
                bookingPanelAnimationTimer = null;
                
                if (isBookingPanelClosing)
                {
                    panel.Visible = false;
                    // Reset position for next opening
                    int centerX = (this.ClientSize.Width - panel.Width) / 2;
                    int centerY = (this.ClientSize.Height - panel.Height) / 2;
                    panel.Location = new Point(centerX, centerY);
                }
                
                isBookingPanelAnimating = false;
                isBookingPanelClosing = false;
            }
        }

        private async void BookingBtn_Click(object sender, EventArgs e)
        {
            // Show booking control and hide other controls
            bookingControl.Visible = true;
            bookingControl.BringToFront();
            dashboardControl.Visible = false;
            propertiesControl.Visible = false;
            addPropertyBtn.Visible = false;
            
            // Load bookings from API
            await LoadBookingsFromApiAsync();
        }

        private async System.Threading.Tasks.Task LoadBookingsFromApiAsync()
        {
            try
            {
                var apiService = new Services.ApiService();
                var bookings = await apiService.GetOwnerBookingsAsync();
                
                // Clear existing cards
                bookingCardManager.ClearAllCards();
                
                // Only show PendingApproval bookings in owner's booking page
                // Approved, CheckedIn, CheckedOut bookings should only appear in front desk
                var pendingBookings = bookings
                    .Where(b => b.Status == "PendingApproval" || b.Status == "Rejected")
                    .OrderByDescending(b => b.CreatedAt)
                    .ToList();
                
                // Add cards for each pending booking
                foreach (var booking in pendingBookings)
                {
                    bookingCardManager.AddBookingCard(booking);
                }
                
                System.Diagnostics.Debug.WriteLine($"Loaded {pendingBookings.Count} pending bookings from API (excluded {bookings.Count - pendingBookings.Count} approved/checked-in/checked-out bookings)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading bookings: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error loading bookings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeUpdateComboBox()
        {
            // Add default property types to update combo box
            propertiesUpdateControl.updTypeCmb.Items.Clear();
            propertiesUpdateControl.updTypeCmb.Items.Add("Studio Type");
            propertiesUpdateControl.updTypeCmb.Items.Add("1 Bedroom");
            propertiesUpdateControl.updTypeCmb.Items.Add("2 Bedroom");
            propertiesUpdateControl.updTypeCmb.Items.Add("3 Bedroom");
            propertiesUpdateControl.updTypeCmb.Items.Add("Penthouse");
            propertiesUpdateControl.updTypeCmb.Items.Add("Duplex");
            
            // Initialize availability combo box with status options
            propertiesUpdateControl.availabilityCmb.Items.Clear();
            propertiesUpdateControl.availabilityCmb.Items.Add("Available");
            propertiesUpdateControl.availabilityCmb.Items.Add("Maintenance");
            propertiesUpdateControl.availabilityCmb.Items.Add("Occupied");
        }

        private void PropertyCard_Clicked(object sender, PropertyData propertyData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"PropertyCard_Clicked in Principal - Property: {propertyData?.Title}");
                // Find the card that was clicked
                if (sender is PropertyCard clickedCard)
                {
                    System.Diagnostics.Debug.WriteLine($"Opening update form for card: {clickedCard.PropertyData?.Title}");
                    // Show update form with this property's data
                    updatePropertyFormManager.ShowUpdatePropertyForm(clickedCard);
                    System.Diagnostics.Debug.WriteLine("Update form shown successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Sender is not PropertyCard, it's: {sender?.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in PropertyCard_Clicked: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error opening update form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #region Properties Button Events

        private void PropertiesBtn_Click(object sender, EventArgs e)
        {
            // Show properties control and hide dashboard control
            propertiesControl.Visible = true;
            propertiesControl.BringToFront();
            addPropertyBtn.Visible = true;
            dashboardControl.Visible = false;
            bookingControl.Visible = false;
            
            // Load properties from API when Properties tab is clicked
            propertyFormManager.LoadPropertiesFromApi();
            
            // Ensure containerPanel is removed and cardContainer is brought to front
            // so property cards can be clicked
            if (propertyFormManager?.cardManager != null)
            {
                propertyFormManager.cardManager.BringCardContainerToFront();
            }
        }

        private void AddPropertyBtn_Click(object sender, EventArgs e)
        {
            // Animate and show Add Property step 1 over the gradient
            propertyFormManager.ShowAddPropertyForm();
        }

        #endregion

        #region Property Form Navigation Events

        private void Properties1Next_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties2();
        }

        private void Properties2Next_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties3();
        }

        private void Properties2Back_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties1();
        }

        private void Properties3Back_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties2();
        }

        private void Properties3Next_Click(object sender, EventArgs e)
        {
            // Navigate to page 4 without submitting property
            // Property will be created when submitLoginBtn is clicked on page 4
            propertyFormManager.ShowProperties4();
        }

        private void SubmitLoginBtn_Click(object sender, EventArgs e)
        {
            // submitLoginBtn is only on page 4
            // When clicked, it should submit both the property AND the front desk account
            propertyFormManager.SubmitProperty(navigateToPage4: false); // Submit property (adds card) without navigating
            propertyFormManager.SubmitFrontDeskAccount(); // Submit front desk account and close
        }

        private void Properties4Back_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties3();
        }

        private void PropertiesClose1_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties1);
        }

        private void PropertiesClose2_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties2);
        }

        private void PropertiesClose3_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties3);
        }

        private void PropertiesClose4_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties4);
        }

        #endregion

        #region Update Property Form Navigation Events

        private void UpdateProperties1Next_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties2();
        }

        private void UpdateProperties1Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdatePropertiesFirst();
        }

        private void UpdateProperties2Next_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties3();
        }

        private void UpdateProperties2Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties1();
        }

        private void UpdateProperties3Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties2();
        }

        private void UpdateProperties3Next_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties4();
        }

        private void UpdateProperties4Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties3();
        }

        private void UpdateSubmitLoginBtn_Click(object sender, EventArgs e)
        {
            // Update button - saves changes to property
            updatePropertyFormManager.UpdateProperty();
        }

        private void UpdatePropertiesClose1_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties1);
        }

        private void UpdatePropertiesClose2_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties2);
        }

        private void UpdatePropertiesClose3_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties3);
        }

        private void UpdatePropertiesClose4_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties4);
        }

        private void NextProperties10_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties1();
        }

        private async void DelBtnProperties_Click(object sender, EventArgs e)
        {
            try
            {
                await updatePropertyFormManager.DeletePropertyAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in DelBtnProperties_Click: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error deleting property: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdPropertiesFirstClose_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.HideFormWithAnimation(propertiesUpdateControl.updPropertiesFirst);
        }

        private void CopyClip_Click(object sender, EventArgs e)
        {
            try
            {
                string bookingLink = propertiesUpdateControl.txtBookingLink.Text;
                
                if (string.IsNullOrWhiteSpace(bookingLink))
                {
                    MessageBox.Show("No booking link available to copy.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // Copy to clipboard
                Clipboard.SetText(bookingLink);
                
                // Show confirmation
                MessageBox.Show("Booking link copied to clipboard!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying link: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void logOutBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Logout button clicked in Principal");
            
            try
            {
                // Log out the user
                ApiService.Logout();
                System.Diagnostics.Debug.WriteLine("User logged out successfully");
                
                // Close this dialog form - this will return control to LoginForm
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during logout: {ex.Message}");
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
