using salalal.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }  // Navigation property

    public int SkiId { get; set; }
    public Ski Ski { get; set; } // Navigation property

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
