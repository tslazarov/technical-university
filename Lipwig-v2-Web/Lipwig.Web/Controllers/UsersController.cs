using Bytes2you.Validation;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using Lipwig.Web.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Lipwig.Web.Controllers
{
    public class UsersController : ApiController
    {

        private IUsersService usersService;


        public UsersController(IUsersService usersService)
        {
            Guard.WhenArgument<IUsersService>(usersService, "Users service cannot be null.")
                .IsNull()
                .Throw();

            this.usersService = usersService;
        }

        // GET api/users/{email}
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        [Route("api/users/{email}")]
        public HttpResponseMessage GetUsersByEmail(string email)
        {
            var resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(this.usersService.GetUserByEmail(StringHelper.DecodeEmailFromUrl(email))));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // PUT api/users/update
        [System.Web.Http.AcceptVerbs("PUT")]
        [System.Web.Http.HttpPut]
        [Route("api/users/update")]
        public HttpResponseMessage Update(User user)
        {
            var resp = new HttpResponseMessage();

            if (user != null)
            {
                try
                {
                    this.usersService.UpdateUser(user);
                }
                catch
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return resp;
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1" }));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }

            resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // PUT api/users/updatePassword
        [System.Web.Http.AcceptVerbs("PUT")]
        [System.Web.Http.HttpPut]
        [Route("api/users/updatePassword")]
        public HttpResponseMessage UpdatePassword(UserUpdatePasswordViewModel user)
        {
            var resp = new HttpResponseMessage();
            bool isOldPasswordMatching = false;

            if (user != null)
            {
                try
                {
                    isOldPasswordMatching = this.usersService.UpdateUserPassword(user.Email, user.OldPassword, user.NewPassword);
                }
                catch
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0", ComplexObject = isOldPasswordMatching }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return resp;
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1", ComplexObject = isOldPasswordMatching  }));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }

            isOldPasswordMatching = true;
            resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0", ComplexObject = isOldPasswordMatching } ));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }


        // POST api/users/addExpense
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [Route("api/users/addExpense")]
        public HttpResponseMessage AddExpense(ExpenseCreateViewModel model)
        {
            var resp = new HttpResponseMessage();

            if (model.Expense != null)
            {
                try
                {
                    this.usersService.SaveExpense(model.Email, model.Expense);
                }
                catch
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return resp;
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1" }));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }

            resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }


        // POST api/users/addIncome
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [Route("api/users/addIncome")]
        public HttpResponseMessage AddIncome(IncomeCreateViewModel model)
        {
            var resp = new HttpResponseMessage();

            if (model.Income != null)
            {
                try
                {
                    this.usersService.SaveIncome(model.Email, model.Income);
                }
                catch
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return resp;
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1" }));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }

            resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}