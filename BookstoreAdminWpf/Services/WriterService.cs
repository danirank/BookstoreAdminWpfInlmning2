using BookstoreAdminWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAdminWpf.Services
{
    public class WriterService
    {
        private readonly AppDbContext _db;

        public WriterService(AppDbContext db)
        {
            _db = db;
        }
        //Get all writers 
        public async Task<List<Writer>> GetAllWritersAsync()
        {
            var result = await _db.Writers.ToListAsync();

            return result; 
        }

        //Post writer 

        public async Task AddNewWriterAsync(Writer writer)
        {
            await _db.Writers.AddAsync(writer);
            await _db.SaveChangesAsync();

        }

        //Delete Writer
        public async Task<bool> DeleteWriterAsync(int writerId)
        {
            var writer = await _db.Writers.FindAsync(writerId);

            if (writer is null)
            {
                return false;
            }

             _db.Writers.Remove(writer);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
