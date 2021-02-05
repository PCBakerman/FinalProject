using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;

namespace FinalProject.Data.Interfaces
{
    public interface ICardDataAccess
    {
        public Task<List<Card>> GetCardsAsync();
        public Task<Card> GetCardAsync(int id);
        public Task<Card> Upsert(Card card);
        public Task<Card> GetCardByNameAsync(string name);
        public Task<List<InventoryCardMapping>> GetCardMappingsForUserAsync(int userInventoryId);
        public Task<InventoryCardMapping> UpsertCardMappingAsync(InventoryCardMapping inventoryCardMapping);
        public Task<InventoryCardMapping> GetSpecificCardMappingForUserAsync(int userInventoryId, int cardId);
        public Task<bool> RemoveCardMappingForUser(int userInventoryId, int cardId);
    }
}
