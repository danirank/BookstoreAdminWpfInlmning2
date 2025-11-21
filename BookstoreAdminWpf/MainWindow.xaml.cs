using BookstoreAdminWpf.Services;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookstoreAdminWpf.Models;
using BookstoreAdminWpf.ViewModel;
using System.Threading.Tasks;
namespace BookstoreAdminWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _vm;
        private WriterService _writerService;
        private GenreService _genreService;
        private PublisherService _publisherService;

        public MainWindow()
        {
            InitializeComponent();

            var db = new AppDbContext();
            var bookService = new BookService(db);
            var storeService = new StoreService(db);
            _writerService = new WriterService(db);
            _genreService = new GenreService(db);
            _publisherService = new PublisherService(db);
            _vm = new MainViewModel(bookService, storeService);

            DataContext = _vm;

            Loaded += MainWindow_Loaded;

        }


        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _vm.LoadBooksAsync();
            await _vm.LoadAllStoresAsync();
        }

        private async void NewBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm) return;

            var newBook = new Book();

            var dialog = new EditAddBookWindow(newBook, _writerService, _genreService, _publisherService)
            {
                Owner = this
            };
            var result = dialog.ShowDialog();

            if (result == true)
            {

                await _vm.AddNewBookAsync(newBook);

            }
        }

        private async void EditSelectedBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm || vm.SelectedBook is null)
            {
                MessageBox.Show("Välj en bok först.");
                return;
            }

            var dialog = new EditAddBookWindow(vm.SelectedBook, _writerService, _genreService, _publisherService)
            {
                Owner = this
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {

                await _vm.UpdateBookAsync(vm.SelectedBook);

            }
        }

        private async void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedBook is null)
            {
                MessageBox.Show("Markera boken du vill ta bort");
                return;
            }

            await _vm.DeleteBookAsync(_vm.SelectedBook);

        }

        private async void EditStoreBtn_Click(object sender, RoutedEventArgs e)
        {

            EditStorePanel.Visibility = Visibility.Visible;
            var btn = sender as Button;

            if (btn != null)
            {
                if(btn.Tag.ToString() =="2")
                {
                    _vm.SelectedStore = null;
                    return;
                }
                if (btn.Tag.ToString() == "3")
                {
                  await  _vm.DeleteStoreAsync(_vm.SelectedStore);
                    EditStorePanel.Visibility = Visibility.Hidden;
                }
            }



            

        }
    

        private async void ConfirmStoreUpdate_Click(object sender, RoutedEventArgs e)
        {
            int? id = _vm.SelectedStore?.StoreId;
           
            bool storeExists = await _vm.StoreExistsInDb(_vm.SelectedStore, id);

            

            if (_vm.Stores.Any(i=> i.StoreId == _vm.SelectedStore?.StoreId) && storeExists) 
            {
                await UpdateStoreInfo();
            } else
            {
               await NewStoreOnClick();
            }

            EditStorePanel.Visibility = Visibility.Hidden;
        }


        private async Task UpdateStoreInfo()
        {
            ConfirmStoreUpdate.Content = "Uppdatera";
            if (_vm.SelectedStore is null)
            {
                MessageBox.Show("Välj en butik att redigera");
                return;
            }
            Store store = _vm.SelectedStore;

            await _vm.UpdateStoreAsync(store);
            
        }

        private async Task NewStoreOnClick()
        {
            ConfirmStoreUpdate.Content = "Spara ny butik";

            Store newStore = new Store();

            newStore.Name = StoreNamebox.Text;
            newStore.Country = StoreCountryBox.Text;
            newStore.Street = StoreStreetBox.Text;
            newStore.City = StoreCityBox.Text;
            newStore.PostalCode = StorePostalCodeBox.Text;

            await _vm.AddNewStoreAsync(newStore);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            EditStorePanel.Visibility = Visibility.Hidden;
        }


        //private async void StoresGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (StoresGrid.SelectedItem is Store store)
        //    {
        //        InventoryHeader.Text = "Lager: "+ store.Name;
        //        var id = store.StoreId;
        //        await _vm.ShowInventoryAsync(id);
        //    }
        //}
    }
}