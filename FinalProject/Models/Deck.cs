using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public int UserInventoryId { get; set; }
        public UserInventory UserInventory { get; set; }
        public string DeckName { get; set; }
        public string BuildPurpose { get; set; }
        public string DeckImage { get; set; }
        public List<DeckCardMapping> DeckCardMappings { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
