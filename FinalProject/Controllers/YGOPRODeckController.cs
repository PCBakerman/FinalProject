using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using FinalProject.Models;

namespace FinalProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class YGOPRODeckController : ControllerBase
    {
        [HttpGet]
        [Route("search")]
        public async Task<List<Card>> SearchCardsAsync([FromQuery]string q)
        {
            var cards = new List<Card>();
            var client = new RestClient($"https://db.ygoprodeck.com/api/v7/cardinfo.php?fname={q}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "__cfduid=d13abaa4aab935e5fda6f7a9bbe67b31a1610472930");
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                YGOProDeckData data = JsonConvert.DeserializeObject<YGOProDeckData>(response.Content);
                if(data != null)
                {
                    cards = YGOProDeckData.MapCardsFromData(data);
                }
            }

            Console.WriteLine(response.Content);
            return cards;

        }

    }
}
