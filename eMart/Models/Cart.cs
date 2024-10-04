using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        public AppUser? User { get; set; }

      public List<CartProducts>? Products { get; set; }  
    }
}
