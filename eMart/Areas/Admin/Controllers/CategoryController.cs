using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.InteropServices;

namespace eMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = clsRoles.RoleAdmin)]
    public class CategoryController : Controller
    {

        public CategoryController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        private readonly IUnitOfWork _unit;

        public async Task<IActionResult> Index()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            var categories = await _unit.categories.FindAllAsync(nameof(Category.SubCategories));
            return View(categories);
        }

        public IActionResult InsertorEdit(int? id)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            if (id == 0 || id == null)
            {

                return View();
            }
            
            var i = _unit.categories.Find((int)id);
        
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
        public IActionResult InsertorEdit(Category t)
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
                    TempData["success"] = $"Category: {t.Name} Is Added";
                    _unit.categories.Addone(t);
                }
                else
                {
                    var category = _unit.categories.Find(t.Id);
                    if (t.ClientFile != null && t.ClientFile.Length > 0)
                    {
                       
                        using (MemoryStream stream = new MemoryStream())
                        {
                            t.ClientFile.CopyTo(stream);
                            category.Image = stream.ToArray(); 
                        }
                    }
                    category.Name=t.Name;
                    TempData["success"] = $"Category: {category.Name} Is Updated";
                    _unit.categories.UpdateOne(category);
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
            
            var category = _unit.categories.Find(id);
            _unit.categories.DeleteOne(category);


            return RedirectToAction("Index");
        }
    }

}



