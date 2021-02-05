using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModels
{
    public class TradeListingsDetailViewModel
    {
        public bool IsUsers { get; set; }
        public TradeListing Listing { get; set; }
        public TradeOffer TradeOffer  { get; set; }
        public string Source { get; set; }
    }
}
