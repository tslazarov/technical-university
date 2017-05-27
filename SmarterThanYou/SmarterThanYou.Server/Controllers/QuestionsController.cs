using Bytes2you.Validation;
using Newtonsoft.Json;
using SmarterThanYou.Data.Factories;
using SmarterThanYou.Server.Models;
using SmarterThanYou.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SmarterThanYou.Server.Controllers
{
    public class QuestionsController : ApiController
    {
        private IQuestionsService questionsService;
        private IAnswersService answersService;
        private IQuestionsFactory questionsFactory;
        private ICategoriesService categoriesService;
        private IAnswersFactory answersFactory;

        public QuestionsController(IQuestionsService questionsService,
            IAnswersService answersService,
            ICategoriesService categoriesService,
            IQuestionsFactory questionsFactory,
            IAnswersFactory answersFactory)
        {
            Guard.WhenArgument<IQuestionsService>(questionsService, "Questions service cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IAnswersService>(answersService, "Answers service cannot be null.")
                 .IsNull()
                 .Throw();

            Guard.WhenArgument<ICategoriesService>(categoriesService, "Categories service cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IQuestionsFactory>(questionsFactory, "Questions factory cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IAnswersFactory>(answersFactory, "Answers factory cannot be null.")
                .IsNull()
                .Throw();

            this.questionsService = questionsService;
            this.answersService = answersService;
            this.categoriesService = categoriesService;
            this.questionsFactory = questionsFactory;
            this.answersFactory = answersFactory;
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

        // POST api/questions/create
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public void Create(QuestionViewModel model)
        {
            var category = this.categoriesService.GetCategoryByName(model.Category);

            var question = this.questionsFactory.CreateQuestion(category.Id, category, model.Question);

            foreach (var answer in model.Answers)
            {
                var addedAnswer = this.answersFactory.CreateAnswer(answer);
                this.answersService.CreateAnswer(addedAnswer);
            }

            foreach (var answer in model.Answers)
            {
                var addedAnswer = this.answersService.GetAnswer(answer);

                if(addedAnswer.Member == model.Answer)
                {
                    question.Answer = addedAnswer;
                    question.AnswerId = addedAnswer.Id;
                }

                question.Answers.Add(addedAnswer);
            }

            this.questionsService.CreateQuestion(question);
        }
    }
}
