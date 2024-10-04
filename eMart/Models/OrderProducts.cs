using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Models
{
    public class OrderProducts
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(order))]
        public int orderId { get; set; }

        public Order? order { get; set; }

        [ForeignKey(nameof(product))]
        public int ProductId { get; set; }

        public Product? product { get; set; }

        [Required]
        public int Quantity { get; set; }


        [Required]
        public decimal Price { get; set; }


    }
}
