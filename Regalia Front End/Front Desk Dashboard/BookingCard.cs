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
    public partial class BookingCard : UserControl
    {
        public BookingResponse BookingData { get; private set; }
        public event EventHandler<BookingResponse> OnCardClicked;
        private bool isMouseDown = false;

        public BookingCard()
        {
            InitializeComponent();
            WireUpClickEvents();
        }

        public BookingCard(BookingResponse bookingData)
        {
            InitializeComponent();
            BookingData = bookingData ?? throw new ArgumentNullException(nameof(bookingData));
            LoadBookingData();
            
            // Wire up events after component initialization
            this.Load += (s, e) => WireUpClickEvents();
            WireUpClickEvents();
        }

        private void WireUpClickEvents()
        {
            System.Diagnostics.Debug.WriteLine($"WireUpClickEvents called for BookingCard: {BookingData?.FullName}");
            
            // Remove any existing handlers to avoid duplicates
            this.Click -= BookingCard_Click;
            this.MouseDown -= BookingCard_MouseDown;
            this.MouseUp -= BookingCard_MouseUp;
            
            // Wire up click events for the card itself
            this.Click += BookingCard_Click;
            this.MouseClick += BookingCard_MouseClick;
            this.Cursor = Cursors.Hand;
            this.MouseDown += BookingCard_MouseDown;
            this.MouseUp += BookingCard_MouseUp;
            
            // Wire up child controls (if they exist)
            WireUpChildControls();
            
            System.Diagnostics.Debug.WriteLine($"WireUpClickEvents completed - Controls count: {this.Controls.Count}");
        }
        
        private void BookingCard_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"BookingCard_MouseClick triggered!");
            BookingCard_Click(sender, EventArgs.Empty);
        }
        
        private void WireUpChildControls()
        {
            int controlsWired = 0;
            // Wire up child controls
            foreach (Control control in this.Controls)
            {
                control.Click -= BookingCard_Click;
                control.MouseDown -= ChildControl_MouseDown;
                control.MouseUp -= ChildControl_MouseUp;
                
                control.Click += BookingCard_Click;
                control.MouseClick += (s, e) => BookingCard_Click(s, EventArgs.Empty);
                control.Cursor = Cursors.Hand;
                control.MouseDown += ChildControl_MouseDown;
                control.MouseUp += ChildControl_MouseUp;
                controlsWired++;
                
                // Also wire up nested controls
                foreach (Control nestedControl in control.Controls)
                {
                    nestedControl.Click -= BookingCard_Click;
                    nestedControl.MouseDown -= ChildControl_MouseDown;
                    nestedControl.MouseUp -= ChildControl_MouseUp;
                    
                    nestedControl.Click += BookingCard_Click;
                    nestedControl.MouseClick += (s, e) => BookingCard_Click(s, EventArgs.Empty);
                    nestedControl.Cursor = Cursors.Hand;
                    nestedControl.MouseDown += ChildControl_MouseDown;
                    nestedControl.MouseUp += ChildControl_MouseUp;
                    controlsWired++;
                }
            }
            System.Diagnostics.Debug.WriteLine($"Wired up {controlsWired} child controls");
        }

        private void BookingCard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }

        private void BookingCard_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown && e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
                BookingCard_Click(this, EventArgs.Empty);
            }
        }

        private void ChildControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }

        private void ChildControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown && e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
                BookingCard_Click(this, EventArgs.Empty);
            }
        }

        private void BookingCard_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"BookingCard_Click triggered for: {BookingData?.FullName}");
            
            if (BookingData != null)
            {
                int subscriberCount = OnCardClicked?.GetInvocationList().Length ?? 0;
                System.Diagnostics.Debug.WriteLine($"OnCardClicked event has {subscriberCount} subscribers");
                
                if (subscriberCount > 0)
                {
                    OnCardClicked?.Invoke(this, BookingData);
                    System.Diagnostics.Debug.WriteLine("OnCardClicked invoked");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: OnCardClicked has NO subscribers!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("BookingData is null!");
            }
        }

        private void LoadBookingData()
        {
            if (BookingData == null) return;

            // Set guest name
            frontGuestName.Text = BookingData.FullName ?? "Guest";

            // Set unit name
            string unitName = BookingData.Condo?.Name ?? "Unit";
            if (!string.IsNullOrEmpty(BookingData.Condo?.Location))
            {
                unitName += $" - {BookingData.Condo.Location}";
            }
            frontUnitName.Text = unitName;

            // Set arrival/departure time
            string timeInfo = "";
            if (BookingData.StartDateTime != default(DateTime))
            {
                timeInfo += BookingData.StartDateTime.ToString("MMM dd, yyyy");
            }
            if (BookingData.EndDateTime != default(DateTime))
            {
                if (!string.IsNullOrEmpty(timeInfo))
                    timeInfo += " / ";
                timeInfo += BookingData.EndDateTime.ToString("MMM dd, yyyy");
            }
            if (string.IsNullOrEmpty(timeInfo))
            {
                timeInfo = "N/A";
            }
            frontTime.Text = timeInfo;

            // Set scanned status label
            if (BookingData.Status == "CheckedIn")
            {
                scannedStatus.Text = "Checked In";
                scannedStatus.ForeColor = Color.LimeGreen;
                scannedStatus.Font = new Font(scannedStatus.Font, FontStyle.Bold);
            }
            else
            {
                scannedStatus.Text = "";
            }
        }

        public void UpdateStatus(string status)
        {
            if (BookingData != null)
            {
                BookingData.Status = status;
                LoadBookingData();
            }
        }

        private void BookingCard_Load(object sender, EventArgs e)
        {
            if (BookingData != null)
            {
                LoadBookingData();
            }
        }

        private void frontUnitName_Click(object sender, EventArgs e)
        {

        }
    }
}
