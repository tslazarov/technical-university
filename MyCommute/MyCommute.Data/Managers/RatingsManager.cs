using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;

namespace MyCommute.Data.Managers
{
    public class RatingsManager : IManager, IRatingsManager
    {
        private readonly IData data;

        public RatingsManager(IData data)
        {
            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.RatingsRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.RatingsRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.RatingsRepository.Create((Rating)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.RatingsRepository.Delete((Rating)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.RatingsRepository.Update((Rating)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
