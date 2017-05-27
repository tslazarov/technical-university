using Bytes2you.Validation;
using Newtonsoft.Json;
using SmarterThanYou.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SmarterThanYou.Server.Controllers
{
    public class QuestionsController : ApiController
    {
        private IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            Guard.WhenArgument<IQuestionsService>(questionsService, "Questions service cannot be null.")
                .IsNull()
                .Throw();

            this.questionsService = questionsService;
        }

        // GET api/questions/random
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Random()
        {
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(this.questionsService.GetQuestion()))
            };

            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
    }
}
