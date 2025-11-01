using System;
using System.Drawing;
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
        #endregion

        #region Public Properties
        public PropertyData PropertyData { get; private set; }
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
            get => statusLbl.Text; 
            set => statusLbl.Text = value ?? "Available"; 
        }
        #endregion

        #region Constructor
        public PropertyCard(PropertyData data)
        {
            PropertyData = data ?? throw new ArgumentNullException(nameof(data));
            InitializeComponent();
            LoadPropertyData();
        }
        #endregion

        #region Public Methods
        public void UpdatePropertyData(PropertyData newData)
        {
            if (newData == null) return;
            
            PropertyData = newData;
            LoadPropertyData();
        }

        public void SetImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    guna2PictureBox1.Image = Image.FromFile(imagePath);
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
            
            // Set default status
            Status = "Available";
            
            // Load first image if available
            SetImage(PropertyData.Image1Path);
        }
        #endregion

        #region Event Handlers
        private void PropertyCard_Click(object sender, EventArgs e)
        {
            OnCardClicked?.Invoke(this, PropertyData);
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
            this.Load += new System.EventHandler(this.PropertyCard_Load);
            this.Click += new System.EventHandler(this.PropertyCard_Click);
            this.DoubleClick += new System.EventHandler(this.PropertyCard_DoubleClick);
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

        }
    }

    public class PropertyData
    {
        #region Public Properties
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