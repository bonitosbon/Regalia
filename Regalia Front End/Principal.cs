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
        private PropertiesControl propertiesControl;
        private AnimationManager animationManager;
        private PropertyFormManager propertyFormManager;
        private ImageManager imageManager;
        
        public Principal()
        {
            InitializeComponent();
            this.Load += Principal_Load;
            InitializePropertiesControl();
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

        private void InitializePropertiesControl()
        {
            // Initialize properties control
            propertiesControl = new PropertiesControl();
            propertiesControl.Dock = DockStyle.Fill;
            propertiesControl.Visible = false;
            guna2Panel3.Controls.Add(propertiesControl);
            // Add property forms directly to the main form for correct centering/animation
            this.Controls.Add(propertiesControl.addProperties1);
            this.Controls.Add(propertiesControl.addProperties2);
            this.Controls.Add(propertiesControl.addProperties3);
            this.Controls.Add(propertiesControl.addProperties4);
            propertiesControl.addProperties1.BringToFront();
            propertiesControl.addProperties2.BringToFront();
            propertiesControl.addProperties3.BringToFront();
            propertiesControl.addProperties4.BringToFront();
            
            // Initialize animation manager for property forms
            animationManager = new AnimationManager(this, propertiesControl.addProperties1, propertiesControl.addProperties2);
            animationManager.SetProperties3Panel(propertiesControl.addProperties3);
            
            // Initialize image manager
            imageManager = new ImageManager(propertiesControl);
            
            // Initialize property form manager
            propertyFormManager = new PropertyFormManager(this, propertiesControl, imageManager);
            
            // Wire up button events
            propertiesBtn.Click += PropertiesBtn_Click;
            addPropertyBtn.Click += AddPropertyBtn_Click;
            
            // Wire up property form navigation events
            propertiesControl.propertiesNext1.Click += Properties1Next_Click; // Next button in addProperties1
            propertiesControl.propertiesNext2.Click += Properties2Next_Click; // Next button in addProperties2
            propertiesControl.propertiesBack2.Click += Properties2Back_Click; // Back button in addProperties2
            propertiesControl.propertiesBack3.Click += Properties3Back_Click; // Back button in addProperties3
            propertiesControl.propertiesNext3.Click += Properties3Next_Click; // Next button in addProperties3 (navigates to page 4)
            propertiesControl.propertiesBack4.Click += Properties4Back_Click; // Back button in addProperties4
            // Note: submitLoginBtn is shared between page 3 and page 4 - will handle dynamically
            propertiesControl.submitLoginBtn.Click += SubmitLoginBtn_Click; // Submit button (page 3: goes to page 4, page 4: submits front desk)
            
            // Wire up close button events
            propertiesControl.propertiesClose1.Click += PropertiesClose1_Click; // Close button in addProperties1
            propertiesControl.propertiesClose2.Click += PropertiesClose2_Click; // Close button in addProperties2
            propertiesControl.propertiesClose3.Click += PropertiesClose3_Click; // Close button in addProperties3
            propertiesControl.propertiesClose4.Click += PropertiesClose4_Click; // Close button in addProperties4
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #region Properties Button Events

        private void PropertiesBtn_Click(object sender, EventArgs e)
        {
            // Show properties control and add property button
            propertiesControl.Visible = true;
            addPropertyBtn.Visible = true;
            propertiesControl.BringToFront();
        }

        private void AddPropertyBtn_Click(object sender, EventArgs e)
        {
            // Animate and show Add Property step 1 over the gradient
            propertyFormManager.ShowAddPropertyForm();
        }

        #endregion

        #region Property Form Navigation Events

        private void Properties1Next_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties2();
        }

        private void Properties2Next_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties3();
        }

        private void Properties2Back_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties1();
        }

        private void Properties3Back_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties2();
        }

        private void Properties3Next_Click(object sender, EventArgs e)
        {
            // Navigate to page 4 without submitting property
            // Property will be created when submitLoginBtn is clicked on page 4
            propertyFormManager.ShowProperties4();
        }

        private void SubmitLoginBtn_Click(object sender, EventArgs e)
        {
            // submitLoginBtn is only on page 4
            // When clicked, it should submit both the property AND the front desk account
            propertyFormManager.SubmitProperty(navigateToPage4: false); // Submit property (adds card) without navigating
            propertyFormManager.SubmitFrontDeskAccount(); // Submit front desk account and close
        }

        private void Properties4Back_Click(object sender, EventArgs e)
        {
            propertyFormManager.ShowProperties3();
        }

        private void PropertiesClose1_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties1);
        }

        private void PropertiesClose2_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties2);
        }

        private void PropertiesClose3_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties3);
        }

        private void PropertiesClose4_Click(object sender, EventArgs e)
        {
            propertyFormManager.CloseForm(propertiesControl.addProperties4);
        }

        #endregion

        private void propertiesBtn_Click_1(object sender, EventArgs e)
        {

        }
    }
}
