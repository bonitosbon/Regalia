using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Regalia_Front_End.Owner_Dashboard;

namespace Regalia_Front_End
{
    public class PropertyStatusCardManager
    {
        #region Private Fields
        private Panel statusCardContainer;
        private List<PropertyStatusCard> statusCards;
        private Label totalPropertyLabel;
        private const int CARD_SPACING = 10;
        private const int CARD_HEIGHT = 80;
        #endregion

        #region Public Properties
        public int CardCount => statusCards.Count;
        #endregion

        #region Constructor
        public PropertyStatusCardManager(Panel container, Label totalLabel)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            if (totalLabel == null)
                throw new ArgumentNullException(nameof(totalLabel));

            statusCardContainer = container;
            totalPropertyLabel = totalLabel;
            statusCards = new List<PropertyStatusCard>();
            UpdateTotalPropertyCount();
        }
        #endregion

        #region Public Methods
        public void AddPropertyStatusCard(string unitName, string location, string status = "Available")
        {
            try
            {
                // Create new status card
                PropertyStatusCard newCard = new PropertyStatusCard();
                newCard.UnitName = unitName ?? "Unknown Unit";
                newCard.PropertyLocation = location ?? "Unknown Location";
                newCard.Status = status ?? "Available";
                
                // Set card size and styling
                newCard.Size = new Size(statusCardContainer.Width - 40, CARD_HEIGHT);
                newCard.Margin = new Padding(10, CARD_SPACING, 10, 0);
                // BackColor is set to Transparent in Designer, don't override it
                
                // Position card
                int yPosition = 60; // Start below "Available Properties" label
                yPosition += (statusCards.Count * (CARD_HEIGHT + CARD_SPACING));
                newCard.Location = new Point(10, yPosition);
                
                // Add to collection and container
                statusCards.Add(newCard);
                statusCardContainer.Controls.Add(newCard);
                
                // Update total count
                UpdateTotalPropertyCount();
                
                System.Diagnostics.Debug.WriteLine($"PropertyStatusCard added: {unitName} - {location}. Total cards: {statusCards.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding PropertyStatusCard: {ex.Message}");
                MessageBox.Show($"Error adding property status card: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RemovePropertyStatusCard(PropertyStatusCard card)
        {
            if (card == null) return;

            try
            {
                if (statusCards.Contains(card))
                {
                    statusCards.Remove(card);
                    statusCardContainer.Controls.Remove(card);
                    card.Dispose();
                    
                    // Reposition remaining cards
                    RepositionCards();
                    
                    // Update total count
                    UpdateTotalPropertyCount();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing PropertyStatusCard: {ex.Message}");
            }
        }

        public void RemovePropertyStatusCardByUnitName(string unitName)
        {
            if (string.IsNullOrEmpty(unitName)) return;

            try
            {
                PropertyStatusCard cardToRemove = null;
                foreach (var card in statusCards)
                {
                    if (card != null && card.UnitName == unitName)
                    {
                        cardToRemove = card;
                        break;
                    }
                }

                if (cardToRemove != null)
                {
                    RemovePropertyStatusCard(cardToRemove);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing PropertyStatusCard by unit name: {ex.Message}");
            }
        }

        public void ClearAllCards()
        {
            try
            {
                foreach (var card in statusCards)
                {
                    statusCardContainer.Controls.Remove(card);
                    card.Dispose();
                }
                statusCards.Clear();
                UpdateTotalPropertyCount();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing PropertyStatusCards: {ex.Message}");
            }
        }

        public void UpdatePropertyStatus(string unitName, string status)
        {
            if (string.IsNullOrEmpty(unitName)) return;

            try
            {
                foreach (var card in statusCards)
                {
                    if (card.UnitName == unitName)
                    {
                        card.Status = status;
                        System.Diagnostics.Debug.WriteLine($"PropertyStatusCard status updated: {unitName} - {status}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating PropertyStatusCard status: {ex.Message}");
            }
        }
        #endregion

        #region Private Methods
        private void RepositionCards()
        {
            int yPosition = 60; // Start below "Available Properties" label
            foreach (var card in statusCards)
            {
                card.Location = new Point(10, yPosition);
                yPosition += CARD_HEIGHT + CARD_SPACING;
            }
        }

        private void UpdateTotalPropertyCount()
        {
            if (totalPropertyLabel != null)
            {
                totalPropertyLabel.Text = statusCards.Count.ToString();
            }
        }
        #endregion
    }
}

