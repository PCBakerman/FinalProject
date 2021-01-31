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
    }
}
