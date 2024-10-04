using eMart.DTO_Models;
using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Identity;

namespace eMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = clsRoles.RoleAdmin)]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unit;
        public OrdersController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public IActionResult Index()
        {
            List<OrderHandlingDTO> orderHandlingDTOs = new List<OrderHandlingDTO>();
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            var orders = _unit.orders.FindAll();
            foreach (var order in orders)
            { var orderProducts = _unit.orderProducts.FindAll(nameof(OrderProducts.product)).Where(x=>x.orderId==order.Id);

                orderHandlingDTOs.Add(new OrderHandlingDTO (order, orderProducts));
            }

            
            return View(orderHandlingDTOs);
        }

        public IActionResult UpdateOrder(int id)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");


            var i = _unit.orders.Find(id);

                selectStatus(i.status);
                return View(i);
            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrder(Order t)
        {
            if (ModelState.IsValid)
            {
                    TempData["success"] = $"Order {t.Id} has been Updated";
                    _unit.orders.UpdateOne(t);
                     return RedirectToAction("Index");

            }
           

                return View(t);
        }
        

        public IActionResult Delete(int id)
        {

            var order = _unit.orders.Find(id);
            _unit.orders.DeleteOne(order);


            return RedirectToAction("Index");
        }
        public void selectStatus(string? selectStatus = OrderStatus.Pending)
        {
            var lst = new List<string>()
            {  OrderStatus.Pending,
               OrderStatus.Processing,
               OrderStatus.Shipped,
               OrderStatus.Delivered };

            SelectList slst = new SelectList(lst, selectStatus);

            ViewBag.selectlist = slst;
        }
    }
}
