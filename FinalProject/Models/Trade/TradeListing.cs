using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class TradeListing
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        
        [ForeignKey("Card")]
        public int CardId { get; set; }
        public Card Card { get; set; }
        [NotMapped]
        public bool HasCashOffer { get { return CashOffer > 0; } }
        [NotMapped]
        public int UserRating { get; set; }
        [NotMapped]
        public string Email { get; set; }
        public double CashOffer { get; set; }
        public string CashOfferDisplay { get
            {
                return HasCashOffer ? $"${CashOffer.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}" : "";
            } }

        public TradeState TradeState { get; set; }
    }
}
