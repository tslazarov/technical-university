using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Profile
{
    public class ProfileFriendsViewModel
    {
        public IList<FriendRequestViewModel> ApprovedFriendRequests { get; set; }
        public IList<FriendRequestViewModel> PendingFriendRequests { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class FriendRequestViewModel
    {
        public Guid FriendRequestId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
