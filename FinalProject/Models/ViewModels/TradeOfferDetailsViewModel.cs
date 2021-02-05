using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModels
{
    public class TradeOfferDetailsViewModel
    {
        public string Source { get; set; }
        public  bool OwnsListingOne { get; set; }
        public  bool OwnsListingTwo { get; set; }
        public TradeOffer Offer { get; set; }
    }
}
