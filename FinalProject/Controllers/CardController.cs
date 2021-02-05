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

namespace FinalProject.Controllers
{
    public class CardController : Controller
    {
        private IDataAccess _DataAccess;
        public CardController(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;
        }

        // GET: Card
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            if (userInventory == null)
            {
                userInventory = new UserInventory();
                userInventory.ApplicationUserId = userId;
                userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
            }
            var CardMappings = await _DataAccess.CardDataAccess.GetCardMappingsForUserAsync(userInventory.Id);
            return View(CardMappings);
        }

        // GET: Card/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            if (userInventory == null)
            {
                userInventory = new UserInventory();
                userInventory.ApplicationUserId = userId;
                userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
            }
            var CardMapping = await _DataAccess.CardDataAccess.GetSpecificCardMappingForUserAsync(userInventory.Id, id ?? 0);
            return View(CardMapping);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            if (userInventory == null)
            {
                userInventory = new UserInventory();
                userInventory.ApplicationUserId = userId;
                userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
            }
            await _DataAccess.CardDataAccess.RemoveCardMappingForUser(userInventory.Id, id ?? 0);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DecreaseCardCount(int? id, string source)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            if (userInventory == null)
            {
                userInventory = new UserInventory();
                userInventory.ApplicationUserId = userId;
                userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
            }
            var CardMapping = await _DataAccess.CardDataAccess.GetSpecificCardMappingForUserAsync(userInventory.Id, id ?? 0);
            if(CardMapping != null)
            {
                CardMapping.Count--;
                if(CardMapping.Count < 0)
                {
                    await _DataAccess.CardDataAccess.RemoveCardMappingForUser(userInventory.Id, id ?? 0);
                    return RedirectToAction("Index");
                }
                else
                {
                    await _DataAccess.CardDataAccess.UpsertCardMappingAsync(CardMapping);
                    if (source == "index")
                        return RedirectToAction("Index");
                    else
                    {
                        var obj = new
                        {
                            id = id
                        };
                        return RedirectToAction("Details", obj);
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> IncreaseCardCount(int? id, string source)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            if (userInventory == null)
            {
                userInventory = new UserInventory();
                userInventory.ApplicationUserId = userId;
                userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
            }
            var CardMapping = await _DataAccess.CardDataAccess.GetSpecificCardMappingForUserAsync(userInventory.Id, id ?? 0);
            if (CardMapping != null)
            {
                CardMapping.Count++;
                await _DataAccess.CardDataAccess.UpsertCardMappingAsync(CardMapping);
                if(source == "index")
                    return RedirectToAction("Index");
                else
                {
                    var obj = new
                    {
                        id = id
                    };
                    return RedirectToAction("Details", obj);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
