using System.Collections.Generic;

namespace salalal.Models
{
    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public List<Order> Orders { get; set; }
    }
}
