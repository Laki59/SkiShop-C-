using salalal.Models;

public interface IUserRepository
{
    User GetUserById(int id);
    User GetUserByUsernameAndPassword(string username, string password);
    bool UserExists(string username);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
    IEnumerable<User> GetAllUsers();
}
