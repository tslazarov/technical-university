using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Utilities
{
    public interface IPasswordHelper
    {
        string CreateSalt(int size);
        string CreatePasswordHash(string password, string salt);
    }
}
