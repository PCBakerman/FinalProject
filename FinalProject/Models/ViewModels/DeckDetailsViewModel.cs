using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Models.ViewModels
{
    public class DeckDetailsViewModel
    {
        public Deck Deck { get; set; }
        public List<InventoryCardMapping> CardMappings { get; set; }
        public string CardIdToAdd { get; set; }
        public List<SelectListItem> Cards { get
            {
                return CardMappings?.Select(x => new SelectListItem()
                {
                    Value = x.CardId.ToString(),
                    Text = $"{x.Card.Name} Type: {x.Card.Type}"
                }).ToList() ?? new List<SelectListItem>();
            } }
        public string DeckId { get; set; }

        public byte[] DefaultImageBytes { get; set; }
        
    }
}
