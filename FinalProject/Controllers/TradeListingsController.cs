using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Data.Interfaces;
using System.Security.Claims;
using FinalProject.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Controllers
{
    public class TradeListingsController : Controller
    {
        private IDataAccess _DataAccess;
        public TradeListingsController(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;
        }

        // GET: TradeListings
        public async Task<ViewResult> Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.OfferSortParam = sortOrder == "offer" ? "offer_desc" : "offer";
            ViewBag.AttackSortParam = sortOrder == "attack" ? "attack_desc" : "attack";
            ViewBag.TypeSortParam = sortOrder == "type" ? "type_desc" : "type";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listings = await _DataAccess.TradeDataAccess.GetTradeListingsByUser(userId);
            switch (sortOrder)
            {
                case "status":
                    listings = listings.OrderBy(s => s.TradeState).ToList();
                    break;
                case "status_desc":
                    listings = listings.OrderByDescending(s => s.TradeState).ToList();
                    break;
                case "offer":
                    listings = listings.OrderBy(s => s.CashOffer).ToList();
                    break;
                case "offer_desc":
                    listings = listings.OrderByDescending(s => s.CashOffer).ToList();
                    break;
                case "attack":
                    listings = listings.OrderBy(s => s.Card.Attack).ToList();
                    break;
                case "attack_desc":
                    listings = listings.OrderByDescending(s => s.Card.Attack).ToList();
                    break;
                case "type":
                    listings = listings.OrderBy(s => s.Card.Type).ToList();
                    break;
                case "type_desc":
                    listings = listings.OrderByDescending(s => s.Card.Type).ToList();
                    break;
                case "name_desc":
                    listings = listings.OrderByDescending(s => s.Card.Name).ToList();
                    break;
                default:
                    listings = listings.OrderBy(s => s.Card.Name).ToList();
                    break;
            }
            return View(listings);
        }
        // GET: TradeListings
        public async Task<IActionResult> Browse(string sortOrder)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.OfferSortParam = sortOrder == "offer" ? "offer_desc" : "offer";
            ViewBag.AttackSortParam = sortOrder == "attack" ? "attack_desc" : "attack";
            ViewBag.TypeSortParam = sortOrder == "type" ? "type_desc" : "type";
            ViewBag.DefenseSortParam = sortOrder == "defense" ? "defense_desc" : "defense";
            ViewBag.LevelSortParam = sortOrder == "level" ? "level_desc" : "level";
            ViewBag.RaceSortParam = sortOrder == "race" ? "Race_desc" : "race";
            ViewBag.AttributeSortParam = sortOrder == "attr" ? "attr_desc" : "attr";
            ViewBag.LinkValSortParam = sortOrder == "linkval" ? "linkval_desc" : "linkval";
            ViewBag.DescSortParam = sortOrder == "desc" ? "desc_desc" : "desc";
            ViewBag.UserSortParam = sortOrder == "user" ? "user_desc" : "user";
            ViewBag.RatingSortParam = sortOrder == "rating" ? "rating_desc" : "rating";
            var listings = await _DataAccess.TradeDataAccess.GetTradeListings();
            var inventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            listings = listings.Where(x => TradeDataAccess.IsStateDeletable(x.TradeState) && x.ApplicationUserId != userId).ToList();
            if (inventory != null)
            {
                var blockList = await _DataAccess.UserInventoryDataAccess.GetBlocks(inventory.Id);
                listings = listings.Where(x => !blockList.Any(y => y.UserInventoryOne.ApplicationUserId == x.ApplicationUserId || y.UserInventoryTwo.ApplicationUserId == x.ApplicationUserId)).ToList();
            }
            switch (sortOrder)
            {
                case "desc":
                    listings = listings.OrderBy(s => s.Description).ToList();
                    break;
                case "desc_desc":
                    listings = listings.OrderByDescending(s => s.Description).ToList();
                    break;
                case "rating":
                    listings = listings.OrderBy(s => s.UserRating).ToList();
                    break;
                case "rating_desc":
                    listings = listings.OrderByDescending(s => s.UserRating).ToList();
                    break;
                case "user":
                    listings = listings.OrderBy(s => s.Email).ToList();
                    break;
                case "user_desc":
                    listings = listings.OrderByDescending(s => s.Email).ToList();
                    break;
                case "attack":
                    listings = listings.OrderBy(s => s.Card.Attack).ToList();
                    break;
                case "attack_desc":
                    listings = listings.OrderByDescending(s => s.Card.Attack).ToList();
                    break;
                case "defense":
                    listings = listings.OrderBy(s => s.Card.Defence).ToList();
                    break;
                case "defense_desc":
                    listings = listings.OrderByDescending(s => s.Card.Defence).ToList();
                    break;
                case "attr":
                    listings = listings.OrderBy(s => s.Card.Attribute).ToList();
                    break;
                case "attr_desc":
                    listings = listings.OrderByDescending(s => s.Card.Attribute).ToList();
                    break;
                case "level":
                    listings = listings.OrderBy(s => s.Card.Level).ToList();
                    break;
                case "level_desc":
                    listings = listings.OrderByDescending(s => s.Card.Level).ToList();
                    break;
                case "race":
                    listings = listings.OrderBy(s => s.Card.Race).ToList();
                    break;
                case "race_desc":
                    listings = listings.OrderByDescending(s => s.Card.Race).ToList();
                    break;
                case "linkval":
                    listings = listings.OrderBy(s => s.Card.Linkval).ToList();
                    break;
                case "linkval_desc":
                    listings = listings.OrderByDescending(s => s.Card.Linkval).ToList();
                    break;
                case "type":
                    listings = listings.OrderBy(s => s.Card.Type).ToList();
                    break;
                case "type_desc":
                    listings = listings.OrderByDescending(s => s.Card.Type).ToList();
                    break;
                case "name_desc":
                    listings = listings.OrderByDescending(s => s.Card.Name).ToList();
                    break;
                default:
                    listings = listings.OrderBy(s => s.Card.Name).ToList();
                    break;
            }
            return View(listings);
        }

        // GET: TradeListings
        public async Task<IActionResult> Search()
        {

            return View();
        }

        // GET: TradeListings/Details/5
        public async Task<IActionResult> Details(int? id, string source)
        {
            var model = new TradeListingsDetailViewModel();
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tradeListing = await _DataAccess.TradeDataAccess.GetTradeListing(id ?? 0);
            if (userId != tradeListing.ApplicationUserId)
                model.IsUsers = false;
            else
                model.IsUsers = true;
            if (tradeListing == null)
            {
                return NotFound();
            }
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            if (userInventory == null)
            {
                userInventory = new UserInventory();
                userInventory.ApplicationUserId = userId;
                userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
            }
            model.Listing = tradeListing;
            model.Source = source;
            return View(model);
        }

        // GET: TradeListings/Create
        public async Task<IActionResult> Create(string source)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                var totalListings = await _DataAccess.TradeDataAccess.GetActiveTradeListingCountByUser(userId);
                var listingLimit = await  _DataAccess.TradeDataAccess.GetUserTradeListingLimit(userId);

                if(totalListings >= listingLimit)
                {
                    var parameters = new { source = source };

                    return RedirectToAction("TradeLimitExceeded", parameters);
                }

                var model = new CreateTradeListingViewModel();
                var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
                if (userInventory == null)
                {
                    userInventory = new UserInventory();
                    userInventory.ApplicationUserId = userId;
                    userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
                }
                model.CardMappings = await _DataAccess.CardDataAccess.GetCardMappingsForUserAsync(userInventory.Id);
                model.TradeListing = new TradeListing();
                model.Source = source;
                return View(model);
            }
        }

        // POST: TradeListings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                var totalListings = await _DataAccess.TradeDataAccess.GetActiveTradeListingCountByUser(userId);
                var listingLimit = await _DataAccess.TradeDataAccess.GetUserTradeListingLimit(userId);

                if (totalListings >= listingLimit)
                {
                    var parameters = new { source = collection["Source"] };

                    return RedirectToAction("TradeLimitExceeded", parameters);
                }

                var strCardId = collection["TradeListing.CardId"];
                var desc = collection["TradeListing.Description"];
                int cardId;
                if(int.TryParse(strCardId, out cardId))
                {
                    var tradeListing = new TradeListing();
                    tradeListing.ApplicationUserId = userId;
                    tradeListing.CardId = cardId;
                    tradeListing.Description = desc;
                    var tradeListingData = _DataAccess.TradeDataAccess.UpsertTradeListing(tradeListing);
                    var source = collection["Source"];
                    if(source == "Dashboard")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> TradeLimitExceeded(string source)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home", "Index");
            }
            var totalListings = await _DataAccess.TradeDataAccess.GetActiveTradeListingCountByUser(userId);
            var listingLimit = await _DataAccess.TradeDataAccess.GetUserTradeListingLimit(userId);
            var model = new TradeLimitExceededViewModel();
            model.Source = source;
            model.CurrentTradeCount = totalListings;
            model.Limit = listingLimit;
            return View(model);
        }
        public IActionResult AcceptTradeLimit(string source)
        {
            if (source == "Dashboard")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        // GET: TradeListings/Edit/5
        public async Task<IActionResult> Edit(int? id, string source)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Forbid();
            }
            else
            {
                var tradeListing = await _DataAccess.TradeDataAccess.GetTradeListing(id ?? 0);
                if (userId != tradeListing.ApplicationUserId)
                    return Forbid();
                if (tradeListing == null)
                {
                    return NotFound();
                }
                var model = new CreateTradeListingViewModel();
                var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
                if (userInventory == null)
                {
                    userInventory = new UserInventory();
                    userInventory.ApplicationUserId = userId;
                    userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
                }
                model.CardMappings = await _DataAccess.CardDataAccess.GetCardMappingsForUserAsync(userInventory.Id);
                model.TradeListing = tradeListing;
                model.Source = source;
                return View(model);
            }
        }
        
        public IActionResult EditCancel(string source)
        {
            switch (source)
            {
                case "Dashboard":
                    return RedirectToAction("Index", "Dashboard");
                case "Index":
                default:
                    return RedirectToAction("Index");

            }
        }
        public IActionResult DetailsCancel(string source)
        {
            switch (source) 
            {
                case "Dashboard":
                    return RedirectToAction("Index", "Dashboard");
                case "Index":
                default:
                    return RedirectToAction("Index");

            }
        }
        // POST: TradeListings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var source = collection["Source"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                var totalListings = await _DataAccess.TradeDataAccess.GetActiveTradeListingCountByUser(userId);
                var listingLimit = await _DataAccess.TradeDataAccess.GetUserTradeListingLimit(userId);

                if (totalListings >= listingLimit)
                {
                    var parameters = new { source = collection["Source"] };

                    return RedirectToAction("TradeLimitExceeded", parameters);
                }

                var strCardId = collection["TradeListing.CardId"];
                var strId = collection["TradeListing.Id"];
                var desc = collection["TradeListing.Description"];
                int cardId;
                int id;
                if (int.TryParse(strCardId, out cardId))
                {
                    if(int.TryParse(strId, out id))
                    {
                        var tradeListing = await _DataAccess.TradeDataAccess.GetTradeListing(id);
                        if(tradeListing != null)
                        {
                            tradeListing.CardId = cardId;
                            tradeListing.Description = desc;
                            var tradeListingData = await _DataAccess.TradeDataAccess.UpsertTradeListing(tradeListing);
                            if (source == "Dashboard")
                            {
                                return RedirectToAction("Index", "Dashboard");
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }  
                    }
                   
                }
            }
            if (source == "Dashboard")
                return RedirectToAction("Index", "Dashboard");
            return RedirectToAction("Index");
        }

        // GET: TradeListings/Delete/5
        public async Task<IActionResult> Delete(int? id, string source)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeListing = await _DataAccess.TradeDataAccess.GetTradeListing(id ?? 0);

            if (tradeListing == null)
            {
                return NotFound();
            }
            else if(TradeDataAccess.IsStateDeletable(tradeListing.TradeState))
            {
                await _DataAccess.TradeDataAccess.DeleteTradeListing(id ?? 0);
            }
            else
            {
                // Maybe need error message about why trade state is not deletable
            }
            return  source == "Dashboard" ? RedirectToAction("Index", "Dashboard") : RedirectToAction("Index");
        }

    }
}
