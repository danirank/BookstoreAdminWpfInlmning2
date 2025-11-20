using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookstoreAdminWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAdminWpf.Services
{
    public class PublisherService
    {
        private readonly AppDbContext _db; 

        public PublisherService(AppDbContext db)
        {
            _db = db;
        }

        //Get publishers
        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            var publishers = await _db.Publishers.ToListAsync();
            return publishers;
        }

        //Save new publisher
        public async Task AddNewGenreAsync(Publisher publisher)
        {
            await _db.Publishers.AddAsync(publisher);
            await _db.SaveChangesAsync();

        }
    }
}
