using eMart.Models;

namespace eMart.DTO_Models
{
    public class productDto
    { 
        public productDto(Product product,AppUser user)
        {
            this.product = product;
            this.user = user;
            reviews = new List<Review>(); 
          
        }
        public Product  product { get; set; }
        public List<Review> reviews { get; set; } 
        public AppUser user { get; set; }
      
    }
}
