using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using salalal.Repositories;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IActionResult Index()
    {
        var orders = _orderRepository.GetAllOrders();
        return View(orders);
    }
}
