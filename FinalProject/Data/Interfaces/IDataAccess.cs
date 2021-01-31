using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data.Interfaces
{
    public interface IDataAccess 
    {
        public ICardDataAccess CardDataAccess { get; set; }
        public IDeckDataAccess DeckDataAccess { get; set; }
        public IUserInventoryDataAccess UserInventoryDataAccess { get; set; }
    }
}
