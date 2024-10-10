using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace eMart.Controllers
{
    
    [Route("[controller]")]
    [AllowAnonymous]
    public class LanguageController : ControllerBase
    {
        private readonly IStringLocalizer<LanguageController> _localizer;

        public LanguageController(IStringLocalizer<LanguageController> localizer)
        {
            _localizer = localizer;
        }



        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl,string userId,int? pgInHome, int? pgInSubCategory, int? pgInCategory, int id)
        {
            Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            if (userId != null)
            {
                return RedirectToAction("AddRoles", "Roles", new { area = "Admin", userId });
            }
            if (pgInHome != null)
            {
                return RedirectToAction("Index", "Home", new {  pg = pgInHome });
            }
            if (pgInCategory != null)
            {
                return RedirectToAction("Index", "Category", new {  id,pg = pgInCategory });
            }
            if (pgInSubCategory != null)
            {
                return RedirectToAction("Index", "SubCategory", new { id, pg = pgInSubCategory });
            }
           
            
            return LocalRedirect(returnUrl);
        }


    }
}
