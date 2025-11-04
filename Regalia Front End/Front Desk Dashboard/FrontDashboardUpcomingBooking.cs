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

namespace Regalia_Front_End.Front_Desk_Dashboard
{
    public partial class FrontDashboardUpcomingBooking : UserControl
    {
        public BookingResponse BookingData { get; private set; }

        public FrontDashboardUpcomingBooking()
        {
            InitializeComponent();
        }

        public FrontDashboardUpcomingBooking(BookingResponse bookingData)
        {
            InitializeComponent();
            BookingData = bookingData ?? throw new ArgumentNullException(nameof(bookingData));
            LoadBookingData();
        }

        private void LoadBookingData()
        {
            if (BookingData == null) return;

            // Set guest name
            frontBookingName.Text = BookingData.FullName ?? "Guest";

            // Set unit name
            string unitName = BookingData.Condo?.Name ?? "Unit";
            if (!string.IsNullOrEmpty(BookingData.Condo?.Location))
            {
                unitName += $" - {BookingData.Condo.Location}";
            }
            frontUnitName.Text = unitName;

            // Set date (format: "MMM dd, yyyy")
            if (BookingData.StartDateTime != default(DateTime))
            {
                frontBookingDate.Text = BookingData.StartDateTime.ToString("MMM dd, yyyy");
            }
            else
            {
                frontBookingDate.Text = "N/A";
            }
        }
    }
}
