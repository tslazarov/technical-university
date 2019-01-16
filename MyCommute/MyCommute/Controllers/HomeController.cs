using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyCommute.Data.Contracts;
using MyCommute.Extensions.Localization;
using MyCommute.Models;
using MyCommute.Models.Home;
using MyCommute.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyCommute.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class HomeController : Controller
    {
        private readonly IManager ridesManager;

        public HomeController(IRidesManager ridesManager)
        {
            this.ridesManager = ridesManager as IManager;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();

            model.Rides = (this.ridesManager.GetItems() as IEnumerable<Ride>).OrderByDescending(r => r.TravelDate)
                            .Take(RidesToDisplay)
                            .Select(r => new RideSummary()
                            {
                                FromCity = r.FromCity,
                                ToCity = r.ToCity,
                                Id = r.Id
                            });
                                                                           
            return View(model);
        }

        private const int RidesToDisplay = 5;
    }
}
