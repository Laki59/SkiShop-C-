using Microsoft.AspNetCore.Mvc;
using salalal.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace salalal.Controllers
{
    [Authorize]//Samo prijavljeni korisnici
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

        // Prikazuje sadržaj korpe
        public IActionResult Index()
        {
            // Učitavanje korpe iz sesije ili kreiranje nove prazne liste
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("Cart") ?? new List<OrderItem>();
            ViewBag.CartCount = cart.Sum(item => item.Quantity);

            foreach (var item in cart)
            {
                item.Ski = _skiRepository.GetSkiById(item.SkiId);
            }

            return View(cart);
        }

        // Dodaje proizvod u korpu
        [Route("Cart/AddToCart/{skiId}")]
        public IActionResult AddToCart(int skiId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("Cart") ?? new List<OrderItem>();
            // Dohvatanje skije iz baze podataka
            var ski = _skiRepository.GetSkiById(skiId);

            if (ski == null)
                return NotFound();

            // Provera da li ima dovoljno proizvoda na stanju
            if (ski.StockQuantity <= 0)
            {
                TempData["Error"] = $"Not enough stock for {ski.Name}";
                return RedirectToAction("Index");
            }

            var existingItem = cart.FirstOrDefault(i => i.SkiId == skiId);
            // Provera da li proizvod već postoji u korpi,ako da povecava za jedan,ako ne upisuje ga
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

            ski.StockQuantity--;  // Smanjuje količinu na stanju kada korisnik doda proizvod u korpu
            _skiRepository.UpdateSki(ski);

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int skiId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("Cart") ?? new List<OrderItem>();
            var itemToRemove = cart.FirstOrDefault(i => i.SkiId == skiId);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);

                // Vraća obrisanu količinu nazad na stanje
                var ski = _skiRepository.GetSkiById(skiId);
                if (ski != null)
                {
                    ski.StockQuantity += itemToRemove.Quantity;
                    _skiRepository.UpdateSki(ski);
                }

                // Ažurira sesiju sa novim podacima
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }


        public IActionResult Checkout()
        {
            // Dohvata podatke o korpi iz sesijee
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("Cart");

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }

            // Dohvata ID trenutno prijavljenog korisnika
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim);
                // Kreira novi objekat narudžbine
                var order = new Order
                {
                    UserId = userId, 
                    OrderDate = DateTime.Now 
                };

                _orderRepository.AddOrder(order);

                // Dodaje svaki proizvod iz korpe u narudžbinu
                foreach (var item in cart)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        SkiId = item.SkiId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };

                    _orderItemRepository.AddOrderItem(orderItem);
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
