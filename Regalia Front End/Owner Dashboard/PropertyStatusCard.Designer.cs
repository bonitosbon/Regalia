namespace Regalia_Front_End.Owner_Dashboard
{
    partial class PropertyStatusCard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propStatCard = new System.Windows.Forms.Label();
            this.propertyStatusLoc = new System.Windows.Forms.Label();
            this.statusLblYes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // propStatCard
            // 
            this.propStatCard.AutoSize = true;
            this.propStatCard.BackColor = System.Drawing.Color.Transparent;
            this.propStatCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propStatCard.ForeColor = System.Drawing.Color.White;
            this.propStatCard.Location = new System.Drawing.Point(17, 11);
            this.propStatCard.Name = "propStatCard";
            this.propStatCard.Size = new System.Drawing.Size(58, 22);
            this.propStatCard.TabIndex = 0;
            this.propStatCard.Text = "label1";
            // 
            // propertyStatusLoc
            // 
            this.propertyStatusLoc.AutoSize = true;
            this.propertyStatusLoc.BackColor = System.Drawing.Color.Transparent;
            this.propertyStatusLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyStatusLoc.ForeColor = System.Drawing.Color.White;
            this.propertyStatusLoc.Location = new System.Drawing.Point(18, 43);
            this.propertyStatusLoc.Name = "propertyStatusLoc";
            this.propertyStatusLoc.Size = new System.Drawing.Size(44, 16);
            this.propertyStatusLoc.TabIndex = 1;
            this.propertyStatusLoc.Text = "label1";
            // 
            // statusLblYes
            // 
            this.statusLblYes.AutoSize = true;
            this.statusLblYes.BackColor = System.Drawing.Color.Transparent;
            this.statusLblYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLblYes.ForeColor = System.Drawing.Color.Transparent;
            this.statusLblYes.Location = new System.Drawing.Point(180, 24);
            this.statusLblYes.Name = "statusLblYes";
            this.statusLblYes.Size = new System.Drawing.Size(44, 16);
            this.statusLblYes.TabIndex = 2;
            this.statusLblYes.Text = "label1";
            // 
            // PropertyStatusCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.statusLblYes);
            this.Controls.Add(this.propertyStatusLoc);
            this.Controls.Add(this.propStatCard);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Name = "PropertyStatusCard";
            this.Size = new System.Drawing.Size(374, 64);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label propStatCard;
        private System.Windows.Forms.Label propertyStatusLoc;
        private System.Windows.Forms.Label statusLblYes;
    }
}
