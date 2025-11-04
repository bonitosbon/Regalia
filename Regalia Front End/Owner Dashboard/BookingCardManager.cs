using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Regalia_Front_End.Models;
using Regalia_Front_End.Owner_Dashboard;

namespace Regalia_Front_End
{
    public class BookingCardManager
    {
        #region Private Fields
        private List<BookingControlCards> bookingCards;
        private BookingControl bookingControl;
        private const int CARD_SPACING = 15;
        #endregion
        
        #region Public Properties
        public FlowLayoutPanel cardContainer { get; private set; }
        public int CardCount => bookingCards.Count;
        public bool HasCards => bookingCards.Count > 0;
        public event EventHandler<BookingResponse> OnBookingCardClicked;
        #endregion

        #region Constructor
        public BookingCardManager(BookingControl bookingCtrl)
        {
            bookingControl = bookingCtrl ?? throw new ArgumentNullException(nameof(bookingCtrl));
            bookingCards = new List<BookingControlCards>();
            InitializeCardContainer();
        }
        #endregion

        #region Public Methods
        public void AddBookingCard(BookingResponse bookingData)
        {
            if (bookingData == null) 
                throw new ArgumentNullException(nameof(bookingData));

            try
            {
                // Create new booking card
                BookingControlCards newCard = CreateBookingCard(bookingData);
                
                // Ensure card is visible before adding
                newCard.Visible = true;
                newCard.Show();
                
                // Debug: Check if container exists and is valid
                if (cardContainer == null)
                {
                    throw new InvalidOperationException("Card container is not initialized!");
                }
                
                // Add to collection and container
                bookingCards.Add(newCard);
                cardContainer.Controls.Add(newCard);
                
                // CRITICAL: Ensure BookingControl is visible so cards can be seen
                if (!bookingControl.Visible)
                {
                    bookingControl.Visible = true;
                    bookingControl.Show();
                }
                
                // Ensure container is visible
                cardContainer.Visible = true;
                cardContainer.Show();
                
                // ALWAYS bring cardContainer to front
                cardContainer.BringToFront();
                
                // Set cardContainer properties to ensure it can receive mouse events
                cardContainer.Enabled = true;
                cardContainer.TabStop = false;
                
                // But bring the new card to front within its container
                newCard.BringToFront();
                
                // Refresh layout to ensure cards are displayed
                cardContainer.PerformLayout();
                cardContainer.Invalidate();
                cardContainer.Refresh();
                
                System.Diagnostics.Debug.WriteLine($"Added booking card: {bookingData.FullName}, Card visible: {newCard.Visible}, Container visible: {cardContainer.Visible}, Container width: {cardContainer.Width}, Card width: {newCard.Width}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR creating booking card: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error creating booking card: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RemoveBookingCard(BookingControlCards card)
        {
            if (card == null) return;

            try
            {
                // Remove from collection
                bookingCards.Remove(card);
                
                // Remove from container
                cardContainer.Controls.Remove(card);
                
                // Dispose the card
                card.Dispose();
                
                // Refresh layout
                RefreshLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing booking card: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RemoveBookingCardById(int bookingId)
        {
            try
            {
                // Find the card with matching booking ID
                var cardToRemove = bookingCards.FirstOrDefault(card => card.BookingData?.Id == bookingId);
                
                if (cardToRemove != null)
                {
                    RemoveBookingCard(cardToRemove);
                    System.Diagnostics.Debug.WriteLine($"Removed booking card with ID: {bookingId}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Booking card with ID {bookingId} not found");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing booking card by ID: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void ClearAllCards()
        {
            try
            {
                foreach (var card in bookingCards)
                {
                    card.Dispose();
                }
                bookingCards.Clear();
                cardContainer.Controls.Clear();
                RefreshLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing booking cards: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshLayout()
        {
            if (cardContainer != null)
            {
                cardContainer.PerformLayout();
                cardContainer.Invalidate();
            }
        }

        public void BringCardContainerToFront()
        {
            if (cardContainer != null)
            {
                cardContainer.BringToFront();
            }
        }
        #endregion

        #region Private Methods
        private void InitializeCardContainer()
        {
            // Create FlowLayoutPanel container for booking cards
            this.cardContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(CARD_SPACING),
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Visible = true,
                Enabled = true,
                TabStop = false,
                Cursor = Cursors.Default
            };
            
            // Add card container to BookingControl
            bookingControl.Controls.Add(cardContainer);
            
            // CRITICAL: Bring cardContainer to front IMMEDIATELY after adding
            cardContainer.BringToFront();
            
            // Also set TabIndex and ensure it can receive focus
            cardContainer.TabIndex = 1;
            cardContainer.TabStop = true;
            
            System.Diagnostics.Debug.WriteLine($"BookingCardContainer initialized - Added to BookingControl, brought to front. Controls count: {bookingControl.Controls.Count}");
        }

        private BookingControlCards CreateBookingCard(BookingResponse bookingData)
        {
            BookingControlCards newCard = new BookingControlCards(bookingData);
            
            // Ensure card is visible and properly sized
            newCard.Visible = true;
            
            // Make card wider to fill available space (full width minus margins)
            int cardWidth = 1157; // Default width
            if (cardContainer != null && cardContainer.Width > 100)
            {
                // Use container width minus padding (CARD_SPACING is padding on both sides = * 2)
                cardWidth = cardContainer.Width - (CARD_SPACING * 2);
            }
            
            // Ensure reasonable width bounds
            cardWidth = Math.Max(500, Math.Min(cardWidth, 1157));
            
            // Set size - FlowLayoutPanel works best with fixed-size controls
            newCard.Size = new Size(cardWidth, 95);
            newCard.Margin = new Padding(CARD_SPACING / 2);
            
            // Don't use anchors in FlowLayoutPanel - it controls the layout
            newCard.Anchor = AnchorStyles.None;
            
            // Wire up events
            newCard.OnCardClicked += BookingCard_OnCardClicked;
            
            System.Diagnostics.Debug.WriteLine($"Created booking card for {bookingData?.FullName} - Width: {cardWidth}, Container Width: {cardContainer?.Width ?? 0}");
            
            return newCard;
        }

        private void BookingCard_OnCardClicked(object sender, BookingResponse bookingData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"BookingCard_OnCardClicked in BookingCardManager - Booking: {bookingData?.FullName}");
                OnBookingCardClicked?.Invoke(sender, bookingData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in BookingCard_OnCardClicked: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error in booking card manager: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}

