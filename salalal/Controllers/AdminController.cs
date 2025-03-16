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
        //Sve podatke o userima i salje na ManageUsers view
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var users = _userRepository.GetAllUsers();
        return View("ManageUsers", users);
    }

    [HttpGet]
    public IActionResult EditUser(int id)
    {
        //Prikazuje formu za izmenu na osnovu ID-a user-a
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var user = _userRepository.GetUserById(id);
        if (user == null) return NotFound();

        return View("EditUser", user);
    }

    [HttpPost]
    //Cuva gornje izmene
    public IActionResult EditUser(User user)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        if (!ModelState.IsValid) return View(user);

        _userRepository.UpdateUser(user);
        return RedirectToAction("ManageUsers");
    }

    [HttpPost]
    //Brise korisnika po ID-u
    public IActionResult DeleteUser(int id)
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        _userRepository.DeleteUser(id);
        return RedirectToAction("ManageUsers");
    }

    // -------------- SKIS ----------------
    
    //Isto kao ManageUser samo za skije
    public IActionResult ManageSkis()
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var skis = _skiRepository.GetAllSkis();
        return View("ManageSkis", skis);
    }

    [HttpGet]
    //Prikazuje formu za dodavanje
    public IActionResult AddSki()
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        return View("AddSki");
    }

    [HttpPost]
    //Dodaje novu skiju ako je valdina
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
    //Edit isti za skije kao i za user-a
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

    //Menja kolicinu stanja
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

    //Prikaz svih porudzbina
    public IActionResult ViewOrders()
    {
        var redirect = RedirectToHomeIfNotAdmin();
        if (redirect != null) return redirect;

        var orders = _orderRepository.GetAllOrders();
        return View("ViewOrders", orders);
    }
}
