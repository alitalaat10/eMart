using Microsoft.AspNetCore.Identity;

namespace eMart.Models
{
    public class AppUser : IdentityUser
    {  
        public string? Location { get; set; }

        public List<Order>? orders { get; set; }

        public List<Review>? Reviews { get; set; }

        public Cart? cart { get; set; }
    }
}
