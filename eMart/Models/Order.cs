using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace eMart.Models
{
    public class Order
    {
        [Key]
        [DisplayName("Id")]
        public int Id { get; set; }
        [Required]
        [DisplayName("TotalPrice")]
        public decimal TotalPrice { get; set; }
        [Required]
        [DisplayName("status")]
        public string? status {  get; set; }

        [Required]
        [DisplayName("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [DisplayName("DeliveryDate")]
        public DateTime DeliveryDate {  get; set; }
         
        [Required]
        [DisplayName("DeliveryLocation")]
        public string? DeliveryLocation { get; set; }

        [ForeignKey(nameof(user))]
        public string? UserId { get; set; } 

        public AppUser? user { get; set; }
        public List<OrderProducts>? Products { get; set; }


    }
}
