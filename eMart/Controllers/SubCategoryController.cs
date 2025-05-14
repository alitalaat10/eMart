using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Mail;

namespace eMart.Controllers
{
    [AllowAnonymous]
    public class SubCategoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SubCategoryController(ILogger<HomeController> logger, IUnitOfWork unit)
        {
            _logger = logger;
            _unitOfWork = unit;
        }
        public async Task<IActionResult> Index(int id , int pg = 1)
        {
            var subcategory = await _unitOfWork.subCategories.FindAsync(id);
            if (subcategory == null)
            {

                return NotFound();
            }
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            ViewData["Title"] = subcategory.Name;
            var products = _unitOfWork.products.FindAll(nameof(Product.Reviews),nameof(Product.brand));
            var subproducts = products.Where(x => x.SubCategoryId == id);
            List<Brand> brands = new List<Brand>();
            foreach (var product in subproducts)
            {
                brands.Add(product.brand);
                if (!product.Reviews.IsNullOrEmpty())
                {
                    var Rates = product.Reviews.Select(x => x.Rate);
                    product.Rate = Rates.Sum() / Rates.Count();
                }


            }
            List<string> subCategories = new List<string>();
            subCategories.Add(subcategory.Name);
            ViewBag.Brands = brands.Select(b => b.Name).Distinct().ToList();
            ViewBag.Categories = subCategories;

            ViewData["Id"] = id;
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = subproducts.Count();
            var Pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = subproducts.Skip(recSkip).Take(Pager.PageSize).ToList();


            ViewBag.Pager = Pager;
            ViewBag.pgInSubCategory = Pager.CurrentPage;
            return View(data);
        }
    }
}
