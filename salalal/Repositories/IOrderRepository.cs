using salalal.Models;
using System.Collections.Generic;

public interface IOrderRepository
{
    IEnumerable<Order> GetAllOrders();
    Order GetOrderById(int id);
    IEnumerable<Order> GetOrdersByUserId(int userId);
    IEnumerable<Order> GetPendingOrders();  // Za employee uzima ordere sa pending-om
    void AddOrder(Order order);
    void UpdateOrderStatus(int orderId, string status);  // Updejtuje order status Approved/Rejected
    void SaveChanges();
}
