using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string? Name { get; set; }
        [Required]
        [DisplayName("Description")]
        public string? Description { get; set; }

        [Required]
        [Range(0d,1000000000d)]
        [DisplayName("price")]
        public  decimal price { get; set; }

        [Required]
        [DisplayName("Stock")]
        public int Stock { get; set; }

        public byte[]? Image { get; set; }


        [ForeignKey(nameof(subcategory))]
        [DisplayName("SubCategoryId")]
        public int SubCategoryId {  get; set; }

        public SubCategory? subcategory { get; set; }

        [ForeignKey(nameof(brand))]
        [DisplayName("BrandId")]
        public int BrandId { get; set; }

        public Brand? brand { get; set; }
        public List<OrderProducts>? Orders { get; set; }

        public List<CartProducts>? Carts { get; set; }

        public List<Review>? Reviews { get; set; }  

        [NotMapped]
        public IFormFile? ClientFile { get; set; }

        [NotMapped]
        public double Rate { get; set; }

        [NotMapped]
        public string? ImgSrc
        {
            get
            {
                if (Image != null)
                {
                    string base64String = Convert.ToBase64String(Image, 0, Image.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

    }
}
