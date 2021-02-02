using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using FinalProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Data.Interfaces
{
    public interface IDeckDataAccess 
    {
        public Task<List<Deck>> GetDecksAsync();
        public Task<Deck> GetDeckAsync(int id);
        public Task<Deck> Upsert(Deck deck);
        public Task<List<Deck>> GetDecksByUserAsync(int userInventoryId);
        public Task<DeckCardMapping> GetDeckCardMappingAsync(int deckId, int cardId);
        public Task<DeckCardMapping> UpsertDeckCardMappingAsync(DeckCardMapping deckCardMapping);
        
    }
}
