using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Regalia_Front_End.Services;

namespace Regalia_Front_End
{
    public partial class DashBoardControl : UserControl
    {
        public DashBoardControl()
        {
            InitializeComponent();
        }

        private void guna2Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// Loads and updates all dashboard counts (bookings, available, occupied, total properties)
        /// </summary>
        public async Task LoadDashboardCountsAsync()
        {
            try
            {
                using (var apiService = new ApiService())
                {
                    // Load bookings and properties in parallel
                    var bookingsTask = apiService.GetOwnerBookingsAsync();
                    var condosTask = apiService.GetOwnerCondosAsync();

                    await Task.WhenAll(bookingsTask, condosTask);

                    var bookings = await bookingsTask;
                    var condos = await condosTask;

                    // Update counts on UI thread
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => UpdateCounts(bookings, condos)));
                    }
                    else
                    {
                        UpdateCounts(bookings, condos);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading dashboard counts: {ex.Message}");
                // Set counts to 0 on error
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => ResetCounts()));
                }
                else
                {
                    ResetCounts();
                }
            }
        }

        /// <summary>
        /// Updates the count labels based on bookings and condos data
        /// </summary>
        private void UpdateCounts(List<Models.BookingResponse> bookings, List<Models.CondoResponse> condos)
        {
            try
            {
                // Total Bookings - count only pending bookings (Approved/CheckedIn/CheckedOut are handled by front desk)
                int totalBookingsCount = bookings?.Where(b => b.Status == "PendingApproval" || b.Status == "Rejected").Count() ?? 0;
                if (totalBooking != null)
                {
                    totalBooking.Text = totalBookingsCount.ToString();
                }

                // Total Available - count properties with Status == "Available"
                int availableCount = condos?.Count(c => c.Status != null && 
                    c.Status.Equals("Available", StringComparison.OrdinalIgnoreCase)) ?? 0;
                if (totalAvailable != null)
                {
                    totalAvailable.Text = availableCount.ToString();
                }

                // Total Occupied - count properties with Status == "Occupied"
                int occupiedCount = condos?.Count(c => c.Status != null && 
                    c.Status.Equals("Occupied", StringComparison.OrdinalIgnoreCase)) ?? 0;
                if (totalOccupied != null)
                {
                    totalOccupied.Text = occupiedCount.ToString();
                }

                // Total Properties - count all properties
                int totalPropertiesCount = condos?.Count ?? 0;
                if (totalProperty != null)
                {
                    totalProperty.Text = totalPropertiesCount.ToString();
                }

                System.Diagnostics.Debug.WriteLine($"Dashboard counts updated - Bookings: {totalBookingsCount}, Available: {availableCount}, Occupied: {occupiedCount}, Total: {totalPropertiesCount}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating counts: {ex.Message}");
            }
        }

        /// <summary>
        /// Resets all counts to 0
        /// </summary>
        private void ResetCounts()
        {
            if (totalBooking != null) totalBooking.Text = "0";
            if (totalAvailable != null) totalAvailable.Text = "0";
            if (totalOccupied != null) totalOccupied.Text = "0";
            if (totalProperty != null) totalProperty.Text = "0";
        }
    }
}
