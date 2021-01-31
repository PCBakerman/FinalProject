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
                        return deckData;

                    }
                    else
                    {
                        _context.Decks.Add(deck);
                        await _context.SaveChangesAsync();
                        return deck;
                    }
                }
                else
                {
                    _context.Decks.Add(deck);
                    await _context.SaveChangesAsync();
                    return deck;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
