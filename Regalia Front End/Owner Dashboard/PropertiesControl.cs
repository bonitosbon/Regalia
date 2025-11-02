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
    public partial class PropertiesControl : UserControl
    {
        public PropertiesControl()
        {
            InitializeComponent();
            InitializePropertyForms();
        }

        private void InitializePropertyForms()
        {
            // Initially hide all property forms
            addProperties1.Visible = false;
            addProperties2.Visible = false;
            addProperties3.Visible = false;
            addProperties4.Visible = false;
            
            // Set initial positions
            addProperties1.Location = new Point(300, 50);
            addProperties2.Location = new Point(300, 50);
            addProperties3.Location = new Point(300, 50);
            addProperties4.Location = new Point(300, 50);
            
            // Initialize combo box with default values
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            // Add default property types to combo box
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("Studio Type");
            guna2ComboBox1.Items.Add("1 Bedroom");
            guna2ComboBox1.Items.Add("2 Bedroom");
            guna2ComboBox1.Items.Add("3 Bedroom");
            guna2ComboBox1.Items.Add("Penthouse");
            guna2ComboBox1.Items.Add("Duplex");
            
            // Set default selection
            guna2ComboBox1.SelectedIndex = 0; // "Studio Type"
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void propertiesNext3_Click(object sender, EventArgs e)
        {

        }
    }
}
