using SmartherThanYou.Models;

namespace SmarterThanYou.Services.Contracts
{
    public interface ICategoriesService
    {
        Category GetCategoryByName(string name);
    }
}
