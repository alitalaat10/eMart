using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = clsRoles.RoleAdmin)]
    public class BrandController : Controller
    {
        public BrandController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        private readonly IUnitOfWork _unit;

        public async Task<IActionResult> Index()
        {

            var brands = await _unit.brands.FindAllAsync();
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            return View(brands);
        }

        public IActionResult InsertorEdit(int? id)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            if (id == 0 || id == null)
            {

                return View();
            }

            var i = _unit.brands.Find((int)id);

            if (i == null)
            {
                return BadRequest();
            }
            else
            {

                return View(i);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertorEdit(Brand t)
        {    

            if (t.Name.Contains("0") || t.Name.Contains("1") || t.Name.Contains("2") || t.Name.Contains("3") || t.Name.Contains("4") || t.Name.Contains("5") || t.Name.Contains("6") || t.Name.Contains("7") || t.Name.Contains("8") || t.Name.Contains("9"))
            {
                ModelState.AddModelError("Name", "Name can't contains number");
            }

            if (ModelState.IsValid)
            {

                if (t.Id == 0)
                {
                    if (t.ClientFile != null && t.ClientFile.Length > 0)
                    {

                        using (MemoryStream stream = new MemoryStream())
                        {
                            t.ClientFile.CopyTo(stream);
                            t.Image = stream.ToArray();
                        }
                    }
                    TempData["success"] = $"Brand: {t.Name} Is Added";
                    _unit.brands.Addone(t);
                }
                else
                {
                    var brand = _unit.brands.Find(t.Id);
                    if (t.ClientFile != null && t.ClientFile.Length > 0)
                    {

                        using (MemoryStream stream = new MemoryStream())
                        {
                            t.ClientFile.CopyTo(stream);
                            brand.Image = stream.ToArray();
                        }
                    }
                   brand.Name = t.Name;

                    TempData["success"] = $"Brand: {brand.Name} Is Updated";
                    _unit.brands.UpdateOne(brand);
                }

      
                return RedirectToAction("Index");

            }
            else
            {

                return View(t);
            }
        }

        public IActionResult Delete(int id)
        {

            var brand= _unit.brands.Find(id);
            _unit.brands.DeleteOne(brand);


            return RedirectToAction("Index");
        }
    }
}
