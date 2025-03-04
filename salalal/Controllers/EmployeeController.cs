using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using salalal.Models;
using salalal.Repositories;
using System.Linq;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly IOrderRepository _orderRepository;

    public EmployeeController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IActionResult ManageOrders() //Uzima sve ordere koji imaju pending status
    {
        var pendingOrders = _orderRepository.GetPendingOrders();
        return View("ManageOrders", pendingOrders);
    }

    [HttpPost]
    public IActionResult ApproveOrder(int orderId) // Dugme approved menja status na approved
    {
        _orderRepository.UpdateOrderStatus(orderId, "Approved");
        return RedirectToAction("ManageOrders");
    }

    [HttpPost]
    public IActionResult RejectOrder(int orderId) // Ovo dugme na reject
    {
        _orderRepository.UpdateOrderStatus(orderId, "Rejected");
        return RedirectToAction("ManageOrders");
    }
}
