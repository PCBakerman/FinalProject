using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;
using FinalProject.Models;
using FinalProject.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class DeckController : Controller
    {
        private IDataAccess _DataAccess;
        public DeckController(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;

        }
        // GET: DeckController
        public async Task<ActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
                var decks = await _DataAccess.DeckDataAccess.GetDecksByUserAsync(userInventory.Id);
                var cards = await _DataAccess.CardDataAccess.GetCardMappingsForUserAsync(userInventory.Id);
                var model = new DeckIndexViewModel();
                model.Decks = decks;
                model.CardMappings = cards;
                return View(model);
            }
        }
        public async Task<IActionResult> CardDetails(string cardId, string deckId)
        {
            var model = new DeckCardDetailsViewModel();
            int convertedCardId;
            int convertedDeckId;  
            if (int.TryParse(cardId, out convertedCardId) && int.TryParse(deckId, out convertedDeckId))
            {
                var card = await _DataAccess.CardDataAccess.GetCardAsync(convertedCardId);
                model.Card = card;
                model.DeckId = deckId;
                return View("CardDetails", model);
            }
            else
            {
                var detailsModel = new DeckDetailsViewModel();
                detailsModel.DeckId = deckId;
                return RedirectToAction("Details", detailsModel);
            }
           
        }
        // GET: DeckController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeckController/Edit
        public async Task<ActionResult> Edit(DeckDetailsViewModel model)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                if (model != null)
                {
                    int deckId;
                    if (int.TryParse(model.DeckId, out deckId))
                    {
                        model.Deck = await _DataAccess.DeckDataAccess.GetDeckAsync(deckId);
                        if (model.Deck == null)
                        {
                            model = null;
                        }
                    }
                    else
                    {
                        model = null;
                    }   
                }
                var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
                var cards = await _DataAccess.CardDataAccess.GetCardMappingsForUserAsync(userInventory.Id);
                if (model == null)
                {
                    model = new DeckDetailsViewModel();
                    var deck = new Deck();
                    deck.UserInventoryId = userInventory.Id;
                    deck = await _DataAccess.DeckDataAccess.Upsert(deck);
                    model.Deck = deck;
                }
                model.CardMappings = cards;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddCard(DeckDetailsViewModel model)
        {
            int cardId;
            int deckId;
            if (int.TryParse(model.CardIdToAdd, out cardId) && int.TryParse(model.DeckId, out deckId))
            {
                var mapping = await _DataAccess.DeckDataAccess.GetDeckCardMappingAsync(deckId, cardId);
                if (mapping != null)
                {
                    mapping.Count++;
                    await _DataAccess.DeckDataAccess.UpsertDeckCardMappingAsync(mapping);
                }
                else
                {
                    mapping = new DeckCardMapping();
                    mapping.CardId = cardId;
                    mapping.DeckId = deckId;
                    mapping.Count = 1;
                    await _DataAccess.DeckDataAccess.UpsertDeckCardMappingAsync(mapping);

                }
            }
            return RedirectToAction("Edit", model);
        }
        // POST: DeckController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection)
        {
            try
            {
                var deckName = collection["Deck.DeckName"];
                var buildPrupose = collection["Deck.BuildPurpose"];
                var stringdeckId = collection["Deck.Id"];
                int deckId;
                if (int.TryParse(stringdeckId, out deckId))
                {
                    var deck = await _DataAccess.DeckDataAccess.GetDeckAsync(deckId);
                    deck.DeckName = deckName;
                    deck.BuildPurpose = buildPrupose;
                    await _DataAccess.DeckDataAccess.Upsert(deck);
                    var model = new DeckDetailsViewModel();
                    model.DeckId = stringdeckId;

                    return RedirectToAction("Edit", model);
                }
                    
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DeckController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeckController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
