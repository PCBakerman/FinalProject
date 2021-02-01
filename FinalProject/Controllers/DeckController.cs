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

        // GET: DeckController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeckController/Create
        public async Task<ActionResult> Create(DeckDetailsViewModel model)
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
            return RedirectToAction("Create", model);
        }
        // POST: DeckController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: DeckController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeckController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
