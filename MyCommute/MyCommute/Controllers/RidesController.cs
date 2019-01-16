using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCommute.Data.Contracts;
using MyCommute.Extensions.Localization;
using MyCommute.Models;
using MyCommute.Models.Rides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCommute.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class RidesController : Controller
    {
        private const int pageSize = 10;
        private readonly IManager ridesManager;
        private readonly IManager usersManager;
        private readonly IManager ridesUsersManager;

        public RidesController(IRidesManager ridesManager, IUsersManager usersManager, IRidesUsersManager ridesUsersManager)
        {
            this.ridesManager = ridesManager as IManager;
            this.usersManager = usersManager as IManager;
            this.ridesUsersManager = ridesUsersManager as IManager;
        }

        [Authorize]
        public ActionResult Detail(Guid id)
        {
            if(id == Guid.Empty)
            {
                return StatusCode(404);
            }

            var ride = this.ridesManager.GetItem(id) as Ride;

            var viewModel = new RideDetailViewModel();

            if (ride != null)
            {
                var rideUsers = (this.ridesUsersManager.GetItems() as IEnumerable<RidesUser>).Where(ru => ru.RideId == ride.Id);

                foreach (var rideUser in rideUsers)
                {
                    rideUser.User = this.usersManager.GetItem(rideUser.UserId) as User;
                }

                viewModel.Id = ride.Id;
                viewModel.FromCity = ride.FromCity;
                viewModel.ToCity = ride.ToCity;
                viewModel.TravelDate = ride.TravelDate;
                viewModel.Price = ride.Price;
                viewModel.FreeSeats = ride.FreePlaces;
                viewModel.AdditionalInformation = ride.AdditionalInformation;
                viewModel.RideUsers = rideUsers;

                viewModel.DriverId = ride.DriverId.Value;
                var user = (this.usersManager.GetItem(ride.DriverId.Value) as User);
                viewModel.DriverName = user != null ? $"{user.FirstName} {user.LastName}" : "" ;

                var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);
                viewModel.CurrentUserId = userIdClaim.Value;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Detail", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(new CreateRideViewModel());
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            if(id == Guid.Empty)
            {
                return StatusCode(404);
            }

            var ride = this.ridesManager.GetItem(id) as Ride;

            if(ride == null)
            {
                return StatusCode(404);
            }

            var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

            if (ride.DriverId.ToString() != userIdClaim.Value)
            {
                return StatusCode(403);
            }

            var viewModel = new EditRideViewModel();

            viewModel.ToCity = ride.ToCity;
            viewModel.FromCity = ride.FromCity;
            viewModel.TravelDate = ride.TravelDate;
            viewModel.FreeSeats = ride.FreePlaces;
            viewModel.Price = ride.Price;
            viewModel.AdditionalInformation = ride.AdditionalInformation;
            viewModel.Id = ride.Id;

            return View(viewModel);
        }

        public IActionResult Browse(string from, string to, DateTime? startDate, DateTime? endDate, string order, int page)
        {
            BrowseRidesViewModel model = new BrowseRidesViewModel();
            model.FromCity = from;
            model.ToCity = to;
            model.OrderBy = order;
            model.Page = page == 0 ? 1 : page;

            var query = this.ridesManager.GetItems() as IEnumerable<Ride>;

            if (!string.IsNullOrEmpty(from))
            {
                query = query.Where(r => r.FromCity.ToLower() == from.ToLower());
            }

            if (!string.IsNullOrEmpty(to))
            {
                query = query.Where(r => r.ToCity.ToLower() == to.ToLower());
            }

            if(startDate != null)
            {
                query = query.Where(r => r.TravelDate >= startDate.Value);
            }

            if (endDate != null)
            {
                query = query.Where(r => r.TravelDate <= endDate.Value);
            }

            model.TotalCount = query.Count();
            model.TotalPages = (model.TotalCount + pageSize - 1) / pageSize;
            model.Page = model.TotalPages >= model.Page ? model.Page : model.TotalPages;

            if(string.IsNullOrEmpty(order) || order == "date")
            {
                query = query.OrderByDescending(o => o.TravelDate);
            }
            else if(order == "price")
            {
                query = query.OrderByDescending(o => o.Price);
            }

            query = query.Skip((model.Page - 1) * pageSize)
                .Take(pageSize);

            model.Rides = query;

            return View(model);
        }

        [Authorize]
        public IActionResult Manage(int page)
        {
            ManageRidesViewModel model = new ManageRidesViewModel();
            model.Page = page == 0 ? 1 : page;

            var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);
            var query = (this.ridesManager.GetItems() as IEnumerable<Ride>).Where(r => r.DriverId.ToString() == userIdClaim.Value).OrderByDescending(r => r.TravelDate) as IEnumerable<Ride>;


            model.TotalCount = query.Count();
            model.TotalPages = (model.TotalCount + pageSize - 1) / pageSize;
            model.Page = model.TotalPages >= model.Page ? model.Page : model.TotalPages;

            query = query.Skip((model.Page - 1) * pageSize)
                .Take(pageSize);

            model.Rides = query;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateRideViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId;
                var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                Ride ride = new Ride();
                ride.Id = Guid.NewGuid();
                ride.DriverId = Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty;
                ride.FromCity = model.FromCity;
                ride.ToCity = model.ToCity;
                ride.TravelDate = model.TravelDate;
                ride.FreePlaces = model.FreeSeats.Value;
                ride.Price = model.Price.Value;
                ride.AdditionalInformation = model.AdditionalInformation;

                this.ridesManager.CreateItem(ride);
                this.ridesManager.SaveChanges();

                return RedirectToAction("Manage", "Rides");

            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditRideViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ride = this.ridesManager.GetItem(model.Id) as Ride;

                if(ride != null)
                {
                    ride.FromCity = model.FromCity;
                    ride.ToCity = model.ToCity;
                    ride.TravelDate = model.TravelDate;
                    ride.FreePlaces = model.FreeSeats.Value;
                    ride.Price = model.Price.Value;
                    ride.AdditionalInformation = model.AdditionalInformation;
                }

                this.ridesManager.UpdateItem(ride);
                this.ridesManager.SaveChanges();

                return RedirectToAction("Manage", "Rides");

            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Remove(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
                var ride = this.ridesManager.GetItem(id) as Ride;

                if (ride != null)
                {
                    var rideUsers = (this.ridesUsersManager.GetItems() as IEnumerable<RidesUser>).Where(r => r.RideId == ride.Id);

                    foreach (var rideUser in rideUsers)
                    {
                        this.ridesUsersManager.DeleteItem(rideUser);
                    }

                    this.ridesUsersManager.SaveChanges();

                    this.ridesManager.DeleteItem(ride);
                    this.ridesManager.SaveChanges();
                }
            }

            return RedirectToAction("Manage", "Rides");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe(SubscribeRideViewModel model)
        {
            if(model.Id != Guid.Empty)
            {
                var ride = this.ridesManager.GetItem(model.Id) as Ride;

                if (ride != null)
                {
                    Guid userId;
                    var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);
                    
                    var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

                    if(user != null)
                    {
                        var rideUser = new RidesUser();
                        rideUser.User = user;
                        rideUser.UserId = user.Id;
                        rideUser.Ride = ride;
                        rideUser.RideId = ride.Id;

                        this.ridesUsersManager.CreateItem(rideUser);
                        this.ridesUsersManager.SaveChanges();

                        ride.RidesUsers.Add(rideUser);
                        user.RidesUsers.Add(rideUser);
                        ride.FreePlaces -= 1;

                        if(ride.FreePlaces >= 0)
                        {
                            this.ridesManager.UpdateItem(ride);
                            this.ridesManager.SaveChanges();

                            this.usersManager.UpdateItem(user);
                            this.usersManager.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Unsubscribe(UnsubscribeRideViewModel model)
        {
            if (model.Id != Guid.Empty)
            {
                var ride = this.ridesManager.GetItem(model.Id) as Ride;

                if (ride != null)
                {
                    Guid userId;
                    var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                    var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

                    if (user != null)
                    {
                        var rideUser = (this.ridesUsersManager.GetItems() as IEnumerable<RidesUser>).FirstOrDefault(ru => ru.UserId == user.Id);

                        if (rideUser != null)
                        {
                            ride.FreePlaces += 1;

                            this.ridesManager.UpdateItem(ride);
                            this.ridesManager.SaveChanges();

                            this.ridesUsersManager.DeleteItem(rideUser);
                            this.ridesUsersManager.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Detail", new { id = model.Id });
        }
    }
}
