using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class FriendRequest : IDataItem
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public FriendRequestStatusType Status { get; set; }

        public User Receiver { get; set; }
        public User Sender { get; set; }
    }
}
