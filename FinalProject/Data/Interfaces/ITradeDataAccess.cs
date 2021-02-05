using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;

namespace FinalProject.Data.Interfaces
{
    public interface ITradeDataAccess
    {
        public Task<int> GetUserRating(string userId);
        public Task<TradeListing> UpsertTradeListing(TradeListing tradeListing);
        public Task<List<TradeListing>> GetTradeListingsByUser(string userId);
        public Task<List<TradeListing>> GetTradeListingsByCard(int cardId);
        public Task<int> GetUserTradeListingLimit(string userId);
        public Task<int> GetActiveTradeListingCountByUser(string userId);
        public Task<TradeListing> GetTradeListing(int id);
        public Task<List<TradeListing>> GetTradeListings();
        public Task<bool> DeleteTradeListing(int id);
    }
}
