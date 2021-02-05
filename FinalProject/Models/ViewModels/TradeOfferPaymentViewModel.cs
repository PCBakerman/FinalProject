using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe.Checkout;

namespace FinalProject.Models.ViewModels
{
    public class TradeOfferPaymentViewModel
    {
        public TradeOffer Offer { get; set; }
        public SessionLineItemPriceDataOptions PriceData { get;  set; }
    }
}
