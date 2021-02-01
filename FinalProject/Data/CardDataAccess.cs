using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using FinalProject.Data.Interfaces;

namespace FinalProject.Data
{
    public class CardDataAccess : ICardDataAccess
    {
        private ApplicationDbContext _context;
        public CardDataAccess(ApplicationDbContext context)
        {
            _context = context;

        }
        
        public async Task<List<Card>> GetCardsAsync()
        {
            var cards = _context.Cards.ToList();
            return cards;
        }

        
        public async Task<Card> GetCardAsync(int id)
        {
            var card = _context.Cards.FirstOrDefault(x => x.Id == id);
            return card;
        }
        public async Task<Card> GetCardByNameAsync(string name)
        {
            var card = _context.Cards.FirstOrDefault(x => x.Name == name);
            return card;
        }

        public async Task<Card> Upsert(Card card)
        {
            try
            {
                if(card.Id > 0)
                {
                    var cardData = _context.Cards.FirstOrDefault(x => x.Id == card.Id);
                    if(cardData != null)
                    {
                        cardData.Archetype = card.Archetype;
                        cardData.Attack = card.Attack;
                        cardData.Attribute = card.Attribute;
                        cardData.CardImages = card.CardImages;
                        cardData.CardPrices = card.CardPrices;
                        cardData.CardSets = card.CardSets;
                        cardData.Defence = card.Defence;
                        cardData.Description = card.Description;
                        cardData.Level = card.Level;
                        cardData.LinkMarkers = card.LinkMarkers;
                        cardData.Linkval = card.Linkval;
                        cardData.Name = card.Name;
                        cardData.Race = card.Race;
                        cardData.Type = card.Type;
                        await _context.SaveChangesAsync();

                        return cardData;

                    }
                    else
                    {
                        card.Id = 0;
                        _context.Cards.Add(card);
                        await _context.SaveChangesAsync();
                        return card;
                    }
                }
                else
                {
                    _context.Cards.Add(card);
                    await _context.SaveChangesAsync();
                    return card;
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<InventoryCardMapping>> GetCardMappingsForUserAsync(int userInventoryId)
        {
            var mappings = _context.InventoryCardMappings.Where(x => x.UserInventoryId == userInventoryId).ToList();
            foreach (var mapping in mappings)
            {
                mapping.Card = await GetCardAsync(mapping.CardId);
            }
            return mappings;

        }
        public async Task<InventoryCardMapping> UpsertCardMappingAsync(InventoryCardMapping inventoryCardMapping)
        {
            var mapping = _context.InventoryCardMappings.FirstOrDefault(x => x.UserInventoryId == inventoryCardMapping.UserInventoryId && x.CardId == inventoryCardMapping.CardId);
            if(mapping != null)
            {
                mapping.Count = inventoryCardMapping.Count;

            }
            else
            {
                mapping = new InventoryCardMapping();
                mapping.Count = 1;
                mapping.CardId = inventoryCardMapping.CardId;
                mapping.UserInventoryId = inventoryCardMapping.UserInventoryId;
                _context.InventoryCardMappings.Add(mapping);

            }
            await _context.SaveChangesAsync();
            return mapping;     
        }
        public async Task<InventoryCardMapping> GetSpecificCardMappingForUserAsync(int userInventoryId, int cardId)
        {
            var mapping = _context.InventoryCardMappings.FirstOrDefault(x => x.UserInventoryId == userInventoryId && x.CardId == cardId);
            return mapping;

        }
    }
}
