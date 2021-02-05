using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Models.ViewModels
{
    public class CreateTradeOfferViewModel
    {
        public TradeOffer TradeOffer { get; set; }
        public TradeListing Listing { get; set; }
        public List<InventoryCardMapping> CardMappings { get; set; }
        public List<SelectListItem> CardOptions
        {
            get
            {
                if (CardMappings != null)
                {
                    var items = CardMappings
                        .Where(x => x.Count > 0)
                        .Select(x => new SelectListItem() { Text = x.Card?.Name ?? "", Value = x.CardId.ToString() })
                        .ToList();
                    items.Add(new SelectListItem() { Text = "No Card", Value = "0", Selected = true });

                    return items; 
                }
                else
                {
                    return new List<SelectListItem>() { new SelectListItem() { Text = "No Card", Value = "0", Selected = true }  };
                }
            }
        }
    }
}
