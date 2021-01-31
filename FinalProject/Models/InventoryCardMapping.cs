using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class InventoryCardMapping
    {
        [Key, Column(Order = 0)]
        public int UserInventoryId { get; set; }
        public UserInventory UserInventory { get; set; }
        [Key, Column(Order = 1)]
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int Count { get; set; }  
    }
}
