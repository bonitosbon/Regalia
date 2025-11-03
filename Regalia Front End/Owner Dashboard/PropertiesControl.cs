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
        private FlowLayoutPanel cardContainer;

        public PropertiesControl()
        {
            System.Diagnostics.Debug.WriteLine("=== PropertiesControl Constructor START ===");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("InitializeComponent completed");
            InitializePropertyForms();
            System.Diagnostics.Debug.WriteLine("InitializePropertyForms completed");
            SetupContainerPanel();
            System.Diagnostics.Debug.WriteLine("SetupContainerPanel completed");
            System.Diagnostics.Debug.WriteLine("=== PropertiesControl Constructor END ===");
        }

        private void SetupContainerPanel()
        {
            // CRITICAL FIX: Remove containerPanel from Controls when not needed so it can't block clicks
            System.Diagnostics.Debug.WriteLine($"SetupContainerPanel called. containerPanel is null: {containerPanel == null}");
            if (containerPanel != null)
            {
                System.Diagnostics.Debug.WriteLine($"Before removal - Controls count: {this.Controls.Count}, Contains containerPanel: {this.Controls.Contains(containerPanel)}");
                // Remove from Controls collection so it can't intercept any mouse events
                this.Controls.Remove(containerPanel);
                System.Diagnostics.Debug.WriteLine($"After removal - Controls count: {this.Controls.Count}, Contains containerPanel: {this.Controls.Contains(containerPanel)}");
                System.Diagnostics.Debug.WriteLine("containerPanel removed from Controls collection");
                
                // Listen for when cardContainer is added
                this.ControlAdded += PropertiesControl_ControlAdded;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ERROR: containerPanel is NULL!");
            }
        }
        
        public void RestoreContainerPanelPosition()
        {
            if (containerPanel != null)
            {
                // Add containerPanel back to Controls when we need to show forms
                if (!this.Controls.Contains(containerPanel))
                {
                    this.Controls.Add(containerPanel);
                    containerPanel.Dock = DockStyle.Fill;
                    containerPanel.Enabled = true;
                    containerPanel.Visible = true;
                    containerPanel.BringToFront();
                    System.Diagnostics.Debug.WriteLine("containerPanel added back to Controls collection");
                }
            }
        }
        
        public void MoveContainerPanelOutOfWay()
        {
            if (containerPanel != null)
            {
                // Remove containerPanel from Controls so it can't intercept mouse events
                if (this.Controls.Contains(containerPanel))
                {
                    this.Controls.Remove(containerPanel);
                    containerPanel.Enabled = false;
                    containerPanel.Visible = false;
                    System.Diagnostics.Debug.WriteLine("containerPanel removed from Controls collection");
                }
            }
        }

        private void PropertiesControl_ControlAdded(object sender, ControlEventArgs e)
        {
            // Track cardContainer when it's added
            if (e.Control is FlowLayoutPanel flowPanel)
            {
                cardContainer = flowPanel;
                cardContainer.BringToFront();
                System.Diagnostics.Debug.WriteLine("CardContainer detected and brought to front");
            }
        }

        public void RegisterCardContainer(FlowLayoutPanel container)
        {
            cardContainer = container;
            if (cardContainer != null)
            {
                cardContainer.BringToFront();
                System.Diagnostics.Debug.WriteLine($"CardContainer registered and brought to front. containerPanel Enabled: {containerPanel?.Enabled}");
            }
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
            addTypeCmb.Items.Clear();
            addTypeCmb.Items.Add("Studio Type");
            addTypeCmb.Items.Add("1 Bedroom");
            addTypeCmb.Items.Add("2 Bedroom");
            addTypeCmb.Items.Add("3 Bedroom");
            addTypeCmb.Items.Add("Penthouse");
            addTypeCmb.Items.Add("Duplex");
            
            // Set default selection
            addTypeCmb.SelectedIndex = 0; // "Studio Type"
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
