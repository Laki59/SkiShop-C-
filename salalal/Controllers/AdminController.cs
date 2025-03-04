using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using salalal.Models;
using System.Linq;
using System.Security.Claims;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ISkiRepository _skiRepository;
    private readonly IOrderRepository _orderRepository;

    public AdminController(IUserRepository userRepository, ISkiRepository skiRepository, IOrderRepository orderRepository)
    {
        _userRepository = userRepository;
        _skiRepository = skiRepository;
        _orderRepository = orderRepository;
    }

    private bool IsAdmin()
    {
        return User.IsInRole("Admin");
    }

    private IActionResult RedirectToHomeIfNotAdmin()
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }
        return null;
    }

    // -------------- USERS ----------------
    public IActionResult ManageUsers()
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var users = _userRepository.GetAllUsers();
        return View("ManageUsers", users);
    }

    [HttpGet]
    public IActionResult EditUser(int id)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var user = _userRepository.GetUserById(id);
        if (user == null) return NotFound();

        return View("EditUser", user);
    }

    [HttpPost]
    public IActionResult EditUser(User user)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        if (!ModelState.IsValid) return View(user);

        _userRepository.UpdateUser(user);
        return RedirectToAction("ManageUsers");
    }

    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        _userRepository.DeleteUser(id);
        return RedirectToAction("ManageUsers");
    }

    // -------------- SKIS ----------------
    public IActionResult ManageSkis()
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var skis = _skiRepository.GetAllSkis();
        return View("ManageSkis", skis);
    }

    [HttpGet]
    public IActionResult AddSki()
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        return View("AddSki");
    }

    [HttpPost]
    public IActionResult AddSki(Ski ski)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        if (ModelState.IsValid)
        {
            _skiRepository.AddSki(ski);
            _skiRepository.SaveChanges();
            return RedirectToAction("ManageSkis");
        }
        return View("AddSki", ski);
    }

    [HttpGet]
    public IActionResult EditSki(int id)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var ski = _skiRepository.GetSkiById(id);
        if (ski == null) return NotFound();

        return View("EditSki", ski);
    }

    [HttpPost]
    public IActionResult EditSki(Ski ski)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        if (ModelState.IsValid)
        {
            _skiRepository.UpdateSki(ski);
            _skiRepository.SaveChanges();
            return RedirectToAction("ManageSkis");
        }
        return View("EditSki", ski);
    }

    public IActionResult DeleteSki(int id)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        _skiRepository.DeleteSki(id);
        _skiRepository.SaveChanges();
        return RedirectToAction("ManageSkis");
    }

    public IActionResult AdjustStock(int id, int quantity)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var ski = _skiRepository.GetSkiById(id);
        if (ski == null) return NotFound();

        ski.StockQuantity += quantity;
        _skiRepository.UpdateSki(ski);
        _skiRepository.SaveChanges();

        return RedirectToAction("ManageSkis");
    }

    // -------------- Orderi ----------------
    public IActionResult ViewOrders()
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var orders = _orderRepository.GetAllOrders();
        return View("ViewOrders", orders);
    }
}
