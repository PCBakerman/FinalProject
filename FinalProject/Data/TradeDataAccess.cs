using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class TradeDataAccess : ITradeDataAccess
    {
        public static bool IsStateDeletable(TradeState state)
        {
            switch (state) 
            {
                case TradeState.Listed:
                case TradeState.Offered:
                case TradeState.PendingOffer:
                case TradeState.Denied:
                    return true;
                default:
                    return false;
            }
        }
        private ApplicationDbContext _context;
        public TradeDataAccess(ApplicationDbContext context)
        {
            _context = context;

        }
        private async Task<TradeListing> PopulateListingData(TradeListing listing)
        {
            if (listing.CardId > 0)
                listing.Card = _context.Cards.Find(listing.CardId);
            var user = _context.Users.FirstOrDefault(x=>x.Id == listing.ApplicationUserId);
            listing.Email = user.Email;
            listing.UserRating = await GetUserRating(listing.ApplicationUserId);
            return listing;
        }
        public async Task<List<TradeListing>> GetTradeListings()
        {
            var listings = _context.TradeListings.ToList();
            var filledListings = new List<TradeListing>();
            foreach (var listing in listings)
            {
                filledListings.Add(await PopulateListingData(listing));
            }
            return filledListings;
        }
        public async Task<List<TradeListing>> GetTradeListingsByUser(string userId)
        {
            var listings = _context.TradeListings.Where(x => x.ApplicationUserId == userId).ToList();
            var filledListings = new List<TradeListing>();
            foreach (var listing in listings)
            {
                filledListings.Add(await PopulateListingData(listing));
            }
            return filledListings;
        }

        public async Task<TradeListing> GetTradeListing(int id)
        {
            var listing = _context.TradeListings.FirstOrDefault(x => x.Id == id);
            if(listing != null)
            {
                listing = await PopulateListingData(listing);
            }
            return listing;
        }
        public async Task<int> GetActiveTradeListingCountByUser(string userId)
        {
            var listings = _context.TradeListings.Where(x => x.ApplicationUserId == userId && (x.TradeState == TradeState.Listed || x.TradeState == TradeState.PendingOffer)).ToList();
            return listings.Count;
        }
        public async Task<List<TradeListing>> GetTradeListingsByCard(int cardId)
        {
            var listings =  _context.TradeListings.Where(x => x.CardId == cardId).ToList();
            var filledListings = new List<TradeListing>();
            foreach (var listing in listings)
            {
                filledListings.Add(await PopulateListingData(listing));
            }
            return filledListings;
        }
        public async Task<TradeListing> UpsertTradeListing(TradeListing tradeListing)
        {
            if(tradeListing.Id > 0)
            {
                var dataTradeListing = _context.TradeListings.Find(tradeListing.Id);
                if (dataTradeListing != null)
                {
                    dataTradeListing.CardId = tradeListing.CardId;
                    dataTradeListing.CashOffer = tradeListing.CashOffer;
                    dataTradeListing.Description = tradeListing.Description;
                    await _context.SaveChangesAsync();
                    dataTradeListing = await PopulateListingData(dataTradeListing);
                    return dataTradeListing;
                }
                else
                {
                    _context.TradeListings.Add(tradeListing);
                    await _context.SaveChangesAsync();
                    tradeListing = await PopulateListingData(tradeListing);
                    return tradeListing;
                }
            }
            else
            {
                try
                {
                    _context.TradeListings.Add(tradeListing);
                    _context.SaveChanges();
                    tradeListing = await PopulateListingData(tradeListing);
                }
                catch(Exception e)
                {

                }
                return tradeListing;
            }
        }
        public async Task<int> GetUserTradeListingLimit(string userId)
        {
            var listings = _context.TradeListings.Where(x => x.ApplicationUserId == userId).ToList();
            var ratings = new List<int>();
            if (listings.Count > 0)
            {
                foreach (var listing in listings)
                {
                    var offers = _context.TradeOffers.Where(x => x.TradeState == Models.TradeState.Completed && x.TradeListingOneId == listing.Id);
                    foreach (var offer in offers)
                    {
                        ratings.Add(offer.TradeListingOneRating);
                    }
                    offers = _context.TradeOffers.Where(x => x.TradeState == Models.TradeState.Completed && x.TradeListingTwoId == listing.Id);
                    foreach (var offer in offers)
                    {
                        ratings.Add(offer.TradeListingTwoRating);
                    }
                }
            }

            // prob need some way to factor in amount of trades rather than just an avg rating
            int rating = 0;
            int ratingCount = 0;
            foreach (var userRating in ratings)
            {
                if (userRating > 0)
                {
                    ratingCount++;
                    rating += userRating;
                }
            }
            var userRatingTotal = ratingCount <= 0 ? 0 : rating / ratingCount;

            if (userRatingTotal <= 3000)
                return 20;
            else if (userRatingTotal <= 6000)
                return 30;
            else if (userRatingTotal <= 7000)
                return 40;
            else if (userRatingTotal <= 7500)
                return 50;
            else if (userRatingTotal < 8000)
                return 60;
            else
                return 80;
        }
        public async Task<int> GetUserRating(string userId)
        {
            var listings = _context.TradeListings.Where(x => x.ApplicationUserId == userId).ToList();
            var ratings = new List<int>();
            if(listings.Count > 0)
            {
                foreach(var listing in listings) 
                {
                    var offers = _context.TradeOffers.Where(x => x.TradeState == Models.TradeState.Completed && x.TradeListingOneId == listing.Id);
                    foreach( var offer in offers)
                    {
                        ratings.Add(offer.TradeListingOneRating);
                    }
                    offers = _context.TradeOffers.Where(x => x.TradeState == Models.TradeState.Completed && x.TradeListingTwoId == listing.Id);
                    foreach (var offer in offers)
                    {
                        ratings.Add(offer.TradeListingTwoRating);
                    }
                }
            }
            int rating = 0;
            int ratingCount = 0;
            foreach(var userRating in ratings)
            {
                if(userRating > 0)
                {
                    ratingCount++;
                    rating += userRating;
                }
            }
            return ratingCount <= 0 ? 0 :  rating / ratingCount;
        }

        public async Task<bool> DeleteTradeListing(int id)
        {
            var listing = _context.TradeListings.FirstOrDefault(x => x.Id == id);
            if(listing != null)
            {
                _context.TradeListings.Remove(listing);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
