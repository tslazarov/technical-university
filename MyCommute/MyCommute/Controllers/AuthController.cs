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
using MyCommute.Extensions.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using System.IO;
using MyCommute.Utilities;

namespace MyCommute.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class AuthController : Controller
    {
        private IUsersService usersService;
        private IImageHelper imageHelper;

        public AuthController(IUsersService usersService, IImageHelper imageHelper)
        {
            this.usersService = usersService;
            this.imageHelper = imageHelper;
        }

        public async Task<IActionResult> SignIn()
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (authResult.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            return View();
        }

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

        public IActionResult SignInExternal(string provider, string returnUrl = null)
        {
            var redirectUri = Url.Action("Profile");
            if (returnUrl != null)
            {
                redirectUri += "?returnUrl=" + returnUrl;
            }

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, provider);
        }

        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (!authResult.Succeeded)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(IFormFile image, ProfileViewModel model, string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("SignIn");
            }

            if (ModelState.IsValid)
            {
                User user = null;

                if (authResult.Principal.Identity.AuthenticationType == "Cookies")
                {
                    user = await this.usersService.UpdateLocalUser(authResult.Principal.FindFirst(ClaimTypes.Email).Value, model.FirstName, model.LastName, "Default");
                }
                else
                {
                    user = await this.usersService.AddExternalUser(authResult.Principal.FindFirst(ClaimTypes.Email).Value, model.FirstName, model.LastName, authResult.Principal.Identity.AuthenticationType);
                }

                var imagePath = this.imageHelper.UploadImage(image, user.Id.ToString());

                if (user != null && !string.IsNullOrEmpty(imagePath))
                {
                    this.usersService.UpdateImage(user, imagePath);
                };

                return await SignInUser(user, returnUrl);
            }
            return View(model);
        }

        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

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

        public async Task<IActionResult> SignOut()
        {
            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies[RatingNotificationsCookieName]))
            {
                HttpContext.Response.Cookies.Delete(RatingNotificationsCookieName);
            }

            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies[FriendNotificationsCookieName]))
            {
                HttpContext.Response.Cookies.Delete(FriendNotificationsCookieName);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task<IActionResult> SignInUser(User user, string returnUrl = null)
        {
            await HttpContext.SignOutAsync("Temporary");

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Append(RatingNotificationsCookieName, user.RatingNotifications.ToString(), option);
            Response.Cookies.Append(FriendNotificationsCookieName, user.FriendNotifications.ToString(), option);

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

        private const string RatingNotificationsCookieName = "ratingNotifications";
        private const string FriendNotificationsCookieName = "friendNotifications";
    }
}