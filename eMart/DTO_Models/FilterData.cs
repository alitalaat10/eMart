namespace eMart.DTO_Models
{
    public class FilterData
    {
        public bool InStock { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public double? MinRate { get; set; }
        public double? MaxRate { get; set; }

        public List<string>? SelectedBrands { get; set; }
        public List<string>? SelectedCategories { get; set; } 
    }
}
