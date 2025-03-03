public interface IOrderItemRepository
{
    IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId);
    void AddOrderItem(OrderItem orderItem);
}
