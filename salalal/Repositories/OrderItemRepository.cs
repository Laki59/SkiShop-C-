using Microsoft.EntityFrameworkCore;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext _context;

    public OrderItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId) =>
        _context.OrderItems.Where(oi => oi.OrderId == orderId).Include(oi => oi.Ski).ToList();

    public void AddOrderItem(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();
    }
}
