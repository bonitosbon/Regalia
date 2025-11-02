using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Regalia_Front_End
{
    public class PropertyCardManager
    {
        #region Private Fields
        private FlowLayoutPanel cardContainer;
        private List<PropertyCard> propertyCards;
        private PropertiesControl propertiesControl;
        private const int CARDS_PER_ROW = 3;
        private const int CARD_SPACING = 25;
        #endregion

        #region Public Properties
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
                
                // Keep container behind addProperties panels, but ensure it's visible
                // Cards should stay behind the form overlays
                // Make sure containerPanel (which holds addProperties) doesn't cover cards
                if (propertiesControl.containerPanel != null && propertiesControl.containerPanel.Visible)
                {
                    // If containerPanel is visible, cards will be behind it (which is correct for overlays)
                    // But we want cards visible when no overlay is shown
                    cardContainer.SendToBack();
                }
                
                // But bring the new card to front within its container
                newCard.BringToFront();
                
                // Refresh layout
                RefreshLayout();
                cardContainer.Invalidate();
                cardContainer.Update();
                newCard.Invalidate();
                newCard.Update();
                
                // Debug output
                System.Diagnostics.Debug.WriteLine($"Card added: {propertyData.Title}, Container controls: {cardContainer.Controls.Count}, Card visible: {newCard.Visible}, Card size: {newCard.Size}");
                System.Diagnostics.Debug.WriteLine($"PropertiesControl visible: {propertiesControl.Visible}, CardContainer visible: {cardContainer.Visible}, CardContainer parent: {cardContainer.Parent?.Name}");
                
                // Force a refresh to make sure the card appears
                propertiesControl.Refresh();
                cardContainer.Refresh();
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
        #endregion

        #region Private Methods
        private void InitializeCardContainer()
        {
            // Restore original FlowLayoutPanel container
            cardContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(CARD_SPACING),
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Visible = true
            };

            // Add card container to PropertiesControl
            // Make sure it's behind the containerPanel (which has addProperties forms)
            propertiesControl.Controls.Add(cardContainer);
            
            // Send to back so cards appear behind addProperties panels
            // But make sure containerPanel stays behind cards by ensuring proper Z-order
            cardContainer.SendToBack();
            
            System.Diagnostics.Debug.WriteLine($"CardContainer initialized and added to PropertiesControl. Controls count: {propertiesControl.Controls.Count}");
        }

        private PropertyCard CreatePropertyCard(PropertyData propertyData)
        {
            PropertyCard newCard = new PropertyCard(propertyData);
            
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
            // Handle single click - could show property details
            OnPropertyCardClicked?.Invoke(sender, propertyData);
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
