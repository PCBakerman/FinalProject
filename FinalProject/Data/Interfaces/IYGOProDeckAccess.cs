using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Data.Interfaces
{
    public interface IYGOProDeckAccess
    {
        public Task<List<Card>> SearchCardsAsync([FromQuery] string q);
    }
}
