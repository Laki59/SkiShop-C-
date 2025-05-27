using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using salalal.Models;
using salalal.Repositories;
using System.Linq;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISkiRepository _skiRepository;

    public EmployeeController(IOrderRepository orderRepository, ISkiRepository skiRepository)
    {
        _orderRepository = orderRepository;
        _skiRepository = skiRepository;
    }

    // Uzimamo sve ordere sa PENDING na mestu statusa iz DB i salje na Employee/ManageOrders view
    public IActionResult ManageOrders()
    {
        var pendingOrders = _orderRepository.GetAllOrders().Where(o => o.Status == "Pending").ToList();
        return View("ManageOrders", pendingOrders);
    }

    // Po pritisku Approve dugmeta na Employee/ManageOrders view-u,ona menja status na Approve za id tog Ordera
    [HttpPost]
    public IActionResult ApproveOrder(int id)
    {
        var order = _orderRepository.GetOrderById(id);
        if (order == null) return NotFound();

        order.Status = "Approved";
        //Updajtuje order status
        _orderRepository.SaveChanges(); 

        return RedirectToAction("ManageOrders");
    }

    // Po pritisku Reject dugmeta na Employee/ManageOrders view-u,ono brise taj order,i vraca quantity skijama
    [HttpPost]
    public IActionResult RejectOrder(int id)
    {
        var order = _orderRepository.GetOrderById(id);
        if (order == null) return NotFound();

        // Vraca quantity
        foreach (var item in order.OrderItems)
        {
            var ski = _skiRepository.GetSkiById(item.SkiId);
            if (ski != null)
            {
                ski.StockQuantity += item.Quantity; 
                _skiRepository.UpdateSki(ski);
            }
        }

        // Brise order
        _orderRepository.DeleteOrder(id);
        _orderRepository.SaveChanges();

        return RedirectToAction("ManageOrders");
    }
}
