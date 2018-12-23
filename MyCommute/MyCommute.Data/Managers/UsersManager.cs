using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommute.Data.Managers
{
    public class UsersManager : IUsersManager, IManager
    {
        private readonly IData data;

        public UsersManager(IData data)
        {
            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.UsersRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.UsersRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.UsersRepository.Create((User)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.UsersRepository.Delete((User)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.UsersRepository.Update((User)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
