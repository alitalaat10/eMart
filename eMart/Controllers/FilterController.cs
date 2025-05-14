using eMart.DTO_Models;
using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace eMart.Controllers
{
    public class FilterController : Controller
    {
        private readonly IUnitOfWork _unit;

        public FilterController(IUnitOfWork unit) {

            _unit = unit;
        }
        private const string FilterSessionKey = "CurrentFilter";
        [HttpPost]
        public IActionResult FilteredData(FilterData? filter)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");

            HttpContext.Session.SetString(FilterSessionKey, JsonConvert.SerializeObject(filter));


            var products = _unit.products.FindAll(nameof(Product.subcategory), nameof(Product.brand), nameof(Product.Reviews));
            foreach (var product in products)
            {
                if (!product.Reviews.IsNullOrEmpty())
                {
                    var Rates = product.Reviews.Select(x => x.Rate);
                    product.Rate = Rates.Sum() / Rates.Count();
                }


            }
        
         
            if (filter.InStock)
                products = products.Where(p => p.Stock>0);

            if (filter.MinPrice.HasValue)
                products = products.Where(p => p.price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                products = products.Where(p => p.price <= filter.MaxPrice.Value);

            if (filter.MinRate.HasValue)
                products = products.Where(p => p.Rate >= filter.MinRate.Value);

            if (filter.MaxRate.HasValue)
                products = products.Where(p => p.Rate <= filter.MaxRate.Value);

            if (!filter.SelectedBrands.IsNullOrEmpty())
                products = products.Where(p => filter.SelectedBrands.Contains(p.brand.Name));

            if (!filter.SelectedCategories.IsNullOrEmpty())
                products = products.Where(p => filter.SelectedCategories.Contains(p.subcategory.Name));

            return View(products);
        }
        [HttpGet]
        public IActionResult FilteredData()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            FilterData filter = null;
            if (HttpContext.Session.GetString(FilterSessionKey) != null)
            {
                filter = JsonConvert.DeserializeObject<FilterData>(
                    HttpContext.Session.GetString(FilterSessionKey));

                // Get products and apply filter
                var products = _unit.products.FindAll(nameof(Product.subcategory), nameof(Product.brand), nameof(Product.Reviews));

                // Apply product review calculations
                foreach (var product in products)
                {
                    if (!product.Reviews.IsNullOrEmpty())
                    {
                        var Rates = product.Reviews.Select(x => x.Rate);
                        product.Rate = Rates.Sum() / Rates.Count();
                    }
                }

                if(filter.InStock)
                products = products.Where(p => p.Stock > 0);

                if (filter.MinPrice.HasValue)
                    products = products.Where(p => p.price >= filter.MinPrice.Value);

                if (filter.MaxPrice.HasValue)
                    products = products.Where(p => p.price <= filter.MaxPrice.Value);

                if (filter.MinRate.HasValue)
                    products = products.Where(p => p.Rate >= filter.MinRate.Value);

                if (filter.MaxRate.HasValue)
                    products = products.Where(p => p.Rate <= filter.MaxRate.Value);

                if (!filter.SelectedBrands.IsNullOrEmpty())
                    products = products.Where(p => filter.SelectedBrands.Contains(p.brand.Name));

                if (!filter.SelectedCategories.IsNullOrEmpty())
                    products = products.Where(p => filter.SelectedCategories.Contains(p.subcategory.Name));

                return View(products);
            }

            // If no filter in session, return all products
            var allProducts = _unit.products.FindAll(nameof(Product.subcategory), nameof(Product.brand), nameof(Product.Reviews));

            // Apply review calculations
            foreach (var product in allProducts)
            {
                if (!product.Reviews.IsNullOrEmpty())
                {
                    var Rates = product.Reviews.Select(x => x.Rate);
                    product.Rate = Rates.Sum() / Rates.Count();
                }
            }

            return View(allProducts);
        }
    }
}
