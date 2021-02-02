using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Models.ViewModels
{
    public class SearchDetailsViewModel
    {
        public string Query { get; set; }
        public string Name { get; set; }
        public Card Result { get; set; }
        public List<Deck> Decks { get; set; }
        public string DeckId { get; set; }
        public string CardId { get; set; }
        public List<SelectListItem> DeckSelector
        {
            get
            {
                return Decks?.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.DeckName}"
                }).ToList() ?? new List<SelectListItem>();
            }
        }
    }
}
