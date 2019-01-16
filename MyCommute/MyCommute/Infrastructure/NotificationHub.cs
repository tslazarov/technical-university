using Microsoft.AspNetCore.SignalR;
using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCommute.Infrastructure
{
    public class NotificationHub : Hub
    {
        private readonly IManager usersManager;

        public NotificationHub(IUsersManager usersManager)
        {
            this.usersManager = usersManager as IManager;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            if(!string.IsNullOrEmpty(userId))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            return base.OnConnectedAsync();
        }

        public async Task SendRatingNotification(string receiverId)
        {
            if (!string.IsNullOrEmpty(receiverId))
            {
                Guid id;
                if(Guid.TryParse(receiverId, out id))
                {
                    var user = this.usersManager.GetItem(id) as User;

                    if(user != null)
                    {
                        user.RatingNotifications += 1;

                        this.usersManager.UpdateItem(user);
                        this.usersManager.SaveChanges();

                        await Clients.Group(receiverId).SendAsync("ReceiveRatingNotification", user.RatingNotifications.ToString());
                    }
                }
            }
        }

        public async Task SendFriendNotification(string receiverId)
        {
            if (!string.IsNullOrEmpty(receiverId))
            {
                Guid id;
                if (Guid.TryParse(receiverId, out id))
                {
                    var user = this.usersManager.GetItem(id) as User;

                    if (user != null)
                    {
                        user.FriendNotifications += 1;

                        this.usersManager.UpdateItem(user);
                        this.usersManager.SaveChanges();

                        await Clients.Group(receiverId).SendAsync("ReceiveFriendNotification", user.FriendNotifications.ToString());
                    }
                }
            }
        }

        public async Task ClearRatingNotification()
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                Guid id;
                if (Guid.TryParse(userId, out id))
                {
                    var user = this.usersManager.GetItem(id) as User;

                    if (user != null)
                    {
                        user.RatingNotifications = 0;

                        this.usersManager.UpdateItem(user);
                        this.usersManager.SaveChanges();

                        await Task.CompletedTask;
                    }
                }
            }
        }

        public async Task ClearFriendNotification()
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                Guid id;
                if (Guid.TryParse(userId, out id))
                {
                    var user = this.usersManager.GetItem(id) as User;

                    if (user != null)
                    {
                        user.FriendNotifications = 0;

                        this.usersManager.UpdateItem(user);
                        this.usersManager.SaveChanges();

                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
