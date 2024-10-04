using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;


namespace eMart.Controllers
{

    public class CartController : Controller
    {

        private readonly UserManager<AppUser> _user;
        private readonly IUnitOfWork _unit;


        public CartController(UserManager<AppUser> user, IUnitOfWork unit)
        {
            _user = user;
            _unit = unit;
        }
        public IActionResult Index()
        {

            var userId = _user.GetUserId(HttpContext.User);
            var ismatched = _user.Users.Any(x => x.Id == userId);
            if (ismatched)
            {
                ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
                var cart = _unit.carts.selectone(x => x.UserId == userId);


                if (HttpContext.Session.GetInt32("count") == 0)
                {
                    ViewData["Count"] = 0;
                    return View(null);
                }

                else
                {
                    var cartproducts = _unit.cartProducts.FindAll(nameof(CartProducts.Product)).Where(x => x.CartId == cart.Id);
                    ViewData["Count"] = HttpContext.Session.GetInt32("count");
                    return View(cartproducts);
                }
            }
           


            return RedirectToAction(nameof(NotUser));
     

        }
        public IActionResult AddToCart(int id, string returnUrl)
        {
            var userId = _user.GetUserId(HttpContext.User);
            var ismatched = _user.Users.Any(x => x.Id == userId);
            if (ismatched)
            {
                var cart = _unit.carts.selectone(x => x.UserId == userId);

                var product = _unit.products.Find(id);


                if (product != null && product.Stock != 0)
                {
                    var cartproducts = _unit.cartProducts.FindAll(nameof(CartProducts.Product)).Where(x => x.CartId == cart.Id);

                    var iscartproduct = cartproducts.Any(x => x.ProductId == id);

                    if (iscartproduct)
                    {
                        var cartproduct = cartproducts.FirstOrDefault(x => x.ProductId == id);
                        if (cartproduct.Quantity < product.Stock)
                        {
                            TempData["success"] = $"Product: {cartproduct.Product.Name} Is Added";
                            cartproduct.Quantity += 1;
                            var c = HttpContext.Session.GetInt32("count") ?? 0;
                            HttpContext.Session.SetInt32("count", c + 1);
                            cartproduct.Price = product.price * cartproduct.Quantity;
                            _unit.cartProducts.UpdateOne(cartproduct);
                        }
                        else
                        {
                            TempData["maxQuantity"] = $"the seller only has {product.Stock} pieces of this product";
                        }



                    }
                    else
                    {
                        var c = HttpContext.Session.GetInt32("count");
                        HttpContext.Session.SetInt32("count", (c + 1) ?? 1);
                        TempData["success"] = $"Product: {product.Name} Is Added";
                        _unit.cartProducts.Addone(new CartProducts { CartId = cart.Id, ProductId = product.Id, Price = product.price, Quantity = 1 });

                    }

                }
                else
                {
                    TempData["maxQuantity"] = $"the seller has {product.Stock} pieces of this product";
                }

                return LocalRedirect(returnUrl);

            }
            else
            {
                TempData["Id"] = id;
                return RedirectToPage("/Account/Login", new { area = "Identity" , returnUrl});
            }


        }

        public IActionResult RemoveQuantity(int id, string returnUrl)
        {
            var userId = _user.GetUserId(HttpContext.User);
            var ismatched = _user.Users.Any(x => x.Id == userId);
            if (ismatched)
            {
                var cart = _unit.carts.selectone(x => x.UserId == userId);

                var product = _unit.products.Find(id);


                if (product != null)
                {
                    var cartproducts = _unit.cartProducts.FindAll().Where(x => x.CartId == cart.Id);
                    var cartproduct = cartproducts.FirstOrDefault(x => x.ProductId == id);
                    if (cartproduct.Quantity >1 )
                    {
                      
                        cartproduct.Quantity -= 1;
                        var c = HttpContext.Session.GetInt32("count") ?? 0;
                        HttpContext.Session.SetInt32("count", c-1);
                        cartproduct.Price = product.price * cartproduct.Quantity;
                        _unit.cartProducts.UpdateOne(cartproduct);
                        
                    }

                    else
                    {
                        var c = HttpContext.Session.GetInt32("count") ?? 0;
                        HttpContext.Session.SetInt32("count", c - 1);
                        _unit.cartProducts.DeleteOne(cartproduct);

                    }
                    TempData["success"] = $"Product: {cartproduct.Product.Name} Is Removed";
                }
            
            }

    
            return LocalRedirect(returnUrl);
        }
        public IActionResult Delete(int id)
        {
            var userId = _user.GetUserId(HttpContext.User);
            var isUser = _user.Users.Any(x => x.Id == userId);
            if (isUser)
            {
                var cart = _unit.carts.selectone(x => x.UserId == userId);

                var product = _unit.products.Find(id);


                if (product != null)
                {
                    var cartproducts = _unit.cartProducts.FindAll().Where(x => x.CartId == cart.Id);
                    var cartproduct = cartproducts.FirstOrDefault(x => x.ProductId == id);

                    var c = HttpContext.Session.GetInt32("count") ?? 0;
                    HttpContext.Session.SetInt32("count", c - cartproduct.Quantity);

                    _unit.cartProducts.DeleteOne(cartproduct);

                   
                }

            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult NotUser()
        {
            return View();
        }
        
    }
}
