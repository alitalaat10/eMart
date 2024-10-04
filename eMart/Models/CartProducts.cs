using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Models
{
    public class CartProducts
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }

        public Cart? Cart { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product? Product { get; set; }


        [Required]
        public int Quantity { get; set; }


        [Required]
        public decimal Price { get; set; }



    }
}
