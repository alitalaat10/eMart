using eMart.Models;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = clsRoles.RoleAdmin)]
    public class ProductController : Controller
    {
        public ProductController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        private readonly IUnitOfWork _unit;

        public async Task<IActionResult> Index()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            var products = await _unit.products.FindAllAsync(nameof(Product.brand),nameof(Product.subcategory));
            return View(products);
        }

        public IActionResult InsertorEdit(int? id)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            if (id == 0 || id == null)
            {
                selectSubCategory();
                selectBrand();  
                return View();
            }

            var i = _unit.products.Find((int)id);

            if (i == null)
            {
                return BadRequest();
            }
            else
            {
                selectSubCategory(i.SubCategoryId);
                selectBrand(i.BrandId); 
                return View(i);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertorEdit(Product t)
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
                    TempData["success"] = $"Product: {t.Name} Is Added";
                    _unit.products.Addone(t);
                }
                else
                {
                    var product = _unit.products.Find(t.Id);
                    if (t.ClientFile != null && t.ClientFile.Length > 0)
                    {

                        using (MemoryStream stream = new MemoryStream())
                        {
                            t.ClientFile.CopyTo(stream);
                            product.Image = stream.ToArray();
                        }
                    }
                    product.Name= t.Name;
                    product.Description= t.Description; 
                    product.Stock = t.Stock;    
                    product.price = t.price;
                    product.SubCategoryId = t.SubCategoryId;    
                    product.BrandId = t.BrandId;
                    TempData["success"] = $"Product:{product.Name} Is Updated ";
                    _unit.products.UpdateOne(product);
                }

               
                return RedirectToAction("Index");

            }
            else
            {
                if (t.Id == 0)
                {
                    selectSubCategory();
                    selectBrand();
                }
                else
                {
                    selectSubCategory(t.SubCategoryId);
                    selectBrand(t.BrandId);
                }

                return View(t);
            }
        }

        public IActionResult Delete(int id)
        {

            var product = _unit.products.Find(id);
            _unit.products.DeleteOne(product);


            return RedirectToAction("Index");
        }

        public void selectSubCategory(int selectId = 1)
        {
            var lst = _unit.subCategories.FindAll();

            SelectList slst = new SelectList(lst, "Id", "Name", selectId);

            ViewBag.selectSubCategory = slst;
        }
        public void selectBrand(int selectId = 1)
        {
            var lst = _unit.brands.FindAll();

            SelectList slst = new SelectList(lst, "Id", "Name", selectId);

            ViewBag.selectBrand = slst;
        }
    }
}
