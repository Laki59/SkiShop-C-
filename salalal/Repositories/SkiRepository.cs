using System.Collections.Generic;
using System.Linq;
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
        _context.Skis.Update(ski);
        SaveChanges(); // Commit immediately
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
