using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommute.Data.Managers
{
    public class RidesUsersManager : IManager, IRidesUsersManager
    {
        private readonly IData data;

        public RidesUsersManager(IData data)
        {
            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.RidesUsersRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.RidesUsersRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.RidesUsersRepository.Create((RidesUser)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.RidesUsersRepository.Delete((RidesUser)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.RidesUsersRepository.Update((RidesUser)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
