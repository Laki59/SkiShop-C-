using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using salalal.Models;
using System.Linq;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ISkiRepository _skiRepository;

    public AdminController(IUserRepository userRepository, ISkiRepository skiRepository)
    {
        _userRepository = userRepository;
        _skiRepository = skiRepository;
    }

    // -------------- KORISNICI ----------------
    public IActionResult ManageUsers()
    {
        var users = _userRepository.GetAllUsers();
        return View("ManageUsers", users);
    }

    [HttpGet]
    public IActionResult EditUser(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null) return NotFound();

        return View("EditUser", user);
    }

    [HttpPost]
    public IActionResult EditUser(User user)
    {
        if (!ModelState.IsValid) return View(user);

        _userRepository.UpdateUser(user);
        return RedirectToAction("ManageUsers");
    }

    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        _userRepository.DeleteUser(id);
        return RedirectToAction("ManageUsers");
    }

    // -------------- SKIJE ----------------
    public IActionResult ManageSkis()
    {
        var skis = _skiRepository.GetAllSkis();
        return View("ManageSkis", skis);
    }

    [HttpGet]
    //Klikom na add ski dugme samo vraca view na koji treba ici
    public IActionResult AddSki()
    {
        return View("AddSki");
    }

    [HttpPost]
    // Upisuje nove skije koje smo ubacili i proverava da li su sva polja ok
    public IActionResult AddSki(Ski ski)
    {
        if (ModelState.IsValid)
        {
            _skiRepository.AddSki(ski);
            _skiRepository.SaveChanges();
            return RedirectToAction("ManageSkis");
        }
        return View("AddSki", ski);
    }

    [HttpGet]
    // Klikom na edit dugme sve informacije o skiji se ispisu
    public IActionResult EditSki(int id) 
    {
        var ski = _skiRepository.GetSkiById(id);
        if (ski == null) return NotFound();

        return View("EditSki", ski);
    }

    [HttpPost]
    // Ako nesto izmenimo,ono ce to i upisati
    public IActionResult EditSki(Ski ski)
    {
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
        _skiRepository.DeleteSki(id);
        _skiRepository.SaveChanges();
        return RedirectToAction("ManageSkis");
    }

    public IActionResult AdjustStock(int id, int quantity)
    {
        var ski = _skiRepository.GetSkiById(id);
        if (ski == null) return NotFound();

        ski.StockQuantity += quantity;
        _skiRepository.UpdateSki(ski);
        _skiRepository.SaveChanges();

        return RedirectToAction("ManageSkis");
    }
}
