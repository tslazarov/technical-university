using Bytes2you.Validation;
using Newtonsoft.Json;
using SmarterThanYou.Data.Factories;
using SmarterThanYou.Server.Models;
using SmarterThanYou.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmarterThanYou.Server.Controllers
{
    public class AccountController : ApiController
    {
        private IUsersService usersService;
        private IUsersFactory usersFactory;

        public AccountController(IUsersService usersService, IUsersFactory usersFactory)
        {
            Guard.WhenArgument<IUsersService>(usersService, "Users service cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IUsersFactory>(usersFactory, "Users factory cannot be null.")
                .IsNull()
                .Throw();

            this.usersService = usersService;
            this.usersFactory = usersFactory;
        }

        // POST api/account/register
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public string Register(UserViewModel model)
        {
            var user = this.usersFactory.CreateUser(model.Username, model.Password);

            try
            {
                this.usersService.CreateUser(user);
            }
            catch
            {
                return JsonConvert.SerializeObject(new { Status = 0 });
            }

            return JsonConvert.SerializeObject(new { Status = 1 });
        }

        // POST api/account/login
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public string Login(UserViewModel model)
        {
            var user = this.usersService.GetUsers()
                .Where(u => u.Username == model.Username && u.Password == model.Password)
                .FirstOrDefault();

            if(user == null)
            {
                return JsonConvert.SerializeObject(new { Status = "0" });
            }

            return JsonConvert.SerializeObject(new { Status = "1", Username = user.Username });
        }
    }
}
