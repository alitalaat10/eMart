using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Name")]
        public string? Name { get; set; }

        [ForeignKey(nameof(category))]
        [DisplayName("Category")]
        public int CategoryId { get; set; } 

        public Category? category { get; set; }

        public List<Product>? products { get; set; }

        public byte[]? Image { get; set; }

        [NotMapped]
        public IFormFile? ClientFile { get; set; }


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
