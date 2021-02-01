using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModels
{
    public class SearchDetailsViewModel
    {
        public string Query { get; set; }
        public string Name { get; set; }
        public Card Result { get; set; }

    }
}
