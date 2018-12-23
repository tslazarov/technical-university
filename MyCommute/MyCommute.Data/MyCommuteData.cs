using Microsoft.EntityFrameworkCore;
using MyCommute.Data.Contracts;
using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommute.Data
{
    public class MyCommuteData : IData
    {
        private readonly DbContext context;
        private readonly IEfRepository<User> usersRepository;
        private readonly IEfRepository<Car> carsRepository;
        private readonly IEfRepository<Fuel> fuelsRepository;
        private readonly IEfRepository<Ride> ridesRepository;

        public MyCommuteData(DbContext context,
                        IEfRepository<User> usersRepository,
                        IEfRepository<Car> carsRepository,
                        IEfRepository<Fuel> fuelsRepository,
                        IEfRepository<Ride> ridesRepository)
        {

            this.context = context;
            this.usersRepository = usersRepository;
            this.carsRepository = carsRepository;
            this.fuelsRepository = fuelsRepository;
            this.ridesRepository = ridesRepository;
        }

        public IEfRepository<User> UsersRepository
        {
            get
            {
                return this.usersRepository;
            }
        }

        public IEfRepository<Car> CarsRepository
        {
            get
            {
                return this.carsRepository;
            }
        }

        public IEfRepository<Fuel> FuelsRepository
        {
            get
            {
                return this.fuelsRepository;
            }
        }

        public IEfRepository<Ride> RidesRepository
        {
            get
            {
                return this.ridesRepository;
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}
