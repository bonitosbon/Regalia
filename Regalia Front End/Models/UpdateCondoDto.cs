namespace Regalia_Front_End.Models
{
    public class UpdateCondoDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Amenities { get; set; } = string.Empty;
        public int MaxGuests { get; set; } = 4;
        public decimal PricePerNight { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
    }
}

