using BookstoreAdminWpf.Models;
using BookstoreAdminWpf.Services;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookstoreAdminWpf.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;

            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Book> _books = new();
        public ObservableCollection<Book> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }


        private Store? _selectedStore;
        public Store? SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                OnPropertyChanged();

                if (value != null)
                {
                    
                    _ = ShowInventoryAsync(value.StoreId);
                   
                } else
                {
                    TotalInventoryValue = 0;
                }
            }
        }

        private decimal _totalInventoryValue;
        public decimal TotalInventoryValue
        {
            get => _totalInventoryValue;
            set
            {
                _totalInventoryValue = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Store> _stores = new();
        public ObservableCollection<Store> Stores
        {
            get => _stores;
            set
            {
                _stores = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<Inventory> _inventory = new();
        public ObservableCollection<Inventory> Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }

        
        
        private BookService _bookService;
        private StoreService _storeService;
        
        
        public MainViewModel(BookService bookService, StoreService storeService)
        {
            _bookService = bookService;
            _storeService = storeService;
            
            
        }

        //Book
        public async Task AddNewBookAsync(Book book)
        {
            try
            {
                if (book == null)
                {
                    return;
                }

                await _bookService.CreateBookAsync(book);
                Books.Add(book);
                SelectedBook = book;
                MessageBox.Show("Created new book: " + book.Title);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel vid skapandet av ny bok " + ex.Message);
            }

        }
        public async Task DeleteBookAsync(Book book)
        {
          

            var mbResult = MessageBox.Show(
                $"Är du säker på att du vill ta bort boken med titel {book.Title}",
                "Radera bok", MessageBoxButton.YesNo,
                MessageBoxImage.Warning
                );

            if (mbResult == MessageBoxResult.Yes)
            {
                bool deleted = await _bookService.DeleteBookAsync(book.Isbn13);

                if (deleted)
                {
                    MessageBox.Show("Bok raderad");
                    Books.Remove(book);
                    SelectedBook = null;
                }
                else
                {
                    MessageBox.Show("Misslyckades att radera bok");
                }

            }
        }
        public async Task LoadBooksAsync()
        {
            var list = await _bookService.GetAllBooksAsync();
            Books = new ObservableCollection<Book>(list);
        }
        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                if(book == null || SelectedBook == null)
                {
                    MessageBox.Show("Book is null");
                    return;
                }

                await _bookService.UpdateBookAsync(book, SelectedBook.Isbn13);
                await LoadBooksAsync();

                MessageBox.Show($"Bok med titel {book.Title} uppdaterades"); 
                 
            } catch (Exception ex)
            {
                MessageBox.Show($"Fel vid uppdatering av bok:\n{ex.Message}");
            }
        } 

        //Store

        public async Task LoadAllStoresAsync()
        {
            var list = await _storeService.GetAllStoresAsync();
            Stores = new ObservableCollection<Store>(list);
        }

        public async Task UpdateStoreAsync(Store store)
        {

            try
            {
                if (store == null || SelectedStore == null)
                {
                    MessageBox.Show("Store is null");
                    return;
                }

                await _storeService.UpdateStoreAsync(store, SelectedStore.StoreId);
                await LoadAllStoresAsync();

                MessageBox.Show($"Butik med namn {store.Name} uppdaterades");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fel vid uppdatering av butik:\n{ex.Message}");
            }
        }

        public async Task AddNewStoreAsync(Store store)
        {
            try
            {
              await _storeService.CreateNewStoreAsync(store); 
                Stores.Add(store);
            } catch(Exception ex)
            {
                MessageBox.Show("Something went wrong \n" +  ex.Message);
            }
        }

        public async Task ShowInventoryAsync(int storeId)
        {

            var list = await _storeService.GetInventoriesForStoreAsync(storeId);
            Inventory = new ObservableCollection<Inventory>(list);

            if (SelectedStore != null)
            {
                TotalInventoryValue = SelectedStore.GetTotalInventoryValue();
                
            }
        }

        public async Task AddBookToInventoryAsync(int storeId, int quantity, string isbn13)
        {
            try
            {
             await ShowInventoryAsync(storeId);
            var inventory = Inventory.SingleOrDefault(i => i.StoreId == storeId && i.Isbn13 == isbn13);

            if (inventory is null)
            {
                    Inventory newInventory = new Inventory()
                    {
                        Isbn13 = isbn13,
                        Quantity = quantity,
                        StoreId = storeId
                    };

                    await _storeService.SaveInventoryAsync(newInventory);

                }
                else
                {

                        inventory.Quantity += quantity;
                        await _storeService.SaveInventoryAsync(inventory);
                }

            

            
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public async Task<bool> RemoveBooksFromInventoryAsync(int storeId, int quantity, string isbn13)
        {
            
            var inventory = Inventory.FirstOrDefault(i => i.StoreId == storeId && i.Isbn13 == isbn13);

            if (inventory is null)
            {
                
                return false;
            }

            if (quantity < 1)
            {
               
                return false;
            }

            if (inventory.Quantity < quantity)
            {
                MessageBox.Show("Inte tillräckligt med böcker att flytta");
                return false;
            }

            
            inventory.Quantity -= quantity;

            await _storeService.SaveInventoryAsync(inventory);

            return true;
        }


        public async Task<bool> StoreExistsInDb(Store? store, int? storeId)
        {
            if (store == null || storeId == null) return false;
           
            var dbStore = await _storeService.GetStoreByID(storeId);
            

            return dbStore.StoreId == store.StoreId;
        } 

        public async Task DeleteStoreAsync(Store store)
        {
            if (SelectedStore is null)
            {
                MessageBox.Show("Välj butiken du vill radera");
                return;
            }

            var mbResult = MessageBox.Show(
                $"Är du säker på att du vill ta bort butiken med namn {store.Name}",
                "Radera butik", MessageBoxButton.YesNo,
                MessageBoxImage.Warning
                );

            if (mbResult == MessageBoxResult.Yes)
            {
                bool deleted = await _storeService.DeleteStoreAsync(SelectedStore.StoreId);

                if (deleted)
                {
                    MessageBox.Show("Butik raderad");
                    Stores.Remove(store);
                    SelectedStore = null;
                }
                else
                {
                    MessageBox.Show("Misslyckades att radera nutik");
                }

            }
        }


    }
}
