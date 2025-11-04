using System;

namespace Regalia_Front_End.Models
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public int GuestCount { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string QrCodeData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? CheckedInAt { get; set; }
        public DateTime? CheckedOutAt { get; set; }
        public string Notes { get; set; }
        public string RejectionReason { get; set; }
        public string PaymentImageUrl { get; set; }
        public CondoSummary Condo { get; set; } = new CondoSummary();
    }
    
    public class CondoSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ImageUrl { get; set; }
    }
}

