using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommute.Data.Managers
{
    public class FuelsManager : IFuelsManager, IManager
    {
        private readonly IData data;

        public FuelsManager(IData data)
        {
            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.UsersRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.FuelsRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.FuelsRepository.Create((Fuel)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.FuelsRepository.Delete((Fuel)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.FuelsRepository.Update((Fuel)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
