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


        //Read all
        public async Task<List<Store>> GetAllStoresAsync()
        {
            var stores = await _db.Stores
                .Include(x=> x.Orders)
                .ThenInclude(x => x.OrderItems)
                .ToListAsync();

            return stores;
        }

        //Read by ID
        public async Task<Store> GetStoreByID(int? storeId)
        {
            var store = await _db.Stores.FirstOrDefaultAsync(x => x.StoreId == storeId);
            return store;

        }

        //Create Store
        public async Task CreateNewStoreAsync(Store store)
        {
            if (store is not null)
            {
                await _db.Stores.AddAsync(store);
                await _db.SaveChangesAsync();
            }
        }

        //Update Store
        public async Task UpdateStoreAsync(Store store, int id)
        {
            if (store.StoreId != id)
            {
                return;
            }

            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
        }

        //Delete Store
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

        //Get inventories for a specific store
        public async Task<List<Inventory>> GetInventoriesForStoreAsync(int storeId)
        {
            
        
            var inventory =  await _db.Inventories
                .Where(i => i.StoreId == storeId)
                .Include(i => i.Isbn13Navigation)          
                .ToListAsync();
        
            return inventory;
    }

       
        //Update or Add inventory row
        public async Task SaveInventoryAsync(Inventory inventory)
        {
            // Hämta ev. befintlig rad från DB
            var existing = await _db.Inventories
                .SingleOrDefaultAsync(i => i.StoreId == inventory.StoreId
                                           && i.Isbn13 == inventory.Isbn13);

            if (existing is null)
            {
                // Ny rad
                _db.Inventories.Add(inventory);
            }
            else
            {
               
                existing.Quantity = inventory.Quantity;
                 
                
            }

            await _db.SaveChangesAsync();
        }



        


    }
}
