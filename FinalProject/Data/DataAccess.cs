using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;

namespace FinalProject.Data
{
    public class DataAccess : IDataAccess
    {
        public ICardDataAccess CardDataAccess { get; set; } 
        public IDeckDataAccess DeckDataAccess { get; set; }
        public IUserInventoryDataAccess UserInventoryDataAccess { get; set; }
        public IYGOProDeckAccess YGOProDeckAccess { get; set; }
        public IDefaultImageDataAccess DefaultImageDataAccess { get; set; }
        public ITradeDataAccess TradeDataAccess { get; set; }
        public DataAccess(ApplicationDbContext context)
        {
            CardDataAccess = new CardDataAccess(context);
            DeckDataAccess = new DeckDataAccess(context);
            UserInventoryDataAccess = new UserInventoryDataAccess(context);
            DefaultImageDataAccess = new DefaultImageAccess(context);
            TradeDataAccess = new TradeDataAccess(context);

        }
    }
}
