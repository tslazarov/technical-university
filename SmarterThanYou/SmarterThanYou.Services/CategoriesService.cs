using SmarterThanYou.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartherThanYou.Models;
using SmarterThanYou.Data.Contracts;
using Bytes2you.Validation;

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
