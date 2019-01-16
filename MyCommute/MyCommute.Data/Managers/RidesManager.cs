using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommute.Data.Managers
{
    public class RidesManager : IManager, IRidesManager
    {
        private readonly IData data;

        public RidesManager(IData data)
        {
            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.RidesRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.RidesRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.RidesRepository.Create((Ride)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.RidesRepository.Delete((Ride)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.RidesRepository.Update((Ride)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
