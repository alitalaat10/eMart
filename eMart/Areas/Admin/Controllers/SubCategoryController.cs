using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = clsRoles.RoleAdmin)]
    public class SubCategoryController : Controller
    {
        public SubCategoryController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        private readonly IUnitOfWork _unit;

        public async Task<IActionResult> Index()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            var subcategories = await _unit.subCategories.FindAllAsync("category");
            return View(subcategories);
        }

        public IActionResult InsertorEdit(int? id)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            if (id == 0 || id == null)
            {
                selectCategory();
                return View();
            }

            var i = _unit.subCategories.Find((int)id);

            if (i == null)
            {
                return BadRequest();
            }
            else
            {
                selectCategory(i.CategoryId);
                return View(i);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertorEdit(SubCategory t)
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
                    TempData["success"] = $"Sub Category: {t.Name} Is Added";
                    _unit.subCategories.Addone(t);
                }
                else
                {
                    var subcategory = _unit.subCategories.Find(t.Id);
                    if (t.ClientFile != null && t.ClientFile.Length > 0)
                    {

                        using (MemoryStream stream = new MemoryStream())
                        {
                            t.ClientFile.CopyTo(stream);
                            subcategory.Image = stream.ToArray();
                        }
                    }
                    subcategory.Name = t.Name;
                    subcategory.CategoryId = t.CategoryId;
                    TempData["success"] = $"Sub Category: {subcategory.Name} Is Updated";
                    _unit.subCategories.UpdateOne(subcategory);
                }

              
                return RedirectToAction("Index");

            }
            else
            {
                if (t.Id == 0)
                {
                    selectCategory();
                }
                else
                {
                    selectCategory(t.CategoryId);
                }

                return View(t);
            }
        }

        public IActionResult Delete(int id)
        {

            var subcategory = _unit.subCategories.Find(id);
            _unit.subCategories.DeleteOne(subcategory);


            return RedirectToAction("Index");
        }

        public void selectCategory(int selectId = 1)
        {
            var lst =  _unit.categories.FindAll();

            SelectList slst = new SelectList(lst, "Id", "Name", selectId);

            ViewBag.selectlist = slst;
        }
    }
}

