using salalal.Models;

public class SkiRepository : ISkiRepository
{
    private readonly AppDbContext _context;

    public SkiRepository(AppDbContext context)
    {
        _context = context;
    }

    public Ski GetSkiById(int id)
    {
        return _context.Skis.FirstOrDefault(s => s.Id == id);
    }

    public IEnumerable<Ski> GetAllSkis()
    {
        return _context.Skis.ToList();
    }

    public void AddSki(Ski ski)
    {
        _context.Skis.Add(ski);
        SaveChanges(); // Commit immediately
    }

    public void UpdateSki(Ski ski)
    {
        var existingSki = _context.Skis.FirstOrDefault(s => s.Id == ski.Id);
        if (existingSki != null)
        {
            existingSki.Name = ski.Name;
            existingSki.Model = ski.Model;
            existingSki.Price = ski.Price;
            existingSki.StockQuantity = ski.StockQuantity;

            // Only update the image if a new one is provided
            if (!string.IsNullOrEmpty(ski.ImagePath))
            {
                existingSki.ImagePath = ski.ImagePath;
            }

            SaveChanges(); // Commit immediately
        }
    }

    public void DeleteSki(int id)
    {
        var ski = _context.Skis.FirstOrDefault(s => s.Id == id);
        if (ski != null)
        {
            _context.Skis.Remove(ski);
            SaveChanges(); // Commit immediately
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
