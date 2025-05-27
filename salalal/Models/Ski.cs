namespace salalal.Models
{
    public class Ski
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public string? ImagePath { get; set; }
    }
}
