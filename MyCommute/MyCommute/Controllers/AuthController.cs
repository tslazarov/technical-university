using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyCommute.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MyCommute.Models;
using MyCommute.Models.Auth;

namespace MyCommute.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private IUsersService usersService;

        public AuthController(IUsersService usersService)
        {
            this.usersService = usersService;
        }


        [Route("signin")]
        public async Task<IActionResult> SignIn()
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (authResult.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            return View();
        }

        [Route("signin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                User user;
                if (await this.usersService.ValidateCredentials(out user, model.Email, model.Password, "Default"))
                {
                    await SignInUser(user, returnUrl);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [Route("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {
            var redirectUri = Url.Action("Profile");
            if (returnUrl != null)
            {
                redirectUri += "?returnUrl=" + returnUrl;
            }

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, provider);
        }

        [Route("signin/profile")]
        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("SignIn");
            }
            var user = await this.usersService.GetUserByIdentifier(authResult.Principal.FindFirst(ClaimTypes.Email).Value, authResult.Principal.Identity.AuthenticationType);
            if (user != null)
            {
                return await SignInUser(user, returnUrl);
            }

            var model = new ProfileViewModel();

            model.FirstName = authResult.Principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
            model.LastName = authResult.Principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

            return View(model);
        }

        [Route("signin/profile")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model, string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("SignIn");
            }

            if (ModelState.IsValid)
            {
                if(authResult.Principal.Identity.AuthenticationType == "Cookies")
                {
                    var user = await this.usersService.UpdateLocalUser(authResult.Principal.FindFirst(ClaimTypes.Email).Value, model.FirstName, model.LastName, "Default");
                    return await SignInUser(user, returnUrl);
                }
                else
                {
                    var user = await this.usersService.AddExternalUser(authResult.Principal.FindFirst(ClaimTypes.Email).Value, model.FirstName, model.LastName, authResult.Principal.Identity.AuthenticationType);
                    return await SignInUser(user, returnUrl);
                }
            }
            return View(model);
        }

        [Route("signup")]
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [Route("signup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model, string returnUrl = null)
        {
            var user = await this.usersService.GetUserByIdentifier(model.Email, "Default");

            if(user != null)
            {
                ModelState.AddModelError("Error", "Could not add user. Username already in use...");
            }

            if (ModelState.IsValid)
            {
                user = await this.usersService.AddLocalUser(model.Email, model.Password, "Default");
                return await SignInUserTemporary(model.Email);
            }
            return View(model);
        }

        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task<IActionResult> SignInUser(User user, string returnUrl = null)
        {
            await HttpContext.SignOutAsync("Temporary");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Name, string.Format("{0} {1}", user.FirstName, user.LastName)),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("provider", user.ProviderName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Redirect(returnUrl == null ? "/" : returnUrl);
        }

        public async Task<IActionResult> SignInUserTemporary(string email)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("name", email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", null);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("Temporary", principal);

            return RedirectToAction("Profile");

        }
    }
}