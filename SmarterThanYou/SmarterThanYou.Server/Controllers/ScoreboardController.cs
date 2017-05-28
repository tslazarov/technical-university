using Bytes2you.Validation;
using Newtonsoft.Json;
using SmarterThanYou.Data.Factories;
using SmarterThanYou.Server.Models;
using SmarterThanYou.Services.Contracts;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SmarterThanYou.Server.Controllers
{
    public class ScoreboardController : ApiController
    {
        private IScoresService scoresService;
        private IUsersService usersService;
        private IScoresFactory scoresFactory;

        public ScoreboardController(IScoresService scoresService,
            IUsersService usersService,
            IScoresFactory scoresFactory)
        {
            Guard.WhenArgument<IScoresService>(scoresService, "Scores service cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IUsersService>(usersService, "Users service cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IScoresFactory>(scoresFactory, "Scores factory cannot be null.")
                .IsNull()
                .Throw();

            this.scoresService = scoresService;
            this.usersService = usersService;
            this.scoresFactory = scoresFactory;
        }

        // POST api/account/submit
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Submit(ScoreViewModel model)
        {
            var resp = new HttpResponseMessage();

            var score = this.scoresService.GetScoreByUsername(model.Username);

            if(score == null)
            {
                var user = this.usersService.GetUserByUsername(model.Username);
                var newScore = this.scoresFactory.CreateScore(user.Id, user, model.Points);

                this.scoresService.CreateScore(newScore);

                resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1" }));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                return resp;
            }
            else
            {
                if(score.Points < model.Points)
                {
                    score.Points = model.Points;
                    this.scoresService.UpdateScore(score);

                    resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "1" }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return resp;
                }
            }

            resp.Content = new StringContent(JsonConvert.SerializeObject(new { Status = "0" }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // GET api/scoreboard/all
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage All()
        {
            var resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(new { Scoreboard = this.scoresService.GetTopScores(10).ToList().Select(s => new { Username = s.User.Username, Points = s.Points }) }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}