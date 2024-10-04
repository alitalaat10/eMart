using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required, Range(0, 5)]
        public int Rate { get; set; }
        public string? Comment { get; set;}


        [ForeignKey(nameof(User)),Required]
        public string? UserId { get; set; } 
        public AppUser? User { get; set; }   


        [ForeignKey(nameof(Product))]
        public int ProductId {  get; set; }  
        public Product? Product { get; set; }



    }
}
