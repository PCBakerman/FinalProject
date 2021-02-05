using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data.Interfaces;

namespace FinalProject.Data
{
    public class DefaultImageAccess : IDefaultImageDataAccess
    {
        private ApplicationDbContext _context;
        public DefaultImageAccess(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<byte[]> GetDefaultImageByNameBytesAync(string name)
        {
            var bytes = new byte[0];
            var imageRecord = _context.DefaultImages.FirstOrDefault(x => x.Name == name);
            if(imageRecord != null)
            {
                bytes = imageRecord.ImageBytes;
            }
            return bytes;
        }
        public async Task<byte[]> GetDefaultImageByIdBytesAync(int id)
        {
            var bytes = new byte[0];
            var imageRecord = _context.DefaultImages.FirstOrDefault(x => x.Id == id);
            if (imageRecord != null)
            {
                bytes = imageRecord.ImageBytes;
            }
            return bytes;
        }
    }
}
