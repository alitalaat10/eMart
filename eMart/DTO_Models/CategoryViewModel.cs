using eMart.Models;

namespace eMart.DTO_Models
{
    public class CategoryViewModel
    {
        public List<Product>? Products { get; set; }

        public List<SubCategory>? SubCategories { get; set; }
    }
}
