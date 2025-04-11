using Microsoft.AspNetCore.Mvc;

public class SkiController : Controller
{
    private readonly ISkiRepository _skiRepository;

    public SkiController(ISkiRepository skiRepository)
    {
        _skiRepository = skiRepository;
    }

  
    public IActionResult Index()
    {
        var skis = _skiRepository.GetAllSkis();
        return View(skis);
    }


    public IActionResult Details(int id)
    {
        var ski = _skiRepository.GetSkiById(id);
        if (ski == null)
        {
            return NotFound();  
        }

        return View(ski);  
    }
}
