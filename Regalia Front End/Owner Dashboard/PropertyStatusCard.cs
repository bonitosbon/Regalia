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
    public partial class PropertyStatusCard : UserControl
    {
        public PropertyStatusCard()
        {
            InitializeComponent();
        }

        public string UnitName
        {
            get => propStatCard?.Text ?? "";
            set
            {
                if (propStatCard != null)
                    propStatCard.Text = value ?? "";
            }
        }

        public string PropertyLocation
        {
            get => propertyStatusLoc?.Text ?? "";
            set
            {
                if (propertyStatusLoc != null)
                    propertyStatusLoc.Text = value ?? "";
            }
        }

        public string Status
        {
            get => statusLblYes?.Text ?? "Available";
            set
            {
                if (statusLblYes != null)
                {
                    string status = value ?? "Available";
                    statusLblYes.Text = status;
                    
                    // Set color based on status (same as PropertyCard)
                    switch (status.ToLower())
                    {
                        case "available":
                            statusLblYes.ForeColor = Color.FromArgb(66, 133, 244); // Blue
                            break;
                        case "maintenance":
                            statusLblYes.ForeColor = Color.FromArgb(255, 193, 7); // Yellow/Gold
                            break;
                        case "occupied":
                            statusLblYes.ForeColor = Color.FromArgb(244, 67, 54); // Red
                            break;
                        default:
                            statusLblYes.ForeColor = Color.FromArgb(66, 133, 244); // Default to blue
                            break;
                    }
                }
            }
        }
    }
}
