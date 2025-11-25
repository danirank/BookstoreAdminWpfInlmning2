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
        public async Task AddNewPubslisherAsync(Publisher publisher)
        {
            await _db.Publishers.AddAsync(publisher);
            await _db.SaveChangesAsync();

        }

        //Delete Publisher
        public async Task<bool> DeletePublisherAsync(int publisherId)
        {
            var publisher = await _db.Publishers.FindAsync(publisherId);

            if (publisher is null)
            {
                return false;
            }

            _db.Publishers.Remove(publisher);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
