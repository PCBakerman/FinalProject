using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class DeckDataAccess : IDeckDataAccess
    {
        private ApplicationDbContext _context;
        public DeckDataAccess(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<List<Deck>> GetDecksAsync()
        {
            var decks = _context.Decks.ToList();
            return decks;
        }
        public async Task<List<Deck>> GetDecksByUserAsync(int userInventoryId)
        {
            var decks = _context.Decks.Where(x => x.UserInventoryId == userInventoryId).ToList();
            return decks;
        }

        public async Task<Deck> GetDeckAsync(int id)
        {
            var deck = _context.Decks.FirstOrDefault(x => x.Id == id);
            deck = await PopulateCardMappings(deck);
            return deck;
        }
        private async Task<Deck> PopulateCardMappings(Deck deck)
        {
            deck.DeckCardMappings = _context.DeckCardMappings.Where(x => x.DeckId == deck.Id).ToList();
            foreach (var card in deck.DeckCardMappings)
            {
                card.Card = _context.Cards.FirstOrDefault(x => x.Id == card.CardId);
            }
            return deck;
        }

        public async Task<Deck> Upsert(Deck deck)
        {
            try
            {
                if (deck.Id > 0)
                {
                    var deckData = _context.Decks.FirstOrDefault(x => x.Id == deck.Id);
                    if (deckData != null)
                    {
                        deckData.DeckCardMappings = deck.DeckCardMappings;
                        deckData.DeckImage = deck.DeckImage;
                        deckData.DeckName = deck.DeckName;
                        deckData.UserInventory = deck.UserInventory;
                        deckData.UserInventoryId = deck.UserInventoryId;
                        await _context.SaveChangesAsync();
                        deckData = await PopulateCardMappings(deckData);
                        return deckData;

                    }
                    else
                    {
                        _context.Decks.Add(deck);
                        await _context.SaveChangesAsync();
                        deck = await PopulateCardMappings(deck);
                        return deck;
                    }
                }
                else
                {
                    _context.Decks.Add(deck);
                    await _context.SaveChangesAsync();
                    deck = await PopulateCardMappings(deck);
                    return deck;
                }
            }
            catch
            {
                return null;
            }

        }
        public async Task<DeckCardMapping> GetDeckCardMappingAsync(int deckId, int cardId)
        {
            var mapping = _context.DeckCardMappings.FirstOrDefault(x => x.DeckId == deckId && x.CardId == cardId);
            return mapping;

        }
        public async Task<DeckCardMapping> UpsertDeckCardMappingAsync(DeckCardMapping deckCardMapping)
        {
            var mapping = _context.DeckCardMappings.FirstOrDefault(x => x.DeckId == deckCardMapping.DeckId && x.CardId == deckCardMapping.CardId);
            if (mapping != null)
            {
                mapping.Count = deckCardMapping.Count;

            }
            else
            {
                mapping = new DeckCardMapping();
                mapping.Count = 1;
                mapping.CardId = deckCardMapping.CardId;
                mapping.DeckId = deckCardMapping.DeckId;
                _context.DeckCardMappings.Add(mapping);

            }
            await _context.SaveChangesAsync();
            return mapping;
        }
        public async Task<bool> DeleteDeckAsync(int id)
        {
            var deck = _context.Decks.FirstOrDefault(x => x.Id == id);
            if (deck != null)
            {
                // deck exists so remove it
                _context.Decks.Remove(deck);
                await _context.SaveChangesAsync();
                // return true so we know it was deleted
                return true;
            }
            else
            {
                // deck not found so return false so we know it wasn't deleted
                return false;
            }
        }
    }
}
