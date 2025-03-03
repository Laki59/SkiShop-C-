using Microsoft.AspNetCore.Mvc;

public class SkiController : Controller
{
    private readonly ISkiRepository _skiRepository;

    public SkiController(ISkiRepository skiRepository)
    {
        _skiRepository = skiRepository;
    }

    // Index action to display all skis
    public IActionResult Index()
    {
        var skis = _skiRepository.GetAllSkis();
        return View(skis);
    }

    // Details action to display a specific ski
    public IActionResult Details(int id)
    {
        var ski = _skiRepository.GetSkiById(id);
        if (ski == null)
        {
            return NotFound();  // Return a 404 if the ski is not found
        }

        return View(ski);  // Pass the specific ski to the view
    }
}
