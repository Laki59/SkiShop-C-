using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using salalal.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;

    public AccountController(IUserRepository userRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        var dbUser = _userRepository.GetUserByUsernameAndPassword(user.Username, user.Password);

        if (dbUser != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dbUser.Username),
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                new Claim(ClaimTypes.Role, dbUser.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Ski");
        }

        ViewBag.Error = "Invalid credentials";
        return View();
    }

    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (_userRepository.UserExists(user.Username))
        {
            ViewBag.Error = "Username already exists";
            return View();
        }

        _userRepository.AddUser(user);
        return RedirectToAction("Login");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return RedirectToAction("Login");

        int userId = int.Parse(userIdClaim);
        var user = _userRepository.GetUserById(userId);
        if (user == null) return RedirectToAction("Login");

        var orders = _orderRepository.GetOrdersByUserId(userId); // Fetch orders

        var model = new UserProfileViewModel
        {
            UserName = user.Username,
            Role = user.Role,
            Password = user.Password,
            Orders = (List<Order>)orders
        };

        return View(model);
    }

}
