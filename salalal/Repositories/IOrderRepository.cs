using salalal.Models;
using System.Collections.Generic;

public interface IOrderRepository
{
    IEnumerable<Order> GetAllOrders();
    Order GetOrderById(int id);
    IEnumerable<Order> GetOrdersByUserId(int userId);
    IEnumerable<Order> GetPendingOrders();  // For employees to view pending orders
    void AddOrder(Order order);
    void UpdateOrderStatus(int orderId, string status);  // Updates order status to Approved/Rejected
    void DeleteOrder(int id);  // Deletes an order when rejected
    void SaveChanges();
}
