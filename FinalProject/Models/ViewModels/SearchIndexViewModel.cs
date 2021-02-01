using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModels
{
    public class SearchIndexViewModel
    {
        public string Query { get; set; }
        public List<Card> Results { get; set; }
    }
}
