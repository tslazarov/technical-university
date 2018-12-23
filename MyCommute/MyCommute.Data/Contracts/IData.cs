using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommute.Data.Contracts
{
    public interface IData
    {
        IEfRepository<User> UsersRepository { get; }

        IEfRepository<Car> CarsRepository { get; }

        IEfRepository<Fuel> FuelsRepository { get; }

        IEfRepository<Ride> RidesRepository { get; }

        void SaveChanges();
    }
}
