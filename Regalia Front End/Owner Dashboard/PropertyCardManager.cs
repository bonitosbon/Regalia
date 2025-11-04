using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Regalia_Front_End
{
    public class PropertyCardManager
    {
        #region Private Fields
        private List<PropertyCard> propertyCards;
        private PropertiesControl propertiesControl;
        private const int CARDS_PER_ROW = 3;
        private const int CARD_SPACING = 25;
        #endregion
        
        #region Public Properties
        public FlowLayoutPanel cardContainer { get; private set; }
        public int CardCount => propertyCards.Count;
        public bool HasCards => propertyCards.Count > 0;
        #endregion

        #region Constructor
        public PropertyCardManager(PropertiesControl propertiesCtrl)
        {
            propertiesControl = propertiesCtrl ?? throw new ArgumentNullException(nameof(propertiesCtrl));
            propertyCards = new List<PropertyCard>();
            InitializeCardContainer();
        }
        #endregion

        #region Public Methods
        public void AddPropertyCard(PropertyData propertyData)
        {
            if (propertyData == null) 
                throw new ArgumentNullException(nameof(propertyData));

            if (!propertyData.IsValid())
            {
                MessageBox.Show("Invalid property data. Please ensure title and price are provided.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Create new property card
                PropertyCard newCard = CreatePropertyCard(propertyData);
                
                // Ensure card is visible before adding
                newCard.Visible = true;
                newCard.Show();
                
                // Debug: Check if container exists and is valid
                if (cardContainer == null)
                {
                    throw new InvalidOperationException("Card container is not initialized!");
                }
                
                // Add to collection and container
                propertyCards.Add(newCard);
                cardContainer.Controls.Add(newCard);
                
                // CRITICAL: Ensure PropertiesControl is visible so cards can be seen
                if (!propertiesControl.Visible)
                {
                    propertiesControl.Visible = true;
                    propertiesControl.Show();
                }
                
                // Ensure container is visible
                cardContainer.Visible = true;
                cardContainer.Show();
                
                // CRITICAL: FORCE REMOVE containerPanel - it's blocking all clicks!
                if (propertiesControl.containerPanel != null)
                {
                    if (propertiesControl.Controls.Contains(propertiesControl.containerPanel))
                    {
                        System.Diagnostics.Debug.WriteLine("FORCE REMOVING containerPanel from Controls NOW!");
                        propertiesControl.Controls.Remove(propertiesControl.containerPanel);
                        propertiesControl.containerPanel.Enabled = false;
                        propertiesControl.containerPanel.Visible = false;
                    }
                }
                
                // ALWAYS bring cardContainer to front so cards can receive clicks
                cardContainer.BringToFront();
                
                // Set cardContainer properties to ensure it can receive mouse events
                cardContainer.Enabled = true;
                cardContainer.TabStop = false;
                
                // CRITICAL CHECK: Is containerPanel still blocking?
                if (propertiesControl.containerPanel != null)
                {
                    if (propertiesControl.Controls.Contains(propertiesControl.containerPanel))
                    {
                        // Force remove it again
                        propertiesControl.Controls.Remove(propertiesControl.containerPanel);
                    }
                }
                
                // But bring the new card to front within its container
                newCard.BringToFront();
                
                // Refresh layout (will be done in batch when ResumeLayout is called)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating property card: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RemovePropertyCard(PropertyCard card)
        {
            if (card == null) return;

            try
            {
                // Remove from collection
                propertyCards.Remove(card);
                
                // Remove from container
                cardContainer.Controls.Remove(card);
                
                // Dispose the card
                card.Cleanup();
                card.Dispose();
                
                // Refresh layout
                RefreshLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing property card: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ClearAllCards()
        {
            try
            {
                foreach (var card in propertyCards)
                {
                    card.Cleanup();
                    card.Dispose();
                }
                
                propertyCards.Clear();
                cardContainer.Controls.Clear();
                RefreshLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing property cards: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public PropertyCard GetCardByIndex(int index)
        {
            if (index < 0 || index >= propertyCards.Count)
                return null;
            
            return propertyCards[index];
        }

        public List<PropertyCard> GetAllCards()
        {
            return new List<PropertyCard>(propertyCards);
        }

        public void BringCardContainerToFront()
        {
            if (cardContainer != null)
            {
                // CRITICAL: Remove containerPanel from Controls to prevent it from blocking clicks
                if (propertiesControl.containerPanel != null)
                {
                    if (propertiesControl.Controls.Contains(propertiesControl.containerPanel))
                    {
                        System.Diagnostics.Debug.WriteLine("BringCardContainerToFront: Removing containerPanel from Controls");
                        propertiesControl.Controls.Remove(propertiesControl.containerPanel);
                    }
                    propertiesControl.containerPanel.Enabled = false;
                    propertiesControl.containerPanel.Visible = false;
                }
                
                // Bring cardContainer to front and ensure it's enabled
                cardContainer.Enabled = true;
                cardContainer.BringToFront();
                System.Diagnostics.Debug.WriteLine($"BringCardContainerToFront: cardContainer brought to front, Enabled: {cardContainer.Enabled}");
            }
        }
        #endregion

        #region Private Methods
        private void InitializeCardContainer()
        {
            // Restore original FlowLayoutPanel container
            this.cardContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(CARD_SPACING),
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Visible = true,
                Enabled = true,
                TabStop = false,
                Cursor = Cursors.Default
            };
            
            // Add mouse events to verify cardContainer is receiving mouse messages
            cardContainer.MouseEnter += (s, e) => System.Diagnostics.Debug.WriteLine("cardContainer MouseEnter");
            cardContainer.MouseLeave += (s, e) => System.Diagnostics.Debug.WriteLine("cardContainer MouseLeave");
            cardContainer.MouseClick += (s, e) => System.Diagnostics.Debug.WriteLine($"cardContainer MouseClick at {e.Location}");
            cardContainer.MouseDown += (s, e) => System.Diagnostics.Debug.WriteLine($"cardContainer MouseDown at {e.Location}");

            // CRITICAL: Disable containerPanel FIRST before adding cardContainer
            if (propertiesControl.containerPanel != null)
            {
                propertiesControl.containerPanel.Enabled = false;
                propertiesControl.containerPanel.TabStop = false;
                // Send containerPanel to back
                propertiesControl.containerPanel.SendToBack();
            }
            
            // Add card container to PropertiesControl
            propertiesControl.Controls.Add(cardContainer);
            
            // CRITICAL: Register cardContainer with PropertiesControl so it can forward clicks
            propertiesControl.RegisterCardContainer(cardContainer);
            
            // CRITICAL: Bring cardContainer to front IMMEDIATELY after adding
            cardContainer.BringToFront();
            
            // Also set TabIndex and ensure it can receive focus
            cardContainer.TabIndex = 1;
            cardContainer.TabStop = true;
            
            System.Diagnostics.Debug.WriteLine($"CardContainer initialized - Added to PropertiesControl, brought to front. containerPanel Enabled: {propertiesControl.containerPanel?.Enabled}, Controls count: {propertiesControl.Controls.Count}");
        }

        private PropertyCard CreatePropertyCard(PropertyData propertyData)
        {
            PropertyCard newCard = new PropertyCard(propertyData);
            
            // Store condo ID in card for update/delete operations
            newCard.CondoId = propertyData.CondoId;
            
            // Ensure PropertyData also has CondoId set
            if (newCard.PropertyData != null && newCard.PropertyData.CondoId == 0 && propertyData.CondoId > 0)
            {
                newCard.PropertyData.CondoId = propertyData.CondoId;
            }
            
            System.Diagnostics.Debug.WriteLine($"CreatePropertyCard: Created card for {propertyData.Title}, CondoId = {newCard.CondoId}, PropertyData.CondoId = {newCard.PropertyData?.CondoId ?? 0}");
            
            // Ensure card is visible and properly sized
            newCard.Visible = true;
            newCard.Size = new Size(250, 344); // Ensure proper size
            newCard.Margin = new Padding(CARD_SPACING / 2);
            
            // Wire up events
            newCard.OnCardClicked += PropertyCard_OnCardClicked;
            newCard.OnCardDoubleClicked += PropertyCard_OnCardDoubleClicked;
            
            return newCard;
        }

        private void PropertyCard_OnCardClicked(object sender, PropertyData propertyData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"PropertyCard_OnCardClicked in PropertyCardManager - Property: {propertyData?.Title}");
                // Handle single click - could show property details
                if (OnPropertyCardClicked != null)
                {
                    System.Diagnostics.Debug.WriteLine($"OnPropertyCardClicked has {OnPropertyCardClicked.GetInvocationList().Length} subscribers");
                    OnPropertyCardClicked?.Invoke(sender, propertyData);
                    System.Diagnostics.Debug.WriteLine("OnPropertyCardClicked invoked successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("OnPropertyCardClicked is NULL - event not wired in Principal!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in PropertyCard_OnCardClicked: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error in card manager: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PropertyCard_OnCardDoubleClicked(object sender, PropertyData propertyData)
        {
            // Handle double click - could open property for editing
            OnPropertyCardDoubleClicked?.Invoke(sender, propertyData);
        }

        private void RefreshLayout()
        {
            cardContainer.Refresh();
            cardContainer.PerformLayout();
        }
        #endregion

        #region Public Events
        public event EventHandler<PropertyData> OnPropertyCardClicked;
        public event EventHandler<PropertyData> OnPropertyCardDoubleClicked;
        #endregion

        #region Cleanup
        public void Dispose()
        {
            try
            {
                ClearAllCards();
                cardContainer?.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error disposing PropertyCardManager: {ex.Message}");
            }
        }
        #endregion
    }
}
