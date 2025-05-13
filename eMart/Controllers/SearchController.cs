using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace eMart.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unit;

       public SearchController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        
        public  IActionResult Result(string searchTerm,int pg = 1)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return View();
            }
            var products = _unit.products.FindAll(nameof(Product.Reviews)).Where(p => p.Name.ToUpper().Contains(searchTerm.ToUpper()) || p.Description.ToUpper().Contains(searchTerm.ToUpper()));
            foreach (var product in products)
            {
                if (!product.Reviews.IsNullOrEmpty())
                {
                    var Rates = product.Reviews.Select(x => x.Rate);
                    product.Rate = Rates.Sum() / Rates.Count();
                }
            

            }
            ViewData["searchTerm"] = searchTerm;

            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = products.Count();
            var Pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = products.Skip(recSkip).Take(Pager.PageSize).ToList();


            ViewBag.Pager = Pager;
            ViewBag.pgInResult = Pager.CurrentPage;
            return View(data);
          
        }
    }
}
