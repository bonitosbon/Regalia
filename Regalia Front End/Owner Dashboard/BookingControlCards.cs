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

namespace Regalia_Front_End.Owner_Dashboard
{
    public partial class BookingControlCards : UserControl
    {
        public BookingResponse BookingData { get; private set; }
        public event EventHandler<BookingResponse> OnCardClicked;

        public BookingControlCards(BookingResponse bookingData)
        {
            InitializeComponent();
            BookingData = bookingData ?? throw new ArgumentNullException(nameof(bookingData));
            LoadBookingData();
            
            // Wire up click events
            this.Click += BookingControlCards_Click;
            this.Cursor = Cursors.Hand;
            
            // Wire up child controls
            foreach (Control control in this.Controls)
            {
                control.Click += BookingControlCards_Click;
                control.Cursor = Cursors.Hand;
            }
        }

        private void LoadBookingData()
        {
            if (BookingData == null) return;

            // Set guest name
            bookingName.Text = BookingData.FullName ?? "Guest";

            // Set unit name
            string unitName = BookingData.Condo?.Name ?? "Unit";
            if (!string.IsNullOrEmpty(BookingData.Condo?.Location))
            {
                unitName += $" - {BookingData.Condo.Location}";
            }
            bookingUnitName.Text = unitName;

            // Set date (format: "Nov 20, 2023")
            if (BookingData.StartDateTime != default(DateTime))
            {
                bookingDate.Text = BookingData.StartDateTime.ToString("MMM dd, yyyy");
            }
            else
            {
                bookingDate.Text = "N/A";
            }

            // Set status with color
            SetStatus(BookingData.Status ?? "Pending");
        }

        private void SetStatus(string status)
        {
            // Format status text: add spaces before capital letters (e.g., "PendingApproval" -> "Pending Approval")
            string formattedStatus = System.Text.RegularExpressions.Regex.Replace(status, "([a-z])([A-Z])", "$1 $2");
            statusLabel.Text = formattedStatus;
            
            // Set color based on status (use original status for comparison)
            Color dotColor;
            string statusLower = status.ToLower();
            switch (statusLower)
            {
                case "confirmed":
                case "approved":
                    dotColor = Color.FromArgb(76, 175, 80); // Green
                    statusLabel.ForeColor = Color.FromArgb(76, 175, 80);
                    break;
                case "pending":
                case "pendingapproval":
                    dotColor = Color.FromArgb(158, 158, 158); // Gray
                    statusLabel.ForeColor = Color.FromArgb(158, 158, 158);
                    break;
                case "cancelled":
                case "rejected":
                    dotColor = Color.FromArgb(244, 67, 54); // Red
                    statusLabel.ForeColor = Color.FromArgb(244, 67, 54);
                    break;
                default:
                    dotColor = Color.FromArgb(158, 158, 158); // Gray
                    statusLabel.ForeColor = Color.FromArgb(158, 158, 158);
                    break;
            }
            
            statusDot.ForeColor = dotColor;
        }

        private void BookingControlCards_Click(object sender, EventArgs e)
        {
            OnCardClicked?.Invoke(this, BookingData);
        }
    }
}
