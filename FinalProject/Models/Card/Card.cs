using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public string DescriptionDisplay { get { return Description?.Substring(0, (Description.Length >= 200 ? 200:Description.Length)); } }
        public int Attack { get; set; }
        [NotMapped]
        public string AttackDisplay { get { return Attack <= 0 ? string.Empty:Attack.ToString(); } }
        public int Defence { get; set; }
        [NotMapped]
        public string DefenseDisplay { get { return Defence <= 0 ? string.Empty : Defence.ToString(); } }
        public int Level { get; set; }
        [NotMapped]
        public string LevelDisplay { get { return Level <= 0 ? string.Empty : Level.ToString(); } }
        public string Race { get; set; }
        public string Attribute { get; set; }
        public string Archetype { get; set; }
        public int Linkval { get; set; }
        [NotMapped]
        public string LinkvalDisplay { get { return Linkval <= 0 ? string.Empty : Linkval.ToString(); } }
        public string LinkMarkers { get; set; }
        public byte[] ImageBytes { get; set; }

        public List<CardSet> CardSets { get; set; }
        public List<CardImage> CardImages { get; set; }
        public List<CardPrice> CardPrices { get; set; }


    }
}
