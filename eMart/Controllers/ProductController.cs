using eMart.DTO_Models;
using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace eMart.Controllers
{
    [AllowAnonymous]
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _user;   
        public ProductController(ILogger<HomeController> logger, IUnitOfWork unit,UserManager<AppUser> user)
        {
            _logger = logger;
            _unitOfWork = unit;
            _user = user;
        }
    
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");

            var products = await _unitOfWork.products.FindAllAsync(nameof(Product.brand));
            var product = products.SingleOrDefault(x => x.Id == id);

            var reviews = await _unitOfWork.reviews.FindAllAsync(nameof(Review.User));
            reviews = reviews.Where(x => x.ProductId == id);

            var user = await _user.GetUserAsync(HttpContext.User);

            productDto productDto = new productDto(product, user);

            ViewData["Title"] = product.Name;
            if (!reviews.IsNullOrEmpty())
            {
                foreach (var review in reviews) 
                {
                    productDto.reviews.Add(review);
                }
            }



            return View(productDto);
        }

    }
}