using salalal.Models;
using System.Collections.Generic;
using System.Linq;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    //Nalazi korisnika po ID-u u DB
    public User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    //Nalazi korisnike po imenu i sifri
    public User GetUserByUsernameAndPassword(string username, string password)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }

    //Proverava da li korisnik vec postoji
    public bool UserExists(string username)
    {
        return _context.Users.Any(u => u.Username == username);
    }

    //Dodaje novog korisnika i cuva ga u DB
    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    //Updejtuje korisnika 
    public void UpdateUser(User user)
    {
        var dbUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (dbUser != null)
        {
            dbUser.Username = user.Username;
            dbUser.Role = user.Role;
            dbUser.Password = user.Password; // Hesirati
            _context.SaveChanges();
        }
    }

    //Brise korisnika
    public void DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }
}
