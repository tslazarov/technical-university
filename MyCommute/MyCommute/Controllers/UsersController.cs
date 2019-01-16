using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCommute.Data.Contracts;
using MyCommute.Extensions.Localization;
using MyCommute.Models;
using MyCommute.Models.Users;

namespace MyCommute.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class UsersController : Controller
    {
        private const int pageSize = 10;
        private readonly IManager usersManager;
        private readonly IManager friendRequestsManager;
        private readonly IManager ratingsManager;
        private readonly IManager carsManager;

        public UsersController(IUsersManager usersManager, 
            IFriendRequestsManager friendRequestsManager, 
            IRatingsManager ratingsManager,
            ICarsManager carsManager)
        {
            this.usersManager = usersManager as IManager;
            this.friendRequestsManager = friendRequestsManager as IManager;
            this.ratingsManager = ratingsManager as IManager;
            this.carsManager = carsManager as IManager;
        }

        [Authorize]
        public IActionResult Index(string name, string order, int page)
        {
            UsersViewModel model = new UsersViewModel();
            model.Name = name;
            model.OrderBy = order;
            model.Page = page == 0 ? 1 : page;

            var query = this.usersManager.GetItems() as IEnumerable<User>;
            var ratings = this.ratingsManager.GetItems() as IEnumerable<Rating>;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(r => r.FirstName.ToLower().Contains(name.ToLower()) || r.LastName.ToLower().Contains(name.ToLower()));
            }

            var result = query.ToList().Select(u =>
            {
                var driverRatings = ratings.Where(r => r.ReceiverId == u.Id && r.RatingType == RatingType.DriverRating);
                var passengerRatings = ratings.Where(r => r.ReceiverId == u.Id && r.RatingType == RatingType.PassengerRating);
                return new UserViewModel
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    DriverRating = driverRatings.Count() > 0 ? driverRatings.Sum(r => r.Value) / driverRatings.Count() : 0.0,
                    PassengerRating = passengerRatings.Count() > 0 ? passengerRatings.Sum(r => r.Value) / passengerRatings.Count() : 0.0,
                };
            });
            
            model.TotalCount = query.Count();
            model.TotalPages = (model.TotalCount + pageSize - 1) / pageSize;
            model.Page = model.TotalPages >= model.Page ? model.Page : model.TotalPages;

            if (string.IsNullOrEmpty(order) || order == "name")
            {
                result = result.OrderByDescending(r => r.Name);
            }
            else if (order == "driverRating")
            {
                result = result.OrderByDescending(r => r.DriverRating);
            }
            else if (order == "passengerRating")
            {
                result = result.OrderByDescending(r => r.PassengerRating);
            }

            result = result.Skip((model.Page - 1) * pageSize)
                .Take(pageSize);

            model.Users = result;

            return View(model);
        }

        public ActionResult Detail(Guid id)
        {
            var viewModel = new UserDetailViewModel();

            if(id != Guid.Empty)
            {
                var user = this.usersManager.GetItem(id) as User;

                if(user != null)
                {
                    viewModel.Id = user.Id;
                    viewModel.Name = $"{user.FirstName} {user.LastName}";
                    viewModel.Email = user.Email;
                    viewModel.Image = string.IsNullOrEmpty(user.Image) ? "/images/default-user-profile-image.svg" : user.Image;

                    Guid currentUserId;
                    var currentUserIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                    if (Guid.TryParse(currentUserIdClaim.Value, out currentUserId))
                    {
                        viewModel.CurrentUserId = currentUserId;

                        var currentUser = this.usersManager.GetItem(currentUserId) as User;

                        if (currentUser != null)
                        {
                            var friendRequests = (this.friendRequestsManager.GetItems() as IEnumerable<FriendRequest>);
                            viewModel.IsFriend = friendRequests.Any(fr => fr.Status == FriendRequestStatusType.Approved && 
                                ((fr.ReceiverId == id && fr.SenderId == currentUserId) || (fr.ReceiverId == currentUserId && fr.SenderId == id)));
                            viewModel.IsPending = friendRequests.Any(fr => fr.Status == FriendRequestStatusType.Pending &&
                                ((fr.ReceiverId == id && fr.SenderId == currentUserId) || (fr.ReceiverId == currentUserId && fr.SenderId == id)));

                            var ratings = (this.ratingsManager.GetItems() as IEnumerable<Rating>);
                            var driverRating = ratings.FirstOrDefault(r => (r.RaterId == currentUserId && r.ReceiverId == id) && r.RatingType == RatingType.DriverRating);

                            if(driverRating != null)
                            {
                                viewModel.IsRatedAsDriver = true;
                                viewModel.DriverRating = driverRating.Value;
                            }

                            var passengerRating = ratings.FirstOrDefault(r => (r.RaterId == currentUserId && r.ReceiverId == id) && r.RatingType == RatingType.PassengerRating);

                            if(passengerRating != null)
                            {
                                viewModel.IsRatedAsPassenger = true;
                                viewModel.PassengerRating = passengerRating.Value;
                            }

                            var driverRatings = ratings.Where(r => r.ReceiverId == user.Id && r.RatingType == RatingType.DriverRating);
                            var passengerRatings = ratings.Where(r => r.ReceiverId == user.Id && r.RatingType == RatingType.PassengerRating);

                            viewModel.DriverRatingDisplayed = driverRatings.Count() > 0 ? driverRatings.Sum(r => r.Value) / driverRatings.Count() : 0.0;
                            viewModel.PassengerRatingDisplayed = passengerRatings.Count() > 0 ? passengerRatings.Sum(r => r.Value) / passengerRatings.Count() : 0.0;

                            var car = (this.carsManager.GetItems() as IEnumerable<Car>).Where(c => c.OwnerId == user.Id).FirstOrDefault();

                            if(car != null)
                            {
                                viewModel.CarBrand = car.Brand;
                                viewModel.CarModel = car.Model;
                            }
                        }
                    }
                }
                else
                {
                    return StatusCode(404);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddFriend(Guid id)
        {
            if (id != Guid.Empty)
            {
                var receiver = this.usersManager.GetItem(id) as User;

                if (receiver != null)
                {
                    Guid senderId;
                    var senderIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                    if (Guid.TryParse(senderIdClaim.Value, out senderId))
                    {
                        var sender = this.usersManager.GetItem(senderId) as User;

                        if (sender != null)
                        {
                            var friendRequest = new FriendRequest();
                            friendRequest.Id = Guid.NewGuid();
                            friendRequest.Receiver = receiver;
                            friendRequest.ReceiverId = receiver.Id;
                            friendRequest.Sender = sender;
                            friendRequest.SenderId = sender.Id;
                            friendRequest.Status = FriendRequestStatusType.Pending;

                            this.friendRequestsManager.CreateItem(friendRequest);
                            this.friendRequestsManager.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoveFriend(Guid id)
        {
            if (id != Guid.Empty)
            {
                var receiver = this.usersManager.GetItem(id) as User;

                if (receiver != null)
                {
                    Guid currentUserId;
                    var currentUserIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                    if (Guid.TryParse(currentUserIdClaim.Value, out currentUserId))
                    {
                        var currentUser = this.usersManager.GetItem(currentUserId) as User;

                        if (currentUser != null)
                        {
                            var friendRequests = (this.friendRequestsManager.GetItems() as IEnumerable<FriendRequest>);

                            var friendRequest = friendRequests.FirstOrDefault(fr => (fr.ReceiverId == id && fr.SenderId == currentUserId) || (fr.ReceiverId == currentUserId && fr.SenderId == id));

                            this.friendRequestsManager.DeleteItem(friendRequest);
                            this.friendRequestsManager.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddRating(Guid id, byte ratingValue, string ratingComment, string ratingType)
        {
            if (id != Guid.Empty)
            {
                var receiver = this.usersManager.GetItem(id) as User;

                if (receiver != null)
                {
                    Guid raterId;
                    var raterIdClaim = this.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);

                    if (Guid.TryParse(raterIdClaim.Value, out raterId))
                    {
                        var rater = this.usersManager.GetItem(raterId) as User;

                        if (rater != null)
                        {
                            var rating = new Rating();
                            rating.Id = Guid.NewGuid();
                            rating.Receiver = receiver;
                            rating.ReceiverId = receiver.Id;
                            rating.Rater = rater;
                            rating.RaterId = rater.Id;
                            rating.Value = ratingValue;
                            rating.RatingType = ratingType == "Driver" ? RatingType.DriverRating : RatingType.PassengerRating;
                            rating.Comment = ratingComment;
                            rating.DateCreated = DateTime.Now;

                            this.ratingsManager.CreateItem(rating);
                            this.ratingsManager.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Detail", new { id });
        }
    }
}