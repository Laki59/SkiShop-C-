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

    public User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User GetUserByUsernameAndPassword(string username, string password)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }

    public bool UserExists(string username)
    {
        return _context.Users.Any(u => u.Username == username);
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        var dbUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (dbUser != null)
        {
            dbUser.Username = user.Username;
            dbUser.Role = user.Role;
            dbUser.Password = user.Password; // Should be hashed in real scenarios
            _context.SaveChanges();
        }
    }

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
