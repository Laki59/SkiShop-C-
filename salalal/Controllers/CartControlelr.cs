using Microsoft.AspNetCore.Mvc;
using salalal.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace salalal.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ISkiRepository _skiRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public CartController(ISkiRepository skiRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _skiRepository = skiRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("Cart") ?? new List<OrderItem>();
            ViewBag.CartCount = cart.Sum(item => item.Quantity);

            foreach (var item in cart)
            {
                item.Ski = _skiRepository.GetSkiById(item.SkiId);
            }

            return View(cart);
        }

        [Route("Cart/AddToCart/{skiId}")]
        public IActionResult AddToCart(int skiId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("Cart") ?? new List<OrderItem>();
            var ski = _skiRepository.GetSkiById(skiId);

            if (ski == null)
                return NotFound();

            var existingItem = cart.FirstOrDefault(i => i.SkiId == skiId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new OrderItem
                {
                    SkiId = ski.Id,
                    Quantity = 1,
                    Price = ski.Price
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("Cart");

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim);
                var order = new Order { UserId = userId, OrderDate = DateTime.Now };

                _orderRepository.AddOrder(order);

                foreach (var item in cart)
                {
                    var ski = _skiRepository.GetSkiById(item.SkiId);

                    if (ski != null && ski.StockQuantity >= item.Quantity)
                    {
                        ski.StockQuantity -= item.Quantity;
                        _skiRepository.UpdateSki(ski);

                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            SkiId = item.SkiId,
                            Quantity = item.Quantity,
                            Price = item.Price
                        };

                        _orderItemRepository.AddOrderItem(orderItem);
                    }
                    else
                    {
                        TempData["Error"] = $"Not enough stock for {ski.Name}";
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Ski");
        }
    }
}
