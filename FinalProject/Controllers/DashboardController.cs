using System;
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
    public class DashboardController : Controller
    {
        private IDataAccess _DataAccess;
        public DashboardController(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;
        }
        public async Task<IActionResult> Index()
        {
            var defaultImage = await _DataAccess.DefaultImageDataAccess.GetDefaultImageByIdBytesAync(1);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var tradeListings = await _DataAccess.TradeDataAccess.GetTradeListingsByUser(userId);
            var rating = await _DataAccess.TradeDataAccess.GetUserRating(userId);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId); 
            if(userInventory == null)
            {
                userInventory = new UserInventory();
                userInventory.ApplicationUserId = userId;
                userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
            }
            var model = new DashboardIndexViewModel();
            model.Name = userName;
            model.UserInventory = userInventory;
            model.CardMappings = await _DataAccess.CardDataAccess.GetCardMappingsForUserAsync(userInventory.Id);
            model.Decks = await _DataAccess.DeckDataAccess.GetDecksByUserAsync(userInventory.Id);
            model.DefaultImageBytes = defaultImage;
            model.TradeListings = tradeListings;
            model.Rating = rating;
            return View(model);
        }
    }
}
