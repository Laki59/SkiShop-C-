using salalal.Models;

public class OrderItem
{
    // Order Item sluzi da za jedan order moze da se upise vise stvari u OrderItem,bez ponavljanja podataka u Order tabeli
    public int Id { get; set; }

    //Foreign key
    public int OrderId { get; set; }
    public Order Order { get; set; }  

    //Foreign key
    public int SkiId { get; set; }

    //Pristupa svim skijama
    public Ski Ski { get; set; } 

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
