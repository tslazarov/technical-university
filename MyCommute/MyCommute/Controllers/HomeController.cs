using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCommute.Data.Contracts;
using MyCommute.Models;
using MyCommute.Services;

namespace MyCommute.Controllers
{
    public class HomeController : Controller
    {
        private IUsersService authenticationService;

        public HomeController(IUsersService authenticationService)
        {
            this.authenticationService = authenticationService;
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
