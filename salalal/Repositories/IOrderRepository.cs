using salalal.Models;
using System.Collections.Generic;

public interface IOrderRepository
{
    IEnumerable<Order> GetAllOrders();
    Order GetOrderById(int id);
    IEnumerable<Order> GetOrdersByUserId(int userId);
    IEnumerable<Order> GetPendingOrders();  
    void AddOrder(Order order);
    void UpdateOrderStatus(int orderId, string status); 
    void DeleteOrder(int id); 
    void SaveChanges();
}
