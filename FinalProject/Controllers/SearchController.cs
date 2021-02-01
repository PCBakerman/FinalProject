﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;
using FinalProject.Models;
using FinalProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Controllers
{
    public class SearchController : Controller
    {
        private IYGOProDeckAccess _YGOProDeckAccess;
        private IDataAccess _DataAccess;
        public SearchController(IYGOProDeckAccess yGOProDeckAccess, IDataAccess dataAccess)
        {
            _YGOProDeckAccess = yGOProDeckAccess;
            _DataAccess = dataAccess;

        }
        public async Task<IActionResult> Index(string Query)
        {
            var model = new SearchIndexViewModel();
            if (!string.IsNullOrWhiteSpace(Query))
            {
                model.Query = Query;
                model.Results = await _YGOProDeckAccess.SearchCardsAsync(Query);

            }
            else
            {
                model.Query = string.Empty;
                model.Results = new List<Card>();

            }
            return View(model);
        }
        public async Task<IActionResult> Details(string Name, string query)
        {
            var model = new SearchDetailsViewModel();
            model.Query = query;
            if (!string.IsNullOrWhiteSpace(Name))
            {
               
                model.Result = await _YGOProDeckAccess.GetCardByNameAsync(Name);

            }
            else
            {
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(model.Result?.Name))
            {

                return View("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> AddCard(string name, string query)
        {
            var model = new SearchDetailsViewModel();
            model.Query = query;
            model.Name = name;
            var cardData = await _DataAccess.CardDataAccess.GetCardByNameAsync(name);
            if(cardData == null)
            {
                var apiCard = await _YGOProDeckAccess.GetCardByNameAsync(name);
                if(apiCard.CardImages != null && apiCard.CardImages.Count > 0)
                {
                    apiCard.ImageBytes = _YGOProDeckAccess.GetCardImage(apiCard.CardImages.First());
                }
                cardData = await _DataAccess.CardDataAccess.Upsert(apiCard);        
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home","Index" );
            }
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            var cardmapping = await _DataAccess.CardDataAccess.GetSpecificCardMappingForUserAsync(userInventory.Id, cardData.Id);
            if (cardmapping != null)
            {
                cardmapping.Count++;
                await _DataAccess.CardDataAccess.UpsertCardMappingAsync(cardmapping);
            }
            else
            {
                cardmapping = new InventoryCardMapping();
                cardmapping.CardId = cardData.Id;
                cardmapping.UserInventoryId = userInventory.Id;
                cardmapping.Count = 1;
                await _DataAccess.CardDataAccess.UpsertCardMappingAsync(cardmapping);
            }
            return RedirectToAction("Details", model);
        }
    }
}
