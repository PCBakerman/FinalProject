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
    }
}
