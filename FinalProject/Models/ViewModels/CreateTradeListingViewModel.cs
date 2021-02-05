using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Models.ViewModels
{
    public class CreateTradeListingViewModel
    {
        public List<InventoryCardMapping> CardMappings { get; set; }
        public TradeListing TradeListing { get; set; }
        public string Source { get; set; }
        public List<SelectListItem> CardOptions { 
            get
            {
                if (CardMappings != null)
                {
                    return CardMappings
                        .Where(x => x.Count > 0)
                        .Select(x => new SelectListItem() { Text = x.Card?.Name ?? "", Value = x.CardId.ToString() })
                        .ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
        }
    }
}
