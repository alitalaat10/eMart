// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using eMart.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using eMart.Repository.Base;
using eMart.Repository;
using Microsoft.IdentityModel.Tokens;
using eMart.DTO_Models;

namespace eMart.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<AppUser> _user;
        private readonly IUnitOfWork _unit;

        public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger,UserManager<AppUser> user,IUnitOfWork unit)
        {
            _signInManager = signInManager;
            _logger = logger;
            _user = user;
            _unit = unit;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            if (TempData["Id"] != null)
            {
                ViewData["Id"] = TempData["Id"];    
            }
            if (TempData["searchTerm"] != null)
            {
                ViewData["searchTerm"] = TempData["searchTerm"];
            }
          
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl, int? Id, string searchTerm)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var userId = _user.GetUserId(HttpContext.User);
                    var ismatched = _user.Users.Any(x => x.Id == userId);
                    if (ismatched)
                    {
                        var isolduser = _unit.carts.FindAll().Any(x => x.UserId == userId);
                        if (!isolduser)
                        {
                            Cart c = new Cart { UserId = userId };
                            _unit.carts.Addone(c);
                     
                        }
                        var cart = _unit.carts.selectone(x => x.UserId == userId);
                        if (cart != null)
                        {
                            var cartproducts = _unit.cartProducts.FindAll(nameof(CartProducts.Product)).Where(x => x.CartId == cart.Id);

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
                              
                                
                                    foreach (var cp in cartproducts)
                                    {
                                        var product = _unit.products.Find(cp.ProductId);
                                        if (cp.Quantity > product.Stock)
                                        {
                                            if (product.Stock == 0)
                                            {
                                                var C = HttpContext.Session.GetInt32("count") ?? 0;
                                                HttpContext.Session.SetInt32("count", C - cp.Quantity);
                                                _unit.cartProducts.DeleteOne(cp);
                                            }
                                            else
                                            {

                                                var C = HttpContext.Session.GetInt32("count") ?? 0;
                                                HttpContext.Session.SetInt32("count", C - (cp.Quantity - product.Stock));
                                                cp.Quantity = product.Stock;
                                                cp.Price = product.price * cp.Quantity;
                                                _unit.cartProducts.UpdateOne(cp);

                                            }
                                        }

                                    }
                                
                                var cartprods = _unit.cartProducts.FindAll(nameof(CartProducts.Product)).Where(x => x.CartId == cart.Id);
                                ViewData["Count"] = HttpContext.Session.GetInt32("count");

                            }
                        }
                    }
                  
                    
                        var user = await _user.FindByIdAsync(userId);
                        var userRoles = await _user.GetRolesAsync(user);
                        var isAdmin = userRoles.Any(x => x == "Admin");
                        var isAdminStr = isAdmin.ToString();
                        HttpContext.Session.SetString("isAdmin", isAdminStr);
                        ViewData["isAdmin"] = HttpContext.Session.GetString("isAdmin");


                        
                    
                    _logger.LogInformation("User logged in.");
                 
                    if (Id != null)
                    { if (searchTerm != null)
                        {
                            return RedirectToAction("AddToCart", "Cart", new { area = "",searchTerm, id = Id, returnUrl });
                        }
                       return RedirectToAction("AddToCart", "Cart", new {area="",id=Id,returnUrl });
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
