using SmartherThanYou.Models;

namespace SmarterThanYou.Data.Factories
{
    public interface IUsersFactory
    {
        User CreateUser(string username, string password);
    }
}
