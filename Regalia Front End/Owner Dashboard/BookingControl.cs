using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Regalia_Front_End
{
    public partial class BookingControl : UserControl
    {
        public BookingControl()
        {
            InitializeComponent();
            // Hide bookingInformationPnl initially - it should only show when a card is clicked
            bookingInformationPnl.Visible = false;
            // Remove from this control so it can be added to main form for animation
            if (this.Controls.Contains(bookingInformationPnl))
            {
                this.Controls.Remove(bookingInformationPnl);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
