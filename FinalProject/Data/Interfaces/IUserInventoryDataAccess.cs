using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;

namespace FinalProject.Data.Interfaces
{
    public interface IUserInventoryDataAccess
    {
        public Task<UserInventory> GetUserInventoryByUserAsync(string userId);
        public Task<UserInventory> GetInventoryAsync(int id);
        public Task<UserInventory> Upsert(UserInventory userInventory);
        public Task<FriendBlockItem> UpsertFriendBlockListItem(FriendBlockItem item);
        public Task<FriendBlockItem> GetFriendBlockItem(int idOne, int idTwo);
        public Task<List<FriendBlockItem>> GetBlocks(int userInventoryId);
        public Task<List<FriendBlockItem>> GetPendingRequests(int userInventoryId);
        public Task<List<FriendBlockItem>> GetFriends(int userInventoryId);
    }
}
