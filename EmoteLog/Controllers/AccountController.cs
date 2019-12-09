using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmoteLog.Models;
using System.Security.Claims;


namespace EmoteLog.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<EmoteLogUser> _userManager;
        private SignInManager<EmoteLogUser> _signManager;

        public AccountController(UserManager<EmoteLogUser> usrMgr, SignInManager<EmoteLogUser> signMgr)
        {
            _userManager = usrMgr;
            _signManager = signMgr;
        }

        private Task<EmoteLogUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        [AllowAnonymous]
        public ViewResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(NewUserModel model)
        {
            if (ModelState.IsValid)
            {
                EmoteLogUser user = new EmoteLogUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAccount = DateTime.Now
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                EmoteLogUser user = await _userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result =
                    await _signManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user or password");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                return RedirectToAction("MemberProfile", "Log", user);
            }
            return RedirectToAction("Logout");
        }
    }
}