using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookstoreAdminWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAdminWpf.Services
{
    public class StoreService
    {
        private readonly AppDbContext _db;
        
        public StoreService(AppDbContext db)
        {
            _db = db;
        }

        
        
       public async Task<List<Store>> GetAllStoresAsync()
        {
            var stores = await _db.Stores.ToListAsync();

            return stores;
        }

        public async Task<List<Inventory>> GetInventoriesForStoreAsync(int storeId)
        {
            
        
            var inventory =  await _db.Inventories
                .Where(i => i.StoreId == storeId)
                .Include(i => i.Isbn13Navigation)          
                .ToListAsync();
        
            return inventory;
    }

}
}
