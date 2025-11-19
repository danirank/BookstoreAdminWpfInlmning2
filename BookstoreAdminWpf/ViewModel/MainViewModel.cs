using BookstoreAdminWpf.Models;
using BookstoreAdminWpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAdminWpf.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
       
        private ObservableCollection<Book> _books = new();
        public ObservableCollection<Book> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }


        private ObservableCollection<Store> _stores = new();
        public ObservableCollection<Store> Stores
        {
            get => _stores;
            set
            {
                _stores = value;
                OnPropertyChanged(nameof(Stores));
            }
        }


        private ObservableCollection<Inventory> _inventory = new();
        public ObservableCollection<Inventory> Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                OnPropertyChanged(nameof(Inventory));
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
            
           var list =  await _storeService.GetInventoriesForStoreAsync(id);
        Inventory = new ObservableCollection<Inventory>(list);
        }
    }
}
