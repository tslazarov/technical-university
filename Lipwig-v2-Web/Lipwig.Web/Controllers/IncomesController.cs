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
    public class IncomesController : ApiController
    {
        private IIncomesService incomesService;


        public IncomesController(IIncomesService incomesService)
        {
            Guard.WhenArgument<IIncomesService>(incomesService, "Incomes service cannot be null.")
                .IsNull()
                .Throw();

            this.incomesService = incomesService;
        }

        // GET api/incomes/{id}
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        [Route("api/incomes/{id}")]
        public HttpResponseMessage GetExpense(Guid id)
        {
            var resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(this.incomesService.GetIncome(id)));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // PUT api/incomes/update
        [System.Web.Http.AcceptVerbs("PUT")]
        [System.Web.Http.HttpPut]
        [Route("api/incomes/update")]
        public HttpResponseMessage Update(Income income)
        {
            var resp = new HttpResponseMessage();

            if (income != null)
            {
                try
                {
                    this.incomesService.UpdateIncome(income);
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

        // DELETE api/incomes/{id}
        [System.Web.Http.AcceptVerbs("DELETE")]
        [System.Web.Http.HttpDelete]
        [Route("api/incomes/{id}")]
        public HttpResponseMessage DeleteIncome(Guid id)
        {
            var resp = new HttpResponseMessage();

            var income = this.incomesService.GetIncome(id);

            if (income != null)
            {
                try
                {
                    this.incomesService.DeleteIncome(income);
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