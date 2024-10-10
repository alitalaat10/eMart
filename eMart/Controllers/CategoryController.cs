using eMart.DTO_Models;
using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace eMart.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ILogger<HomeController> logger, IUnitOfWork unit)
        {
            _logger = logger;
            _unitOfWork = unit;
        }
        public async Task<IActionResult> Index(int id, int pg = 1)
        {
            var category = await _unitOfWork.categories.FindAsync(id);
            if (category == null)
            {

                return NotFound();
            }
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            ViewData["Title"] = category.Name;
            var categorysubCategories = _unitOfWork.subCategories.FindAll().Where(x => x.CategoryId == id);
   
            var products = _unitOfWork.products.FindAll(nameof(Product.Reviews)).Where(x => categorysubCategories.Any(c => c.Id == x.SubCategoryId));
           
            foreach (var product in products)
            {
                if (!product.Reviews.IsNullOrEmpty())
                {
                    var Rates = product.Reviews.Select(x => x.Rate);
                    product.Rate = Rates.Sum() / Rates.Count();
                }


            }

            ViewData["Id"] = id;
            const int pageSize =10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = products.Count();
            var Pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = products.Skip(recSkip).Take(Pager.PageSize).ToList();


            ViewBag.Pager = Pager;
            ViewBag.pgInCategory = Pager.CurrentPage;


            CategoryViewModel CVM = new CategoryViewModel
            {
                Products = new List<Product>(data),
                SubCategories = new List<SubCategory>(categorysubCategories)
            };
            return View(CVM);
        }
    }
}
