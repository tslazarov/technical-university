using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyCommute.Extensions;
using MyCommute.Services;

namespace MyCommute.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class HomeController : Controller
    {
        private readonly IUsersService authenticationService;
        private readonly IStringLocalizer<HomeController> localizer;

        public HomeController(IUsersService authenticationService, IStringLocalizer<HomeController> localizer)
        {
            this.authenticationService = authenticationService;
            this.localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("userinformation")]
        [Authorize]
        public IActionResult UserInformation()
        {
            var user = User.Identity;
            return View();
        }
    }
}
