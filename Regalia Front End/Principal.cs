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
using Regalia_Front_End.Owner_Dashboard;

namespace Regalia_Front_End
{
    public partial class Principal : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection();
        private PropertiesControl propertiesControl;
        private PropertiesUpdateControl propertiesUpdateControl;
        private DashBoardControl dashboardControl;
        private AnimationManager animationManager;
        private PropertyFormManager propertyFormManager;
        private UpdatePropertyFormManager updatePropertyFormManager;
        private ImageManager imageManager;
        private PropertyStatusCardManager statusCardManager;
        
        public Principal()
        {
            InitializeComponent();
            this.Load += Principal_Load;
            InitializeDashboardControl();
            InitializePropertiesControl();
            InitializePropertiesUpdateControl();
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            // Show dashboard control and hide properties control
            dashboardControl.Visible = true;
            dashboardControl.BringToFront();
            propertiesControl.Visible = false;
            addPropertyBtn.Visible = false;
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
            
            // Show dashboard control when form loads
            dashboardControl.Visible = true;
            dashboardControl.BringToFront();
        }

        private void InitializeDashboardControl()
        {
            // Initialize dashboard control
            dashboardControl = new DashBoardControl();
            dashboardControl.Dock = DockStyle.Fill;
            dashboardControl.Visible = false;
            guna2Panel3.Controls.Add(dashboardControl);
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
            
            // Initialize property status card manager for dashboard
            statusCardManager = null;
            if (dashboardControl != null && dashboardControl.propertyStatusOver != null && dashboardControl.totalProperty != null)
            {
                statusCardManager = new PropertyStatusCardManager(dashboardControl.propertyStatusOver, dashboardControl.totalProperty);
            }
            
            // Initialize property form manager
            propertyFormManager = new PropertyFormManager(this, propertiesControl, imageManager, statusCardManager);
            
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
            
            // Wire up property card click events
            propertyFormManager.cardManager.OnPropertyCardClicked += PropertyCard_Clicked;
        }

        private void InitializePropertiesUpdateControl()
        {
            // Initialize update control (hidden, used for updating existing properties)
            propertiesUpdateControl = new PropertiesUpdateControl();
            propertiesUpdateControl.Dock = DockStyle.Fill;
            propertiesUpdateControl.Visible = false;
            guna2Panel3.Controls.Add(propertiesUpdateControl);
            
            // Add update property forms directly to the main form for correct centering/animation
            this.Controls.Add(propertiesUpdateControl.updPropertiesFirst);
            this.Controls.Add(propertiesUpdateControl.updProperties1);
            this.Controls.Add(propertiesUpdateControl.updProperties2);
            this.Controls.Add(propertiesUpdateControl.updProperties3);
            this.Controls.Add(propertiesUpdateControl.updProperties4);
            propertiesUpdateControl.updPropertiesFirst.BringToFront();
            propertiesUpdateControl.updProperties1.BringToFront();
            propertiesUpdateControl.updProperties2.BringToFront();
            propertiesUpdateControl.updProperties3.BringToFront();
            propertiesUpdateControl.updProperties4.BringToFront();
            
            // Initialize update property form manager
            updatePropertyFormManager = new UpdatePropertyFormManager(this, propertiesUpdateControl, propertyFormManager.cardManager, statusCardManager);
            
            // Wire up update form navigation events
            propertiesUpdateControl.updatePropertiesNext1.Click += UpdateProperties1Next_Click;
            propertiesUpdateControl.updatePropertiesBack1.Click += UpdateProperties1Back_Click;
            propertiesUpdateControl.updatePropertiesNext2.Click += UpdateProperties2Next_Click;
            propertiesUpdateControl.updatePropertiesBack2.Click += UpdateProperties2Back_Click;
            propertiesUpdateControl.updatePropertiesBack3.Click += UpdateProperties3Back_Click;
            propertiesUpdateControl.updatePropertiesNext3.Click += UpdateProperties3Next_Click;
            propertiesUpdateControl.updatePropertiesBack4.Click += UpdateProperties4Back_Click;
            propertiesUpdateControl.updateSubmitLoginBtn.Click += UpdateSubmitLoginBtn_Click; // Update button
            
            // Wire up update form close button events
            propertiesUpdateControl.updatePropertiesClose1.Click += UpdatePropertiesClose1_Click;
            propertiesUpdateControl.updatePropertiesClose2.Click += UpdatePropertiesClose2_Click;
            propertiesUpdateControl.updatePropertiesClose3.Click += UpdatePropertiesClose3_Click;
            propertiesUpdateControl.updatePropertiesClose4.Click += UpdatePropertiesClose4_Click;
            
            // Wire up updPropertiesFirst buttons
            propertiesUpdateControl.nextProperties10.Click += NextProperties10_Click;
            propertiesUpdateControl.delBtnProperties.Click += DelBtnProperties_Click;
            propertiesUpdateControl.guna2CircleButton1.Click += UpdPropertiesFirstClose_Click;
            
            // Initialize combo box for update form
            InitializeUpdateComboBox();
            
            // Note: availabilityCmb changes are NOT wired - status only updates when Update button is clicked
        }

