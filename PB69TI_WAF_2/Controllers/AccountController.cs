using Cinema.Persistence;
using Cinema.Web.Models;
using ELTE.Cinema.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELTE.Cinema.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,  SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) //Megjegyezzük az oldalt ahonnan érkeztünk
        {
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false); //1.false:maradjunk-e bejelentkezve a böngésző bezárása után? 2.false:zárjuk ki magunkat ha sikertelen volt?
                if(result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Bejelentkezés sikertelen volt");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null) //Megjegyezzük az oldalt ahonnan érkeztünk
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName };// Létrehozunk egy új usert
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "Sikertele regisztáció");
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(String returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) //Ha saját urljeink közül mutat valahová, akkor ide megyünk
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(ListsController.Index), "Lists");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(ListsController.Index), "Lists");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
