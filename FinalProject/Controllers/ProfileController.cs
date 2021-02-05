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
using FinalProject.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Controllers
{
    public class ProfileController : Controller
    {
        private IDataAccess _DataAccess;
        public ProfileController(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;

        }

        public async Task<ActionResult> AddFriend(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userI
            var UserInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            var otherUserInventory = await _DataAccess.UserInventoryDataAccess.GetInventoryAsync(id ?? 0);
            if (UserInventory == null || otherUserInventory == null)
            {
                return NotFound();
            }
            var item = await _DataAccess.UserInventoryDataAccess.GetFriendBlockItem(otherUserInventory.Id, UserInventory.Id);
            if(item == null)
            {
                item = new FriendBlockItem();
                item.UserInventorTwoId = id ?? 0;
                item.UserInventoryOneId = UserInventory.Id;
                item.Status = FriendBlockStatus.Pending;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            else
            {
                item.Status = item.Status == FriendBlockStatus.Pending ? FriendBlockStatus.Friends : FriendBlockStatus.Pending;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            return RedirectToAction("Index", "Dashboard");

        }
        public async Task<ActionResult> RemoveFriend(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userI
            var UserInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            var otherUserInventory = await _DataAccess.UserInventoryDataAccess.GetInventoryAsync(id ?? 0);
            if (UserInventory == null || otherUserInventory == null)
            {
                return NotFound();
            }
            var item = await _DataAccess.UserInventoryDataAccess.GetFriendBlockItem(otherUserInventory.Id, UserInventory.Id);
            if (item == null)
            {
                item = new FriendBlockItem();
                item.UserInventorTwoId = id ?? 0;
                item.UserInventoryOneId = UserInventory.Id;
                item.Status = FriendBlockStatus.Neutral;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            else
            {
                item.Status = FriendBlockStatus.Neutral;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            return RedirectToAction("Index", "Dashboard");

        }
        public async Task<ActionResult> BlockUser(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userI
            var UserInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            var otherUserInventory = await _DataAccess.UserInventoryDataAccess.GetInventoryAsync(id ?? 0);
            if (UserInventory == null || otherUserInventory == null)
            {
                return NotFound();
            }
            var item = await _DataAccess.UserInventoryDataAccess.GetFriendBlockItem(otherUserInventory.Id, UserInventory.Id);
            if (item == null)
            {
                item = new FriendBlockItem();
                item.UserInventorTwoId = id ?? 0;
                item.UserInventoryOneId = UserInventory.Id;
                item.Status = FriendBlockStatus.Blocked;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            else
            {
                item.Status = FriendBlockStatus.Blocked;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            return RedirectToAction("Index", "Dashboard");

        }
        public async Task<ActionResult> UnBlockUser(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userI
            var UserInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            var otherUserInventory = await _DataAccess.UserInventoryDataAccess.GetInventoryAsync(id ?? 0);
            if (UserInventory == null || otherUserInventory == null)
            {
                return NotFound();
            }
            var item = await _DataAccess.UserInventoryDataAccess.GetFriendBlockItem(otherUserInventory.Id, UserInventory.Id);
            if (item == null)
            {
                item = new FriendBlockItem();
                item.UserInventorTwoId = id ?? 0;
                item.UserInventoryOneId = UserInventory.Id;
                item.Status = FriendBlockStatus.Neutral;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            else
            {
                item.Status = FriendBlockStatus.Neutral;
                await _DataAccess.UserInventoryDataAccess.UpsertFriendBlockListItem(item);
            }
            return RedirectToAction("Index", "Dashboard");
        }
        // GET: Profile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new ProfileDetailsViewModel();
            model.UserInventory = await _DataAccess.UserInventoryDataAccess.GetInventoryAsync(id ?? 0);
            if (model.UserInventory == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            model.LifePoints = await _DataAccess.TradeDataAccess.GetUserRating(model.UserInventory.ApplicationUserId);
            model.IsOwn = userId == model.UserInventory?.ApplicationUserId && !string.IsNullOrEmpty(userId);
            model.Listings = await _DataAccess.TradeDataAccess.GetTradeListingsByUser(model.UserInventory?.ApplicationUserId);
            model.UserName = model.UserInventory?.Email;
            return View(model);
        }
        // GET: Profile/DetailsUser/5
        public async Task<IActionResult> DetailsUser(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new ProfileDetailsViewModel();
            model.UserInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(id);
            if (model.UserInventory == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            model.LifePoints = await _DataAccess.TradeDataAccess.GetUserRating(id);
            model.IsOwn = userId == model.UserInventory?.ApplicationUserId && !string.IsNullOrEmpty(userId);
            model.Listings = await _DataAccess.TradeDataAccess.GetTradeListingsByUser(model.UserInventory?.ApplicationUserId);
            model.UserName = model.UserInventory?.Email;
            return View("Details", model);

        }
        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit()
        {
            var model = new ProfileDetailsViewModel();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            model.UserInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
            if (model.UserInventory == null)
            {
                return NotFound();
            }
            model.LifePoints = await _DataAccess.TradeDataAccess.GetUserRating(model.UserInventory.ApplicationUserId);
            model.IsOwn = userId == model.UserInventory?.ApplicationUserId && !string.IsNullOrEmpty(userId);
            if (!model.IsOwn)
            {
                return Forbid();
            }
            model.Listings = await _DataAccess.TradeDataAccess.GetTradeListingsByUser(model.UserInventory?.ApplicationUserId);
            model.UserName = model.UserInventory?.Email;
            return View(model);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection)
        {
            var addressLine = collection["UserInventory.AddressLine"];
            var strIsPublic = collection["UserInventory.IsAddressPublic"][0];
            var city = collection["UserInventory.City"];
            var state = collection["UserInventory.State"];
            var zip = collection["UserInventory.Zip"];
            var strId = collection["UserInventory.Id"];
            int id;
            if(int.TryParse(strId, out id))
            {
                var inventory = await _DataAccess.UserInventoryDataAccess.GetInventoryAsync(id);
                if(inventory == null)
                {
                    return NotFound();
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                if(string.IsNullOrEmpty(userId) || userId != inventory?.ApplicationUserId)
                {
                    return Forbid();
                }
                inventory.AddressLine = addressLine;
                inventory.City = city;
                inventory.State = state;
                inventory.Zip = zip;
                bool ispublic;
                if (bool.TryParse(strIsPublic, out ispublic))
                {
                    inventory.IsAddressPublic = ispublic;
                }
                await _DataAccess.UserInventoryDataAccess.Upsert(inventory);
                var model = new { id = inventory.Id };
                return RedirectToAction("Details", model);
            }
            return NotFound();
        }

    }
}
