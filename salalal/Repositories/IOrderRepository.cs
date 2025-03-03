using salalal.Models;

public interface IOrderRepository
{
    IEnumerable<Order> GetAllOrders();
    Order GetOrderById(int id);
    IEnumerable<Order> GetOrdersByUserId(int userId);
    void AddOrder(Order order);
    void SaveChanges(); // Ensure database updates
}