        private void InitializeUpdateComboBox()
        {
            // Add default property types to update combo box
            propertiesUpdateControl.updTypeCmb.Items.Clear();
            propertiesUpdateControl.updTypeCmb.Items.Add("Studio Type");
            propertiesUpdateControl.updTypeCmb.Items.Add("1 Bedroom");
            propertiesUpdateControl.updTypeCmb.Items.Add("2 Bedroom");
            propertiesUpdateControl.updTypeCmb.Items.Add("3 Bedroom");
            propertiesUpdateControl.updTypeCmb.Items.Add("Penthouse");
            propertiesUpdateControl.updTypeCmb.Items.Add("Duplex");
            
            // Initialize availability combo box with status options
            propertiesUpdateControl.availabilityCmb.Items.Clear();
            propertiesUpdateControl.availabilityCmb.Items.Add("Available");
            propertiesUpdateControl.availabilityCmb.Items.Add("Maintenance");
            propertiesUpdateControl.availabilityCmb.Items.Add("Occupied");
        }

        private void PropertyCard_Clicked(object sender, PropertyData propertyData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"PropertyCard_Clicked in Principal - Property: {propertyData?.Title}");
                // Find the card that was clicked
                if (sender is PropertyCard clickedCard)
                {
                    System.Diagnostics.Debug.WriteLine($"Opening update form for card: {clickedCard.PropertyData?.Title}");
                    // Show update form with this property's data
                    updatePropertyFormManager.ShowUpdatePropertyForm(clickedCard);
                    System.Diagnostics.Debug.WriteLine("Update form shown successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Sender is not PropertyCard, it's: {sender?.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in PropertyCard_Clicked: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error opening update form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #region Properties Button Events

        private void PropertiesBtn_Click(object sender, EventArgs e)
        {
            // Show properties control and hide dashboard control
            propertiesControl.Visible = true;
            propertiesControl.BringToFront();
            addPropertyBtn.Visible = true;
            dashboardControl.Visible = false;
            
            // Ensure containerPanel is removed and cardContainer is brought to front
            // so property cards can be clicked
            if (propertyFormManager?.cardManager != null)
            {
                propertyFormManager.cardManager.BringCardContainerToFront();
            }
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

        #region Update Property Form Navigation Events

        private void UpdateProperties1Next_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties2();
        }

        private void UpdateProperties1Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdatePropertiesFirst();
        }

        private void UpdateProperties2Next_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties3();
        }

        private void UpdateProperties2Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties1();
        }

        private void UpdateProperties3Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties2();
        }

        private void UpdateProperties3Next_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties4();
        }

        private void UpdateProperties4Back_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties3();
        }

        private void UpdateSubmitLoginBtn_Click(object sender, EventArgs e)
        {
            // Update button - saves changes to property
            updatePropertyFormManager.UpdateProperty();
        }

        private void UpdatePropertiesClose1_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties1);
        }

        private void UpdatePropertiesClose2_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties2);
        }

        private void UpdatePropertiesClose3_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties3);
        }

        private void UpdatePropertiesClose4_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.CloseForm(propertiesUpdateControl.updProperties4);
        }

        private void NextProperties10_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.ShowUpdateProperties1();
        }

        private void DelBtnProperties_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.DeleteProperty();
        }

        private void UpdPropertiesFirstClose_Click(object sender, EventArgs e)
        {
            updatePropertyFormManager.HideFormWithAnimation(propertiesUpdateControl.updPropertiesFirst);
        }


        #endregion
    }
}
