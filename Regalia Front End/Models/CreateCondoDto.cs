namespace Regalia_Front_End.Models
{
    public class CreateCondoDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Amenities { get; set; } = string.Empty;
        public int MaxGuests { get; set; } = 4;
        public decimal PricePerNight { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string FrontDeskUsername { get; set; } = string.Empty;
        public string FrontDeskPassword { get; set; } = string.Empty;
    }
}

