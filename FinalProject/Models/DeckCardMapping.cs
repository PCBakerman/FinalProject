using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class DeckCardMapping
    {
        [Key, Column(Order = 0)]
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        [Key, Column(Order = 1)]
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int Count { get; set; }
    }
}
