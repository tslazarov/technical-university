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
        private readonly IEfRepository<RidesUser> ridesUsersRepository;
        private readonly IEfRepository<FriendRequest> friendRequestsUsersRepository;
        private readonly IEfRepository<Rating> ratingsRepository;

        public MyCommuteData(DbContext context,
                        IEfRepository<User> usersRepository,
                        IEfRepository<Car> carsRepository,
                        IEfRepository<Fuel> fuelsRepository,
                        IEfRepository<Ride> ridesRepository,
                        IEfRepository<RidesUser> ridesUsersRepository,
                        IEfRepository<FriendRequest> friendRequestsUsersRepository,
                        IEfRepository<Rating> ratingsRepository)
        {

            this.context = context;
            this.usersRepository = usersRepository;
            this.carsRepository = carsRepository;
            this.fuelsRepository = fuelsRepository;
            this.ridesRepository = ridesRepository;
            this.ridesUsersRepository = ridesUsersRepository;
            this.friendRequestsUsersRepository = friendRequestsUsersRepository;
            this.ratingsRepository = ratingsRepository;
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

        public IEfRepository<RidesUser> RidesUsersRepository
        {
            get
            {
                return this.ridesUsersRepository;
            }
        }

        public IEfRepository<FriendRequest> FriendRequestsUsersRepository
        {
            get
            {
                return this.friendRequestsUsersRepository;
            }
        }

        public IEfRepository<Rating> RatingsRepository
        {
            get
            {
                return this.ratingsRepository;
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}
