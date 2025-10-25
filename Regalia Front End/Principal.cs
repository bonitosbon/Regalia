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

namespace Regalia_Front_End
{
    public partial class Principal : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection();
        
        public Principal()
        {
            InitializeComponent();
            this.Load += Principal_Load;
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Principal_Load(object sender, EventArgs e)
        {
            string fontPath = Path.Combine(Application.StartupPath, "Fonts", "Roca Two Bold.ttf");
            pfc.AddFontFile(fontPath);
            label1.Font = new Font(pfc.Families[0], 36, FontStyle.Regular);
        }
    }
}
