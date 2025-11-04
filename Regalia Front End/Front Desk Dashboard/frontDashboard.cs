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
    public partial class frontDashboard : UserControl
    {
        private const int CARD_SPACING = 15;
        private FlowLayoutPanel upcomingBookingsPanel;
        private FlowLayoutPanel departureBookingsPanel;

        public frontDashboard()
        {
            InitializeComponent();
            InitializeUpcomingBookingsPanel();
            InitializeDepartureBookingsPanel();
            Load += FrontDashboard_Load;
            VisibleChanged += FrontDashboard_VisibleChanged;
        }

        private void InitializeUpcomingBookingsPanel()
        {
            // Create FlowLayoutPanel for upcoming bookings
            upcomingBookingsPanel = new FlowLayoutPanel();
            upcomingBookingsPanel.AutoScroll = true;
            upcomingBookingsPanel.FlowDirection = FlowDirection.TopDown;
            upcomingBookingsPanel.WrapContents = false;
            upcomingBookingsPanel.Padding = new Padding(15);
            upcomingBookingsPanel.BackColor = Color.Transparent;
            
            // Position it below the "Upcoming Arrival" label (label6 is at y=13, height ~32, so start at ~60)
            upcomingBookingsPanel.Location = new Point(0, 60);
            upcomingBookingsPanel.Size = new Size(upcoming.Width - 30, upcoming.Height - 60);
            upcomingBookingsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            
            // Add to the upcoming panel (Upcoming Arrival panel)
            upcoming.Controls.Add(upcomingBookingsPanel);
            upcomingBookingsPanel.BringToFront();
        }

        private void InitializeDepartureBookingsPanel()
        {
            // Create FlowLayoutPanel for departure bookings (CheckedIn)
            departureBookingsPanel = new FlowLayoutPanel();
            departureBookingsPanel.AutoScroll = true;
            departureBookingsPanel.FlowDirection = FlowDirection.TopDown;
            departureBookingsPanel.WrapContents = false;
            departureBookingsPanel.Padding = new Padding(15);
            departureBookingsPanel.BackColor = Color.Transparent;
            
            // Position it below the "Upcoming Departure" label (label1 is at y=13, height ~32, so start at ~60)
            departureBookingsPanel.Location = new Point(0, 60);
            departureBookingsPanel.Size = new Size(departure.Width - 30, departure.Height - 60);
            departureBookingsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            
            // Add to the departure panel
            departure.Controls.Add(departureBookingsPanel);
            departureBookingsPanel.BringToFront();
        }

        private async void FrontDashboard_Load(object sender, EventArgs e)
        {
            await LoadUpcomingBookingsAsync();
        }

        public async Task LoadUpcomingBookingsAsync()
        {
            try
            {
                var apiService = new ApiService();
                var bookings = await apiService.GetFrontDeskBookingsAsync();
                
                // Clear existing cards
                upcomingBookingsPanel.Controls.Clear();
                if (departureBookingsPanel != null)
                {
                    departureBookingsPanel.Controls.Clear();
                }
                
                // Filter for approved bookings that haven't started yet (upcoming)
                var upcomingBookings = bookings
                    .Where(b => b.Status == "Approved" && b.StartDateTime > DateTime.UtcNow)
                    .OrderBy(b => b.StartDateTime)
                    .ToList();
                
                // Filter for checked-in bookings (departure) - exclude CheckedOut
                var departureBookings = bookings
                    .Where(b => b.Status == "CheckedIn")
                    .Where(b => b.Status != "CheckedOut")
                    .OrderBy(b => b.EndDateTime)
                    .ToList();
                
                // Add cards for each upcoming booking
                foreach (var booking in upcomingBookings)
                {
                    var card = new FrontDashboardUpcomingBooking(booking);
                    // Make cards the same width as owner booking cards (1157px)
                    // Use the same calculation as owner booking cards - fill container width
                    int cardWidth = 1157; // Owner booking cards default width
                    if (upcomingBookingsPanel != null && upcomingBookingsPanel.Width > 100)
                    {
                        // Use container width minus padding (CARD_SPACING is padding on both sides = * 2)
                        cardWidth = upcomingBookingsPanel.Width - (CARD_SPACING * 2);
                    }
                    // Ensure reasonable width bounds (same as owner booking cards)
                    cardWidth = Math.Max(500, Math.Min(cardWidth, 1157));
                    card.Size = new Size(cardWidth, 95);
                    card.Margin = new Padding(CARD_SPACING / 2, 0, CARD_SPACING / 2, CARD_SPACING);
                    upcomingBookingsPanel.Controls.Add(card);
                }
                
                // Add cards for each departure booking (CheckedIn)
                foreach (var booking in departureBookings)
                {
                    var card = new FrontDashboardUpcomingBooking(booking);
                    int cardWidth = 1157;
                    if (departureBookingsPanel != null && departureBookingsPanel.Width > 100)
                    {
                        cardWidth = departureBookingsPanel.Width - (CARD_SPACING * 2);
                    }
                    cardWidth = Math.Max(500, Math.Min(cardWidth, 1157));
                    card.Size = new Size(cardWidth, 95);
                    card.Margin = new Padding(CARD_SPACING / 2, 0, CARD_SPACING / 2, CARD_SPACING);
                    departureBookingsPanel.Controls.Add(card);
                }
                
                System.Diagnostics.Debug.WriteLine($"Loaded {upcomingBookings.Count} upcoming bookings and {departureBookings.Count} departure bookings");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading upcoming bookings: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void RemoveBookingFromDeparture(int bookingId)
        {
            // Remove booking card from departure panel
            if (departureBookingsPanel != null)
            {
                foreach (Control control in departureBookingsPanel.Controls)
                {
                    if (control is FrontDashboardUpcomingBooking card && card.BookingData?.Id == bookingId)
                    {
                        departureBookingsPanel.Controls.Remove(card);
                        card.Dispose();
                        System.Diagnostics.Debug.WriteLine($"Removed booking card from departure for booking ID: {bookingId}");
                        break;
                    }
                }
            }
        }

        private async void FrontDashboard_VisibleChanged(object sender, EventArgs e)
        {
            // Refresh bookings when the dashboard becomes visible
            if (this.Visible)
            {
                await LoadUpcomingBookingsAsync();
            }
        }
    }
}
