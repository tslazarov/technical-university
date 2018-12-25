using Microsoft.Extensions.Configuration;
using MyCommute.Data.Contracts;
using MyCommute.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCommute.Extensions.Hangfire
{
    public class FuelPriceFetcher
    {
        private readonly IConfiguration configuration;
        private readonly IManager fuelsManager;
        private readonly IHttpClientFactory clientFactory;
        private readonly string[] fuelTypes = new string[] { "gasoline", "diesel", "lpg", "methane" };
        public FuelPriceFetcher(IFuelsManager fuelsManager, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.fuelsManager = fuelsManager as IManager;
            this.clientFactory = clientFactory;
        }

        public async Task FetchData()
        {
            foreach (var fuelType in fuelTypes)
            {
                var url = string.Format("https://fuelo.net/api/price?key={0}&fuel={1}&date={2}", this.configuration.GetValue<string>("API:Fuelo:Key").ToString(), fuelType, DateTime.Now.ToString("yyyy-MM-dd"));
                await FetchDataInternal(url, fuelType);
            }
        }

        public async Task FetchDataInternal(string url, string fuelType)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,url);

            var client = this.clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var resultObject = JsonConvert.DeserializeObject<FuelPriceModel>(result);

                if(resultObject.Status == "OK")
                {
                    if(Enum.TryParse(fuelType.First().ToString().ToUpper() + fuelType.Substring(1), out FuelType type))
                    {
                        var fuel = (this.fuelsManager.GetItems() as IEnumerable<Fuel>).FirstOrDefault(f => f.FuelType == type.ToString());

                        if(fuel != null)
                        {
                            fuel.FuelPrice = resultObject.Price;
                            fuel.ModifiedDate = DateTime.Now;

                            this.fuelsManager.UpdateItem(fuel);
                            this.fuelsManager.SaveChanges();
                        }
                        else
                        {
                            fuel = new Fuel();
                            fuel.Id = Guid.NewGuid();
                            fuel.FuelPrice = resultObject.Price;
                            fuel.FuelType = type.ToString();

                            this.fuelsManager.CreateItem(fuel);
                            this.fuelsManager.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
