using eMart.Models;
using System.ComponentModel.DataAnnotations;

namespace eMart.DTO_Models
{
    public class OrderDetails
    {
        public OrderDetails()
        { cartProducts = new List<CartProducts>(); }
        public  List<CartProducts> cartProducts {  get; set; }
        public decimal Total {  get; set; }

        public DateTime CreatedDate { get; set; }

 
        public DateTime DeliveryDate { get; set; }

        [Required]
        public string DeliveryLocation { get; set; }
    }
}
