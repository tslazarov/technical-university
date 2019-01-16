using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleApi;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.AutoComplete.Request.Enums;
using GoogleApi.Entities.Common.Enums;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace MyCommute.Controllers
{
    public class PlacesAutocompleteController : Controller
    {
        private readonly IConfiguration configuration;

        public PlacesAutocompleteController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Query(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var request = new PlacesAutoCompleteRequest();
            request.Key = this.configuration.GetValue<string>("API:Google:Key").ToString();
            request.Input = input;
            request.Types = new List<RestrictPlaceType>() { RestrictPlaceType.Cities };
            request.Language = !Regex.IsMatch(input, @"\P{IsCyrillic}") ? Language.Bulgarian : Language.English;
            var uri = request.GetUri();
            var parameterss = request.GetQueryStringParameters();

            var response = GooglePlaces.AutoComplete.Query(request);

            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                var predictions = response.Predictions.Select(p => p.Terms.FirstOrDefault());
                return Json(predictions.Select(p => p.Value).Distinct().ToArray());
            }
            return null;
        }
    }
}
