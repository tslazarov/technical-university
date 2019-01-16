using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyCommute.Data.Contracts;
using MyCommute.Extensions.Localization;
using MyCommute.Models;
using MyCommute.Models.Profile;
using MyCommute.Utilities;
using NToastNotify;

namespace MyCommute.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ProfileController : Controller
    {
        private const int pageSize = 10;

        private readonly IManager usersManager;
        private readonly IManager carsManager;
        private readonly IManager friendRequestsManager;
        private readonly IManager ratingsManager;
        private readonly IToastNotification toastNotification;
        private readonly IImageHelper imageHelper;
        private readonly IStringLocalizer<ProfileController> localizer;

        public ProfileController(IUsersManager usersManager, 
            IFriendRequestsManager friendRequestsManager,
            IRatingsManager ratingsManager,
            ICarsManager carsManager, 
            IToastNotification toastNotification, 
            IImageHelper imageHelper,
            IStringLocalizer<ProfileController> localizer)
        {
            this.usersManager = usersManager as IManager;
            this.friendRequestsManager = friendRequestsManager as IManager;
            this.ratingsManager = ratingsManager as IManager;
            this.carsManager = carsManager as IManager;
            this.toastNotification = toastNotification;
            this.imageHelper = imageHelper;
            this.localizer = localizer;
        }

        [Authorize]
        public IActionResult Friends(string name, int page)
        {
            var model = new ProfileFriendsViewModel();
            model.Name = name;
            model.Page = page == 0 ? 1 : page;

            Guid userId;
            var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

            var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

            if (user != null)
            {
                var friendRequests = (this.friendRequestsManager.GetItems() as IEnumerable<FriendRequest>).Where(fr => fr.ReceiverId == user.Id || fr.SenderId == user.Id);
                var oneWayFriendRequests = (this.friendRequestsManager.GetItems() as IEnumerable<FriendRequest>).Where(fr => fr.ReceiverId == user.Id);

                var approvedFriendRequests = friendRequests.Where(fr => fr.Status == FriendRequestStatusType.Approved);
                var pendingFriendRequests = oneWayFriendRequests.Where(fr => fr.Status == FriendRequestStatusType.Pending);

                model.ApprovedFriendRequests = new List<FriendRequestViewModel>();
                foreach (var request in approvedFriendRequests)
                {
                    User requestUser = null;

                    if(request.ReceiverId != user.Id)
                    {
                        requestUser = this.usersManager.GetItem(request.ReceiverId) as User;
                    }
                    else if(request.SenderId != user.Id)
                    {
                        requestUser = this.usersManager.GetItem(request.SenderId) as User;
                    }

                    if(requestUser != null)
                    {
                        model.ApprovedFriendRequests.Add(new FriendRequestViewModel() {
                            FriendRequestId = request.Id,
                            UserId = requestUser.Id,
                            Name = $"{requestUser.FirstName} {requestUser.LastName}",
                            Email = requestUser.Email
                        });
                    }
                }

                model.PendingFriendRequests = new List<FriendRequestViewModel>();
                foreach (var request in pendingFriendRequests)
                {
                    User requestUser = null;

                    if (request.ReceiverId != user.Id)
                    {
                        requestUser = this.usersManager.GetItem(request.ReceiverId) as User;
                    }
                    else if (request.SenderId != user.Id)
                    {
                        requestUser = this.usersManager.GetItem(request.SenderId) as User;
                    }

                    if (requestUser != null)
                    {
                        model.PendingFriendRequests.Add(new FriendRequestViewModel()
                        {
                            FriendRequestId = request.Id,
                            UserId = requestUser.Id,
                            Name = $"{requestUser.FirstName} {requestUser.LastName}",
                            Email = requestUser.Email
                        });
                    }
                }

                if (!string.IsNullOrEmpty(name))
                {
                    model.ApprovedFriendRequests = model.ApprovedFriendRequests.Where(r => r.Name.ToLower().Contains(name.ToLower())).ToList();
                }

                model.TotalCount = model.ApprovedFriendRequests.Count();
                model.TotalPages = (model.TotalCount + pageSize - 1) / pageSize;
                model.Page = model.TotalPages >= model.Page ? model.Page : model.TotalPages;
            }
            
            return View(model);
        }

        [Authorize]
        public IActionResult Ratings(string filterBy, int page)
        {
            var model = new ProfileRatingsViewModel();
            model.FilterBy = filterBy;
            model.Page = page == 0 ? 1 : page;

            Guid userId;
            var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

            var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

            if (user != null)
            {
                var ratings = (this.ratingsManager.GetItems() as IEnumerable<Rating>).Where(r => r.ReceiverId == user.Id).OrderByDescending(r => r.DateCreated);

                model.Ratings = new List<RatingViewModel>();
                foreach (var rating in ratings)
                {
                    User raterUser = this.usersManager.GetItem(rating.RaterId) as User;

                    if (raterUser != null)
                    {
                        model.Ratings.Add(new RatingViewModel()
                        {
                            RatingId = rating.Id,
                            UserId = raterUser.Id,
                            Name = $"{raterUser.FirstName} {raterUser.LastName}",
                            Value = rating.Value,
                            RatingType = rating.RatingType,
                            Type = rating.RatingType == RatingType.DriverRating ? localizer["Profile_Driver"].Value : localizer["Profile_Passenger"].Value,
                            Comment = rating.Comment
                        });
                    }
                }

                if (!string.IsNullOrEmpty(filterBy) && filterBy != "all")
                {
                    if (Enum.TryParse(filterBy.First().ToString().ToUpper() + filterBy.Substring(1), out RatingType type))
                    {
                        model.Ratings = model.Ratings.Where(r => r.RatingType == type).ToList();
                    }
                }

                model.TotalCount = model.Ratings.Count();
                model.TotalPages = (model.TotalCount + pageSize - 1) / pageSize;
                model.Page = model.TotalPages >= model.Page ? model.Page : model.TotalPages;
            }

            return View(model);
        }



        [Authorize]
        public IActionResult Index()
        {
            var viewModel = new ProfileDetailViewModel();

            Guid userId;
            var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

            var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

            if (user != null)
            {
                viewModel.Id = user.Id;
                viewModel.Name = $"{user.FirstName} {user.LastName}";
                viewModel.Email = user.Email;
                viewModel.Image = string.IsNullOrEmpty(user.Image) ? "/images/default-user-profile-image.svg" : user.Image;
                viewModel.IsExternal = user.IsExternal;
                viewModel.PersonalInformationViewModel = new ProfilePersonalInformationViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

                viewModel.CarInformationViewModel = new ProfileCarInformationViewModel();

                var car = (this.carsManager.GetItems() as IEnumerable<Car>).Where(c => c.OwnerId == user.Id).FirstOrDefault();
                if (car != null)
                {
                    viewModel.CarInformationViewModel.Brand = car.Brand;
                    viewModel.CarInformationViewModel.Model = car.Model;
                    viewModel.CarInformationViewModel.Seats = car.Seats;
                    viewModel.CarInformationViewModel.FuelType = car.FuelType.ToString().ToLower();
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult PersonalInformation(ProfilePersonalInformationViewModel profilePersonalInformationViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid userId;
                var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

                if (user != null)
                {
                    user.FirstName = profilePersonalInformationViewModel.FirstName;
                    user.LastName = profilePersonalInformationViewModel.LastName;

                    this.usersManager.UpdateItem(user);
                    this.usersManager.SaveChanges();

                    // Message is always in English - add localization when fix is available - https://github.com/nabinked/NToastNotify/issues/37
                    this.toastNotification.AddSuccessToastMessage("Successfully changed personal information");
                }
            }   
            else
            {
                return PartialView("PersonalInformationPartial", profilePersonalInformationViewModel);
            }

            return PartialView("PersonalInformationPartial", profilePersonalInformationViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CarInformation(ProfileCarInformationViewModel profileCarInformationViewModel)
        {
            bool isNew = false;

            if (ModelState.IsValid)
            {
                Guid userId;
                var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

                if (user != null)
                {
                    var car = (this.carsManager.GetItems() as IEnumerable<Car>).Where(c => c.OwnerId == user.Id).FirstOrDefault();

                    if(car == null)
                    {
                        isNew = true;
                        car = new Car();
                        car.Id = Guid.NewGuid();
                        car.Owner = user;
                        car.OwnerId = user.Id;
                    }

                    car.Brand = profileCarInformationViewModel.Brand;
                    car.Model = profileCarInformationViewModel.Model;
                    car.Seats = profileCarInformationViewModel.Seats.HasValue ? profileCarInformationViewModel.Seats.Value : 0;

                    var fuelType = profileCarInformationViewModel.FuelType;
                    if (Enum.TryParse(fuelType.First().ToString().ToUpper() + fuelType.Substring(1), out FuelType type))
                    {
                        car.FuelType = type;
                    }

                    if (isNew)
                    {
                        this.carsManager.CreateItem(car);
                    }
                    else
                    {
                        this.carsManager.UpdateItem(car);
                    }

                    this.carsManager.SaveChanges();

                    // Message is always in English - add localization when fix is available - https://github.com/nabinked/NToastNotify/issues/37
                    this.toastNotification.AddSuccessToastMessage("Successfully changed car information");
                }
            }
            else
            {
                return PartialView("CarInformationPartial", profileCarInformationViewModel);
            }

            return PartialView("CarInformationPartial", profileCarInformationViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ProfileImage(IFormFile image)
        {
            Guid userId;
            var userIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

            var user = this.usersManager.GetItem(Guid.TryParse(userIdClaim.Value, out userId) ? userId : Guid.Empty) as User;

            if (user != null)
            {
                var imagePath = this.imageHelper.UploadImage(image, user.Id.ToString()).Result;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    user.Image = imagePath;

                    this.usersManager.UpdateItem(user);
                    this.usersManager.SaveChanges();

                    // Message is always in English - add localization when fix is available - https://github.com/nabinked/NToastNotify/issues/37
                    this.toastNotification.AddSuccessToastMessage("Successfully changed profile image");
                }
            }

            return PartialView("ProfileImagePartial");
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoveFriend(Guid id)
        {
            if (id != Guid.Empty)
            {
                var friendRequest = (this.friendRequestsManager.GetItems() as IEnumerable<FriendRequest>).FirstOrDefault(fr => fr.Id == id);

                if (friendRequest != null)
                {
                    this.friendRequestsManager.DeleteItem(friendRequest);
                    this.friendRequestsManager.SaveChanges();
                }
            }

            return RedirectToAction("Friends");
        }

        [HttpPost]
        [Authorize]
        public ActionResult ApproveFriend(Guid id)
        {
            if (id != Guid.Empty)
            {
                var friendRequest = (this.friendRequestsManager.GetItems() as IEnumerable<FriendRequest>).FirstOrDefault(fr => fr.Id == id);

                if(friendRequest != null)
                {
                    friendRequest.Status = FriendRequestStatusType.Approved;
                    this.friendRequestsManager.UpdateItem(friendRequest);
                    this.friendRequestsManager.SaveChanges();
                }
            }

            return RedirectToAction("Friends");
        }
    }
}