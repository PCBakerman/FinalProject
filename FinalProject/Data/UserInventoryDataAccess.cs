using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class UserInventoryDataAccess : IUserInventoryDataAccess
    {
        private ApplicationDbContext _context;
        public UserInventoryDataAccess(ApplicationDbContext context)
        {
            _context = context;

        }
        private FriendBlockItem PopulateFriendBlockListItem(FriendBlockItem item)
        {
            if(item != null)
            {
                if (item.UserInventorTwoId > 0)
                    item.UserInventoryTwo = PopulateUserInventory(_context.UserInventories.FirstOrDefault(x => x.Id == item.UserInventorTwoId));
                if (item.UserInventoryOneId > 0)
                    item.UserInventoryOne = PopulateUserInventory(_context.UserInventories.FirstOrDefault(x => x.Id == item.UserInventoryOneId)) ;
            }
            return item;
        }
        private UserInventory PopulateUserInventory(UserInventory inventory)
        {
            var decks = _context.Decks.Where(x => x.UserInventoryId == inventory.Id).ToList();
            inventory.Decks = decks;
            var user = _context.Users.FirstOrDefault(x => x.Id == inventory.ApplicationUserId);
            inventory.Email = user.Email;
            return inventory;
        }
        public async Task<List<FriendBlockItem>> GetFriends(int userInventoryId)
        {
            var items = _context.FriendBlockItems.Where(x => (x.UserInventorTwoId == userInventoryId || x.UserInventoryOneId == userInventoryId) && x.Status == FriendBlockStatus.Friends).ToList();
            var populatedItems = new List<FriendBlockItem>();
            foreach(var item in items)
            {
                populatedItems.Add(PopulateFriendBlockListItem(item));
            }
            return populatedItems;
        }
        public async Task<List<FriendBlockItem>> GetPendingRequests(int userInventoryId)
        {
            var items = _context.FriendBlockItems.Where(x => (x.UserInventorTwoId == userInventoryId || x.UserInventoryOneId == userInventoryId) && x.Status == FriendBlockStatus.Pending).ToList();
            var populatedItems = new List<FriendBlockItem>();
            foreach (var item in items)
            {
                populatedItems.Add(PopulateFriendBlockListItem(item));
            }
            return populatedItems;
        }
        public async Task<List<FriendBlockItem>> GetBlocks(int userInventoryId)
        {
            var items = _context.FriendBlockItems.Where(x => (x.UserInventorTwoId == userInventoryId || x.UserInventoryOneId == userInventoryId) && x.Status == FriendBlockStatus.Blocked).ToList();
            var populatedItems = new List<FriendBlockItem>();
            foreach (var item in items)
            {
                populatedItems.Add(PopulateFriendBlockListItem(item));
            }
            return populatedItems;
        }
        public async Task<FriendBlockItem> GetFriendBlockItem(int idOne, int idTwo)
        {
            var item = _context.FriendBlockItems.FirstOrDefault(x => (x.UserInventorTwoId == idOne || x.UserInventoryOneId == idOne) && (x.UserInventorTwoId == idTwo || x.UserInventoryOneId == idTwo));
            if (item != null)
                return PopulateFriendBlockListItem(item);
            else
                return item;
        }
        public async Task<FriendBlockItem> UpsertFriendBlockListItem(FriendBlockItem item)
        {
            var idOne = item.UserInventoryOneId;
            var idTwo = item.UserInventorTwoId;
            var itemData = _context.FriendBlockItems.FirstOrDefault(x => (x.UserInventorTwoId == idOne || x.UserInventoryOneId == idOne) && (x.UserInventorTwoId == idTwo || x.UserInventoryOneId == idTwo));
            if (itemData != null)
            {
                itemData.Status = item.Status;
                _context.SaveChanges();
                return PopulateFriendBlockListItem(itemData);
            }
            else
            {
                _context.FriendBlockItems.Add(item);
                _context.SaveChanges();
                return PopulateFriendBlockListItem(item);
            }
        }
        public async Task<UserInventory> GetUserInventoryByUserAsync(string userId)
        {
            var userInventory = _context.UserInventories.FirstOrDefault(x => x.ApplicationUserId == userId);
            if (userInventory != null)
                return PopulateUserInventory(userInventory);
            return userInventory;
        }

        public async Task<UserInventory> GetInventoryAsync(int id)
        {
            var userInventory = _context.UserInventories.FirstOrDefault(x => x.Id == id);
            if (userInventory != null)
                return PopulateUserInventory(userInventory);
            return userInventory;
        }

        public async Task<UserInventory> Upsert(UserInventory userInventory)
        {
            try
            {
                if (userInventory.Id > 0)
                {
                    
                    var userInventoryData = _context.UserInventories.FirstOrDefault(x => x.Id == userInventory.Id);
                    if (userInventoryData != null)
                    {
                        userInventoryData.ApplicationUser = userInventory.ApplicationUser;
                        userInventoryData.Decks = userInventory.Decks;
                        userInventoryData.AddressLine = userInventory.AddressLine;
                        userInventoryData.City = userInventory.City;
                        userInventoryData.State = userInventory.State;
                        userInventoryData.Zip = userInventory.Zip;
                        await _context.SaveChangesAsync();
                        if (userInventory != null)
                            return PopulateUserInventory(userInventory);
                        return userInventory;

                    }
                    else
                    {
                        _context.UserInventories.Add(userInventory);
                        await _context.SaveChangesAsync();
                        if (userInventory != null)
                            return PopulateUserInventory(userInventory);
                        return userInventory;
                    }
                }
                else
                {
                    _context.UserInventories.Add(userInventory);
                    await _context.SaveChangesAsync();
                    if (userInventory != null)
                        return PopulateUserInventory(userInventory);
                    return userInventory;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
