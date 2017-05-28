using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using SmarterThanYou.Services.Contracts;
using SmartherThanYou.Models;
using System.Linq;

namespace SmarterThanYou.Services
{
    public class CategoriesService : ICategoriesService
    {
        private ÌSmarterThanYouData data;

        public CategoriesService(ÌSmarterThanYouData data)
        {
            Guard.WhenArgument<ÌSmarterThanYouData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public Category GetCategoryByName(string name)
        {
            return this.data.CategoriesRepository.All().Where(c => c.Name == name).FirstOrDefault();
        }
    }
}
