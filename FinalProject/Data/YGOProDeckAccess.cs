using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace FinalProject.Data
{
    public class YGOProDeckAccess : IYGOProDeckAccess
    {
        public async Task<List<Card>> SearchCardsAsync([FromQuery] string q)
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
                if (data != null)
                {
                    cards = YGOProDeckData.MapCardsFromData(data);
                }
            }

            Console.WriteLine(response.Content);
            return cards;

        }
    }
}
