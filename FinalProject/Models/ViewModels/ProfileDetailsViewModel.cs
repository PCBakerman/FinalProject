using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModels
{
    public class ProfileDetailsViewModel
    {
        public bool IsOwn { get; set; }
        public bool IsFriend { get; set; }
        public List<TradeListing> Listings { get; set; }
        public UserInventory UserInventory { get; set; }
        public int LifePoints { get; set; }
        public string UserName { get; set; }
    }
}
