using salalal.Models;

public interface ISkiRepository
{
    Ski GetSkiById(int id);
    IEnumerable<Ski> GetAllSkis();
    void AddSki(Ski ski);
    void UpdateSki(Ski ski);
    void DeleteSki(int id);
    void SaveChanges(); 
}
