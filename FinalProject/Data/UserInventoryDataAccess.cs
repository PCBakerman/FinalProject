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

        
        public async Task<UserInventory> GetUserInventoryByUserAsync(string userId)
        {
            var userInventory = _context.UserInventories.FirstOrDefault(x => x.ApplicationUserId == userId);
            return userInventory;
        }

        public async Task<UserInventory> GetInventoryAsync(int id)
        {
            var userInventory = _context.UserInventories.FirstOrDefault(x => x.Id == id);
            return userInventory;
        }

        public async Task<UserInventory> Upsert(UserInventory userInventory)
        {
            try
            {
                if (userInventory.Id > 0)
                {
                    
                    var userInventoryData = _context.UserInventories.FirstOrDefault(x => x.Id == userInventory.Id);
                    if (userInventory != null)
                    {
                        userInventory.ApplicationUser = userInventory.ApplicationUser;
                        userInventory.Decks = userInventory.Decks;
                        await _context.SaveChangesAsync();
                        return userInventory;

                    }
                    else
                    {
                        _context.UserInventories.Add(userInventory);
                        await _context.SaveChangesAsync();
                        return userInventory;
                    }
                }
                else
                {
                    _context.UserInventories.Add(userInventory);
                    await _context.SaveChangesAsync();
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
