using Bytes2you.Validation;
using Lipwig.Data.Factories;
using Lipwig.Services.Contracts;
using Lipwig.Web.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Lipwig.Web.Controllers
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
        [Route("api/account/register")]
        public HttpResponseMessage Register(UserRegisterViewModel model)
        {
            var existingUser = false;

            if (this.usersService.GetUserByEmail(model.Email) != null)
            {
                existingUser = true;
            }
            var user = this.usersFactory.Create(Guid.NewGuid(), model.Email, model.FirstName, model.LastName, model.Balance, model.Currency);


            var resp = new HttpResponseMessage();

            if (!existingUser)
            {
                try
                {
                    this.usersService.Register(user, model.Password);
                }
                catch
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return resp;
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1" }));
            }
            else
            {
                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "2" }));
            }

            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // POST api/account/login
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [Route("api/account/login")]
        public HttpResponseMessage Login(UserViewModel model)
        {
            var user = this.usersService.Login(model.Email, model.Password);

            var resp = new HttpResponseMessage();

            if (user == null)
            {
                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            else
            {
                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1", ComplexObject = JsonConvert.SerializeObject(new { Email = user.Email, Currency = user.Currency, LocalizedBalance = user.LocalizedBalance }) }));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return resp;
        }
    }
}
