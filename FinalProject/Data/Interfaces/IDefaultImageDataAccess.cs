using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data.Interfaces
{
    public interface IDefaultImageDataAccess
    {
        public Task<byte[]> GetDefaultImageByNameBytesAync(string name);
        public Task<byte[]> GetDefaultImageByIdBytesAync(int id);
    }
}
