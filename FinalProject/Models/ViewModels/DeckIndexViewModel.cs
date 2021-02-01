using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModels
{
    public class DeckIndexViewModel
    {
        public List<Deck> Decks { get; set; }
        public List<InventoryCardMapping> CardMappings { get; set; }
    }
}
