using Bytes2you.Validation;
using Lipwig.Services.Contracts;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Lipwig.Web.Controllers
{
    public class CurrenciesController : ApiController
    {
        private ICurrenciesService currenciesService;


        public CurrenciesController(ICurrenciesService currenciesService)
        {
            Guard.WhenArgument<ICurrenciesService>(currenciesService, "Currencies service cannot be null.")
                .IsNull()
                .Throw();

            this.currenciesService = currenciesService;
        }

        // GET api/currencies
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        [Route("api/currencies")]
        public HttpResponseMessage GetCurrencies()
        {
            var resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(this.currenciesService.GetCurrencies()));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}