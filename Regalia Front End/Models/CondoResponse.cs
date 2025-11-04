namespace Regalia_Front_End.Models
{
    public class CondoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Amenities { get; set; } = string.Empty;
        public int MaxGuests { get; set; }
        public decimal PricePerNight { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string BookingLink { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string LastUpdated { get; set; } = string.Empty;
    }
}

