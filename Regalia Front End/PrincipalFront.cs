using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Regalia_Front_End.Front_Desk_Dashboard;
using Regalia_Front_End.Services;

namespace Regalia_Front_End
{
    public partial class PrincipalFront : Form
    {
        private frontDashboard frontDashboardControl;
        private FrontBookingControl frontBookingControl;
        private MouseEventHandler logoutMouseClickHandler;
        private MouseEventHandler logoutMouseDownHandler;

        public PrincipalFront()
        {
            InitializeComponent();
            InitializeControls();
            WireUpEvents();
        }

        private void InitializeControls()
        {
            // Initialize front dashboard control
            frontDashboardControl = new frontDashboard();
            frontDashboardControl.Dock = DockStyle.Fill;
            frontDashboardControl.Visible = true;
            guna2Panel3.Controls.Add(frontDashboardControl);
            frontDashboardControl.BringToFront();

            // Initialize front booking control
            frontBookingControl = new FrontBookingControl();
            frontBookingControl.Dock = DockStyle.Fill;
            frontBookingControl.Visible = false;
            guna2Panel3.Controls.Add(frontBookingControl);
            
            // Set parent form for scanner panel animations
            frontBookingControl.SetParentForm(this);
        }

        private void WireUpEvents()
        {
            // Wire up dashboard button click
            frontdashboardBtn.Click += FrontDashboardBtn_Click;
            
            // Create reusable mouse click handler
            logoutMouseClickHandler = (s, e) => { System.Diagnostics.Debug.WriteLine("Logout button MouseClick fired"); logOutBtn_Click(s, e); };
            logoutMouseDownHandler = (s, e) => { System.Diagnostics.Debug.WriteLine("Logout button MouseDown fired"); };
            
            // Wire up logout button with multiple events to ensure it works
            logOutBtn.Click -= logOutBtn_Click;
            logOutBtn.Click += logOutBtn_Click;
            logOutBtn.MouseClick -= logoutMouseClickHandler;
            logOutBtn.MouseClick += logoutMouseClickHandler;
            logOutBtn.MouseDown -= logoutMouseDownHandler;
            logOutBtn.MouseDown += logoutMouseDownHandler;
            
            // Ensure button is enabled and visible
            logOutBtn.Enabled = true;
            logOutBtn.Visible = true;
            logOutBtn.BringToFront();
            
            // Fix button position if it's being cut off
            if (guna2Panel1 != null && logOutBtn.Location.Y + logOutBtn.Height > guna2Panel1.Height)
            {
                System.Diagnostics.Debug.WriteLine($"Warning: Logout button may be cut off. Panel height: {guna2Panel1.Height}, Button bottom: {logOutBtn.Location.Y + logOutBtn.Height}");
                // Adjust button position to fit within panel
                int newY = guna2Panel1.Height - logOutBtn.Height - 10; // 10px margin from bottom
                logOutBtn.Location = new System.Drawing.Point(logOutBtn.Location.X, newY);
                System.Diagnostics.Debug.WriteLine($"Adjusted logout button Y position to: {newY}");
            }
            
            // Also ensure the button can receive clicks by checking if it's in a valid state
            System.Diagnostics.Debug.WriteLine($"Logout button state - Enabled: {logOutBtn.Enabled}, Visible: {logOutBtn.Visible}, Location: {logOutBtn.Location}, Size: {logOutBtn.Size}");
            
            // Booking button already wired in designer, just implement the handler
        }

        private void FrontDashboardBtn_Click(object sender, EventArgs e)
        {
            // Show dashboard and hide booking
            frontDashboardControl.Visible = true;
            frontDashboardControl.BringToFront();
            frontBookingControl.Visible = false;
        }

        private void PrincipalFront_Load(object sender, EventArgs e)
        {
            // Ensure dashboard is visible on load
            if (frontDashboardControl != null)
            {
                frontDashboardControl.Visible = true;
                frontDashboardControl.BringToFront();
            }
            
            // Re-wire logout button in Load event to ensure it's properly connected
            if (logOutBtn != null)
            {
                logOutBtn.Click -= logOutBtn_Click;
                logOutBtn.Click += logOutBtn_Click;
                if (logoutMouseClickHandler != null)
                {
                    logOutBtn.MouseClick -= logoutMouseClickHandler;
                    logOutBtn.MouseClick += logoutMouseClickHandler;
                }
                logOutBtn.Enabled = true;
                logOutBtn.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Logout button wired in Load - Enabled: {logOutBtn.Enabled}, Visible: {logOutBtn.Visible}");
            }
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bookingBtn_Click(object sender, EventArgs e)
        {
            // Show booking and hide dashboard
            frontBookingControl.Visible = true;
            frontBookingControl.BringToFront();
            frontDashboardControl.Visible = false;
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void logOutBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Logout button clicked in PrincipalFront");
            
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
