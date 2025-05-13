using Stripe.Checkout;
using eMart.DTO_Models;
using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.AspNetCore.Authorization;

namespace eMart.Controllers
{
    [Authorize(Roles = clsRoles.RoleUser + "," + clsRoles.RoleAdmin)]
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unit;
        private readonly UserManager<AppUser> _user;
        public OrderController(ILogger<HomeController> logger, UserManager<AppUser> user ,IUnitOfWork unit)
        {
            _logger = logger;
            _user = user;
            _unit = unit;
        }
        public async Task<IActionResult> Index()
        {
            OrderDetails details= new OrderDetails();
          
           var user=  await _user.GetUserAsync(HttpContext.User);
           
           var cart = _unit.carts.selectone(x => x.UserId == user.Id);
                var cartproducts = _unit.cartProducts.FindAll(nameof(CartProducts.Product)).Where(x => x.CartId == cart.Id);
            
            foreach (var product in cartproducts)
            {
                details.cartProducts.Add(product);

                details.Total += product.Price;
            }
           
            details.DeliveryLocation = user.Location;
            details.CreatedDate = DateTime.Now;
            details.DeliveryDate = DateTime.Now.AddDays(2);
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            return View(details);
            
          
        }

        public async Task<IActionResult> AddOrder(OrderDetails od)
        {

                var user = await _user.GetUserAsync(HttpContext.User);
            user.Location = od.DeliveryLocation;
           await  _user.UpdateAsync(user);

                var cart = _unit.carts.selectone(x => x.UserId == user.Id);
                var cartproducts = _unit.cartProducts.FindAll(nameof(CartProducts.Product)).Where(x => x.CartId == cart.Id);

                var domain = "https://aliemart.runasp.net/";

                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"Order/OrderConfirmation",
                    CancelUrl = domain + $"Order/Index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    CustomerEmail = user.Email.ToString()

                };
                foreach (var p in cartproducts)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)p.Price,
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = p.Product.Name
                            }
                        },
                        Quantity = p.Quantity,
                    };
                    options.LineItems.Add(sessionListItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                TempData["Session"] = session.Id;

                Response.Headers.Add("Location", session.Url);
            
           
            return new StatusCodeResult(303);
        }



        public async Task<IActionResult> OrderConfirmation()
        { 
          
            var service = new SessionService();
           Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                
                var user = await _user.GetUserAsync(HttpContext.User);

                var cart = _unit.carts.selectone(x => x.UserId == user.Id);
                var cartproducts = _unit.cartProducts.FindAll(nameof(CartProducts.Product)).Where(x => x.CartId == cart.Id);
               
                Order order = new Order();
                List<OrderProducts> orderProducts = new List<OrderProducts>();  
                order.UserId = user.Id;
                order.status = OrderStatus.Pending;
                order.CreatedDate = DateTime.Now;
                order.DeliveryDate = DateTime.Now.AddDays(2);
                order.DeliveryLocation = user.Location;
                foreach (var product in cartproducts)
                {
                   
                    order.TotalPrice += product.Price;
                }

                _unit.orders.Addone(order);

                var dborder = _unit.orders.selectLast(x => x.UserId == user.Id);
                foreach (var cartproduct in cartproducts)
                {
                    orderProducts.Add(new OrderProducts { orderId=dborder.Id, ProductId=cartproduct.ProductId,Quantity=cartproduct.Quantity,Price=cartproduct.Price });
                    var product = _unit.products.Find(cartproduct.ProductId);
                    product.Stock -= cartproduct.Quantity;
                    _unit.products.UpdateOne(product);
                   
                }


                _unit.cartProducts.DeleteMany(cartproducts);
                
                _unit.orderProducts.AddMany(orderProducts);
                HttpContext.Session.SetInt32("count", 0);
                TempData["Success"] = "The products were Orderd Successfully";
               
                return RedirectToAction("Index","Cart");
            }
            return RedirectToAction(nameof(AddOrder));

           
        }
        
    }
}
