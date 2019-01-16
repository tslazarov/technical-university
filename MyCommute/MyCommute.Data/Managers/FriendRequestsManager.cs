using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;

namespace MyCommute.Data.Managers
{
    public class FriendRequestsManager : IManager, IFriendRequestsManager
    {
        private readonly IData data;

        public FriendRequestsManager(IData data)
        {
            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.FriendRequestsUsersRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.FriendRequestsUsersRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.FriendRequestsUsersRepository.Create((FriendRequest)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.FriendRequestsUsersRepository.Delete((FriendRequest)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.FriendRequestsUsersRepository.Update((FriendRequest)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
