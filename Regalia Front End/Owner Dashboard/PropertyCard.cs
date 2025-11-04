using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Regalia_Front_End
{
    public class PropertyCard : UserControl
    {
        #region Private Fields
        private Guna2PictureBox guna2PictureBox1;
        private Label unitLbl;
        private Label typeLbl;
        private Guna2Elipse guna2Elipse1;
        private System.ComponentModel.IContainer components;
        private Label statusLbl;
        private bool isMouseDown = false;
        #endregion

        #region Public Properties
        public PropertyData PropertyData { get; private set; }
        public int CondoId { get; set; } = 0; // Store condo ID from database
        public string UnitName 
        { 
            get => unitLbl.Text; 
            set => unitLbl.Text = value ?? "Unit"; 
        }
        public string PropertyType 
        { 
            get => typeLbl.Text; 
            set => typeLbl.Text = value ?? "Studio Type"; 
        }
        public string Status 
        { 
            get => statusLbl?.Text ?? "Available"; 
            set 
            {
                if (statusLbl != null)
                {
                    string status = value ?? "Available";
                    statusLbl.Text = status;
                    
                    // Set color based on status
                    switch (status.ToLower())
                    {
                        case "available":
                            statusLbl.ForeColor = Color.FromArgb(66, 133, 244); // Blue
                            break;
                        case "maintenance":
                            statusLbl.ForeColor = Color.FromArgb(255, 193, 7); // Yellow/Gold
                            break;
                        case "occupied":
                            statusLbl.ForeColor = Color.FromArgb(244, 67, 54); // Red
                            break;
                        default:
                            statusLbl.ForeColor = Color.FromArgb(230, 245, 244); // Default color
                            break;
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public PropertyCard(PropertyData data)
        {
            PropertyData = data ?? throw new ArgumentNullException(nameof(data));
            InitializeComponent();
            LoadPropertyData();
            // Wire up clicks after control is fully loaded
            this.Load += (s, e) => WireUpChildControlClicks();
            // Also wire immediately in case Load already fired
            WireUpChildControlClicks();
            
            // Add mouse events to verify PropertyCard is receiving mouse messages
            this.MouseEnter += (s, e) => {
                System.Diagnostics.Debug.WriteLine($"PropertyCard MouseEnter - {PropertyData?.Title}");
                this.Cursor = Cursors.Hand;
            };
            this.MouseLeave += (s, e) => {
                System.Diagnostics.Debug.WriteLine($"PropertyCard MouseLeave - {PropertyData?.Title}");
                this.Cursor = Cursors.Default;
            };
            this.MouseDown += (s, e) => {
                System.Diagnostics.Debug.WriteLine($"PropertyCard MouseDown - {PropertyData?.Title} at {e.Location}");
            };
        }
        #endregion

        #region Public Methods
        public void UpdatePropertyData(PropertyData newData)
        {
            if (newData == null) return;
            
            PropertyData = newData;
            CondoId = newData.CondoId; // Preserve condo ID
            Status = newData.Status ?? "Available"; // Update status
            LoadPropertyData();
        }

        public void SetImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Check if it's a base64 data URI
                    if (imagePath.StartsWith("data:image/"))
                    {
                        // Convert base64 to Image
                        guna2PictureBox1.Image = Helpers.ImageBase64Helper.ConvertBase64ToImage(imagePath);
                        System.Diagnostics.Debug.WriteLine($"PropertyCard: Successfully loaded Base64 image");
                    }
                    else if (File.Exists(imagePath))
                    {
                        // It's a file path, load normally
                        guna2PictureBox1.Image = Image.FromFile(imagePath);
                        System.Diagnostics.Debug.WriteLine($"PropertyCard: Successfully loaded image from file: {imagePath}");
                    }
                    else
                    {
                        // Might be a URL or invalid path - try next image
                        System.Diagnostics.Debug.WriteLine($"PropertyCard: Image path not found or invalid: {imagePath}");
                        guna2PictureBox1.Image = null;
                    }
                }
                else
                {
                    guna2PictureBox1.Image = null;
                }
            }
            catch (Exception ex)
            {
                // Log error or show user-friendly message
                System.Diagnostics.Debug.WriteLine($"Error loading image: {ex.Message}");
                guna2PictureBox1.Image = null;
            }
        }
        #endregion

        #region Private Methods
        private void LoadPropertyData()
        {
            if (PropertyData == null) return;

            // Set unit name from title
            UnitName = PropertyData.Title ?? "Unit Name";
            
            // Set property type from location/combo box
            PropertyType = PropertyData.Location ?? "Studio Type";
            
            // Set status from PropertyData, or use current status if PropertyData.Status is empty
            if (!string.IsNullOrWhiteSpace(PropertyData.Status))
            {
                Status = PropertyData.Status;
            }
            else if (string.IsNullOrWhiteSpace(Status))
            {
                Status = "Available"; // Default only if both are empty
            }
            
            // Load first available image - try all images in order
            if (!string.IsNullOrEmpty(PropertyData.Image1Path))
            {
                SetImage(PropertyData.Image1Path);
            }
            else if (!string.IsNullOrEmpty(PropertyData.Image2Path))
            {
                SetImage(PropertyData.Image2Path);
            }
            else if (!string.IsNullOrEmpty(PropertyData.Image3Path))
            {
                SetImage(PropertyData.Image3Path);
            }
            else if (!string.IsNullOrEmpty(PropertyData.Image4Path))
            {
                SetImage(PropertyData.Image4Path);
            }
        }

        private void WireUpChildControlClicks()
        {
            // Make all child controls propagate click events to parent
            // This ensures the entire card is clickable, not just empty spaces
            foreach (Control control in this.Controls)
            {
                System.Diagnostics.Debug.WriteLine($"Wiring up click events for control: {control.Name} ({control.GetType().Name})");
                
                // Wire up all mouse events
                control.MouseDown += ChildControl_MouseDown;
                control.MouseUp += ChildControl_MouseUp;
                control.MouseClick += ChildControl_MouseClick;
                control.Click += ChildControl_Click;
                control.DoubleClick += ChildControl_DoubleClick;
                control.MouseDoubleClick += ChildControl_MouseDoubleClick;
                
                // Set cursor to hand on child controls too
                control.Cursor = Cursors.Hand;
                
                // Special handling for PictureBox - make sure it's clickable
                if (control is Guna.UI2.WinForms.Guna2PictureBox pictureBox)
                {
                    pictureBox.Cursor = Cursors.Hand;
                    // Make sure PictureBox events are wired
                    pictureBox.MouseClick += (s, args) => {
                        System.Diagnostics.Debug.WriteLine($"Guna2PictureBox MouseClick event triggered");
                        ChildControl_MouseClick(s, args);
                    };
                }
            }
            
            // Also handle mouse events at the control level
            this.MouseDown += PropertyCard_MouseDown;
            this.MouseUp += PropertyCard_MouseUp;
            
            System.Diagnostics.Debug.WriteLine($"PropertyCard click events wired. Total child controls: {this.Controls.Count}");
        }

        private void PropertyCard_MouseDown(object sender, MouseEventArgs e)
        {
            // Handle mouse down on the card itself
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }

        private void PropertyCard_MouseUp(object sender, MouseEventArgs e)
        {
            // Handle mouse up - trigger click if mouse was down
            if (isMouseDown && e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
                System.Diagnostics.Debug.WriteLine($"PropertyCard_MouseUp triggered click for card: {PropertyData?.Title}");
                PropertyCard_Click(this, EventArgs.Empty);
            }
        }

        private void ChildControl_MouseDown(object sender, MouseEventArgs e)
        {
            // Stop event from bubbling and trigger our click
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }

        private void ChildControl_MouseUp(object sender, MouseEventArgs e)
        {
            // Trigger click when mouse is released
            if (isMouseDown && e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
                System.Diagnostics.Debug.WriteLine($"ChildControl_MouseUp triggered click from: {((Control)sender).Name}");
                PropertyCard_Click(this, EventArgs.Empty);
            }
        }

        private void ChildControl_Click(object sender, EventArgs e)
        {
            // Propagate click to parent card
            System.Diagnostics.Debug.WriteLine($"ChildControl_Click triggered from: {((Control)sender).Name}");
            PropertyCard_Click(this, e);
        }

        private void ChildControl_DoubleClick(object sender, EventArgs e)
        {
            // Propagate double click to parent card
            PropertyCard_DoubleClick(this, e);
        }

        private void ChildControl_MouseClick(object sender, MouseEventArgs e)
        {
            // Propagate mouse click to parent card
            PropertyCard_Click(this, EventArgs.Empty);
        }

        private void ChildControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Propagate mouse double click to parent card
            PropertyCard_DoubleClick(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void PropertyCard_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"PropertyCard_Click triggered for card: {PropertyData?.Title}");
                if (OnCardClicked != null)
                {
                    System.Diagnostics.Debug.WriteLine($"OnCardClicked has {OnCardClicked.GetInvocationList().Length} subscribers");
                    OnCardClicked?.Invoke(this, PropertyData);
                    System.Diagnostics.Debug.WriteLine("OnCardClicked invoked successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("OnCardClicked is NULL - event not wired!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in PropertyCard_Click: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error handling card click: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PropertyCard_DoubleClick(object sender, EventArgs e)
        {
            OnCardDoubleClicked?.Invoke(this, PropertyData);
        }
        #endregion

        #region Public Events
        public event EventHandler<PropertyData> OnCardClicked;
        public event EventHandler<PropertyData> OnCardDoubleClicked;
        #endregion

        #region Designer Code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.unitLbl = new System.Windows.Forms.Label();
            this.typeLbl = new System.Windows.Forms.Label();
            this.statusLbl = new System.Windows.Forms.Label();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(257, 202);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 0;
            this.guna2PictureBox1.TabStop = false;
            // 
            // unitLbl
            // 
            this.unitLbl.AutoSize = true;
            this.unitLbl.BackColor = System.Drawing.Color.Transparent;
            this.unitLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unitLbl.ForeColor = System.Drawing.Color.White;
            this.unitLbl.Location = new System.Drawing.Point(29, 219);
            this.unitLbl.Name = "unitLbl";
            this.unitLbl.Size = new System.Drawing.Size(69, 36);
            this.unitLbl.TabIndex = 1;
            this.unitLbl.Text = "Unit";
            // 
            // typeLbl
            // 
            this.typeLbl.AutoSize = true;
            this.typeLbl.BackColor = System.Drawing.Color.Transparent;
            this.typeLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(245)))), ((int)(((byte)(244)))));
            this.typeLbl.Location = new System.Drawing.Point(30, 267);
            this.typeLbl.Name = "typeLbl";
            this.typeLbl.Size = new System.Drawing.Size(57, 25);
            this.typeLbl.TabIndex = 2;
            this.typeLbl.Text = "Type";
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.BackColor = System.Drawing.Color.Transparent;
            this.statusLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(245)))), ((int)(((byte)(244)))));
            this.statusLbl.Location = new System.Drawing.Point(30, 301);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(68, 25);
            this.statusLbl.TabIndex = 3;
            this.statusLbl.Text = "Status";
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // PropertyCard
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.typeLbl);
            this.Controls.Add(this.unitLbl);
            this.Controls.Add(this.guna2PictureBox1);
            this.Name = "PropertyCard";
            this.Size = new System.Drawing.Size(250, 344);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
            this.Load += new System.EventHandler(this.PropertyCard_Load);
            this.Click += new System.EventHandler(this.PropertyCard_Click);
            this.DoubleClick += new System.EventHandler(this.PropertyCard_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PropertyCard_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PropertyCard_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Cleanup
        public void Cleanup()
        {
            guna2PictureBox1?.Dispose();
        }
        #endregion

        private void PropertyCard_Load(object sender, EventArgs e)
        {
            // Ensure click events are wired when control loads
            WireUpChildControlClicks();
        }
    }

    public class PropertyData
    {
        #region Public Properties
        public int CondoId { get; set; } = 0; // Store condo ID from database
        public string Title { get; set; }
        public string Price { get; set; }
        public string Location { get; set; }
        public string Bedrooms { get; set; }
        public string Bathrooms { get; set; }
        public string Area { get; set; }
        public string Image1Path { get; set; }
        public string Image2Path { get; set; }
        public string Image3Path { get; set; }
        public string Image4Path { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = "Available"; // Store status
        public string BookingLink { get; set; } // Booking link for guest booking page
        #endregion

        #region Constructor
        public PropertyData()
        {
            CreatedDate = DateTime.Now;
        }
        #endregion

        #region Public Methods
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Price);
        }

        public string GetDisplayTitle()
        {
            return Title ?? "Untitled Property";
        }

        public string GetDisplayPrice()
        {
            return Price ?? "$0";
        }

        public string GetDisplayLocation()
        {
            return Location ?? "Studio Type";
        }
        #endregion
    }
}