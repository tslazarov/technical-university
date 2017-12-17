using Bytes2you.Validation;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Lipwig.Web.Controllers
{
    public class ExpensesController : ApiController
    {
        private IExpensesService expensesService;


        public ExpensesController(IExpensesService expensesService)
        {
            Guard.WhenArgument<IExpensesService>(expensesService, "Expenses service cannot be null.")
                .IsNull()
                .Throw();

            this.expensesService = expensesService;
        }

        // GET api/expenses/{id}
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        [Route("api/expenses/{id}")]
        public HttpResponseMessage GetExpense(Guid id)
        {
            var resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(this.expensesService.GetExpense(id)));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // PUT api/expenses/update
        [System.Web.Http.AcceptVerbs("PUT")]
        [System.Web.Http.HttpPut]
        [Route("api/expenses/update")]
        public HttpResponseMessage Update(Expense expense)
        {
            var resp = new HttpResponseMessage();

            if (expense != null)
            {
                try
                {
                    this.expensesService.UpdateExpense(expense);
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

        // DELETE api/expenses/{id}
        [System.Web.Http.AcceptVerbs("DELETE")]
        [System.Web.Http.HttpDelete]
        [Route("api/expenses/{id}")]
        public HttpResponseMessage DeleteExpense(Guid id)
        {
            var resp = new HttpResponseMessage();

            var expense = this.expensesService.GetExpense(id);

            if (expense != null)
            {
                try
                {
                    this.expensesService.DeleteExpense(expense);
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