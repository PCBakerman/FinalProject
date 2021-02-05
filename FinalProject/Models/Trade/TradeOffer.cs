using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class TradeOffer
    {
        public int Id { get; set; }
        public int TradeListingOneId { get; set; }
        public TradeListing TradeListingOne { get; set; }
        public int TradeListingTwoId { get; set; }
        public TradeListing TradeListingTwo { get; set; }
        public TradeState TradeState { get; set; }
        public int TradeListingOneRating { get; set; }
        public int TradeListingTwoRating { get; set; }
    }
    public enum TradeState
    {
        Listed,
        PendingOffer,
        Offered,
        Accepted,
        Denied,
        AwaitingPayment,
        PaymentComplete,
        Shipped,
        Completed
    }
}
