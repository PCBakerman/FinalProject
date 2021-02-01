using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModels
{
    public class DashboardIndexViewModel
    {
        public string Name { get; set; }
        public UserInventory UserInventory { get; set; }
        public List<InventoryCardMapping> CardMappings { get; set; }
        public List<Deck> Decks { get; set; }

    }
}
