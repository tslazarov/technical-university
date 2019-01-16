using Microsoft.AspNetCore.Mvc;
using MyCommute.Data.Contracts;
using MyCommute.Models;
using MyCommute.Models.Fuel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Controllers
{
    public class FuelController : Controller
    {
        private readonly IManager fuelsManager;

        public FuelController(IFuelsManager fuelsManager)
        {
            this.fuelsManager = fuelsManager as IManager;
        }

        public IActionResult Price(string fuelType)
        {
            if (!string.IsNullOrEmpty(fuelType))
            {
                var fuel = (this.fuelsManager.GetItems() as IEnumerable<Fuel>).FirstOrDefault(f => f.FuelType.ToLower() == fuelType);
                return Json(new { FuelPrice = fuel.FuelPrice  });
            }

            return null;
        }

        [HttpPost]
        public IActionResult Calculate(CalculateViewModel model)
        {
            var fuel = (this.fuelsManager.GetItems() as IEnumerable<Fuel>).FirstOrDefault(f => f.FuelType.ToLower() == model.FuelType);

            if(fuel != null && model.Distance > 0 && model.Consumption > 0 && model.Seats > 0)
            {
                model.FuelPrice = fuel.FuelPrice;
                model.TotalPrice = model.Distance * (model.Consumption / 100.0M) * model.FuelPrice;
                model.PricePerPerson = model.TotalPrice / model.Seats;

                return Json(model);
            }

            return null;
        }
    }
}
