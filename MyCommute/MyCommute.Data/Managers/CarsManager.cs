using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommute.Data.Managers
{
    public class CarsManager : ICarsManager, IManager
    {
        private readonly IData data;

        public CarsManager(IData data)
        {
            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.CarsRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.CarsRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.CarsRepository.Create((Car)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.CarsRepository.Delete((Car)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.CarsRepository.Update((Car)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
