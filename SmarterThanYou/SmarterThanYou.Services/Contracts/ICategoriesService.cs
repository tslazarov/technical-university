using SmartherThanYou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarterThanYou.Services.Contracts
{
    public interface ICategoriesService
    {
        Category GetCategoryByName(string name);
    }
}
