using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Regalia_Front_End.Owner_Dashboard
{
    public partial class PropertiesUpdateControl : UserControl
    {
        public PropertiesUpdateControl()
        {
            InitializeComponent();
            InitializeUpdatePropertyForms();
        }

        private void InitializeUpdatePropertyForms()
        {
            // Initially hide all property forms
            updPropertiesFirst.Visible = false;
            updProperties1.Visible = false;
            updProperties2.Visible = false;
            updProperties3.Visible = false;
            updProperties4.Visible = false;
            
            // Set initial positions
            updPropertiesFirst.Location = new Point(300, 50);
            updProperties1.Location = new Point(300, 50);
            updProperties2.Location = new Point(300, 50);
            updProperties3.Location = new Point(300, 50);
            updProperties4.Location = new Point(300, 50);
        }

        private void updNameTxt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
