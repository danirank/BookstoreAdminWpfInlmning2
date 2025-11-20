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
                }
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
        public async Task LoadBooksAsync()
        {
            var list = await _bookService.GetAllBooksAsync();
            Books = new ObservableCollection<Book>(list);
        }

        public async Task LoadAllStoresAsync()
        {
            var list = await _storeService.GetAllStoresAsync();
            Stores = new ObservableCollection<Store>(list);
        }

        public async Task ShowInventoryAsync(int id)
        {

            var list = await _storeService.GetInventoriesForStoreAsync(id);
            Inventory = new ObservableCollection<Inventory>(list);
        }

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
    }
}
