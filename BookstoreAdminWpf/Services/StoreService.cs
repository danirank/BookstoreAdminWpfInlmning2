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

        public async Task<Store> GetStoreByID(int? storeId)
        {
            var store = await _db.Stores.FirstOrDefaultAsync(x=> x.StoreId == storeId);
            return store;
            
        }

        public async Task<bool> DeleteStoreAsync(int storeId)
        {
            var store = await _db.Stores.FindAsync(storeId);
            if (store == null)
            {
                return false;
            }
            _db.Remove(store);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Inventory>> GetInventoriesForStoreAsync(int storeId)
        {
            
        
            var inventory =  await _db.Inventories
                .Where(i => i.StoreId == storeId)
                .Include(i => i.Isbn13Navigation)          
                .ToListAsync();
        
            return inventory;
    }

        public async Task CreateNewStoreAsync(Store store)
        {
            if (store is not null)
            {
                await _db.Stores.AddAsync(store);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateBookAsync(Store store, int id)
        {
            if (store.StoreId != id)
            {
                return;
            }

            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
        }

    }
}
