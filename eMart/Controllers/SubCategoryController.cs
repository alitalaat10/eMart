using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            var products = _unitOfWork.products.FindAll(nameof(Product.Reviews));
            var subproducts = products.Where(x => x.SubCategoryId == id);

            foreach (var product in subproducts)
            {
                if (!product.Reviews.IsNullOrEmpty())
                {
                    var Rates = product.Reviews.Select(x => x.Rate);
                    product.Rate = Rates.Sum() / Rates.Count();
                }


            }

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
