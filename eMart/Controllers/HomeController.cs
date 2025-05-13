using eMart.Data;
using eMart.DTO_Models;
using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace eMart.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _user;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unit ,UserManager<AppUser> user)
        {
            _logger = logger;
            _unitOfWork = unit; 
                _user = user;
        }

        public async Task<IActionResult> Index(int pg =1)
        {
            HttpContext.Session.SetString("isAdmin", "False");
            
            HttpContext.Session.Remove("count");

            var userId = _user.GetUserId(HttpContext.User);
        
            var ismatched = _user.Users.Any(x => x.Id == userId);
            if (ismatched)
            {
                var user = await _user.FindByIdAsync(userId);
                var userRoles = await _user.GetRolesAsync(user);
                var isAdmin = userRoles.Any(x => x == "Admin");
                var isAdminStr = isAdmin.ToString();
                HttpContext.Session.SetString("isAdmin", isAdminStr);
                ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");


                var cart = _unitOfWork.carts.selectone(x => x.UserId == userId);
                if (cart != null)
                {
                    var cartproducts = _unitOfWork.cartProducts.FindAll().Where(x => x.CartId == cart.Id);

                    if (cartproducts.IsNullOrEmpty())
                    {
                        HttpContext.Session.SetInt32("count", 0);
                        ViewData["Count"] = 0;

                    }

                    else
                    {
                        int count = 0;
                        foreach (var product in cartproducts)
                        {

                            count += product.Quantity;
                        }
                        HttpContext.Session.SetInt32("count", count);
                        ViewData["Count"] = count;

                    }
                }
            }

           
                var products = await _unitOfWork.products.FindAllAsync(nameof(Product.Reviews));
            var categories = await _unitOfWork.categories.FindAllAsync();

            foreach (var product in products)
            {   if (!product.Reviews.IsNullOrEmpty())
                {
                    var Rates = product.Reviews.Select(x => x.Rate);
                    product.Rate = Rates.Sum()/Rates.Count();
                }
               
            
            }
            ViewBag.Brands = _unitOfWork.brands.FindAll().Select(b => b.Name);
            ViewBag.Categories = _unitOfWork.subCategories.FindAll().Select(c => c.Name);   


            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = products.Count();
            var Pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg-1)* pageSize;
            var data = products.Skip(recSkip).Take(Pager.PageSize).ToList();


            ViewBag.Pager = Pager;
            ViewBag.pgInHome = Pager.CurrentPage;

            HomeViewModel HVM = new HomeViewModel
            {
                Products = new List<Product>(data),
                Categorys = new List<Category>(categories)
            };

            return View(HVM);
        }

   
        

     
        public IActionResult Privacy()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
