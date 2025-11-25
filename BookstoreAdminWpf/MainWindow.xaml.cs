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
                if (btn.Tag.ToString() == "1")
                {
                    ConfirmStoreUpdate.Content = "Uppdatera butik";
                }
                if (btn.Tag.ToString() == "2")
                {
                    _vm.SelectedStore = null;
                    ConfirmStoreUpdate.Content = "Spara ny butik";
                    return;
                }
                if (btn.Tag.ToString() == "3")
                {
                    await _vm.DeleteStoreAsync(_vm.SelectedStore);
                    EditStorePanel.Visibility = Visibility.Hidden;
                }
            }





        }


        private async void ConfirmStoreUpdate_Click(object sender, RoutedEventArgs e)
        {
            int? id = _vm.SelectedStore?.StoreId;

            bool storeExists = await _vm.StoreExistsInDb(_vm.SelectedStore, id);



            if (_vm.Stores.Any(i => i.StoreId == _vm.SelectedStore?.StoreId) && storeExists)
            {
                await UpdateStoreInfo();
            }
            else
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
            AddInventoryGrid.Visibility = Visibility.Collapsed;
            MoveInventoryGrid.Visibility = Visibility.Collapsed;
        }


        private async void ConfirmAddMoveBookToInventory_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {

                var tag = btn.Tag.ToString();


                if (_vm.SelectedStore is not Store store || _vm.SelectedBook is not Book book)
                {
                    return;

                }



                //Samma som Tag 2 "From store vid move book"

                int storeId = store.StoreId;
                string isbn = book.Isbn13;
                try
                {
                    switch (tag)
                    {
                        case "1": //Add book to inventory

                            if (!int.TryParse(QuantityBox.Text.Trim(), out var quantity) || quantity < 1)
                            {
                                MessageBox.Show("Antal måste vara en siffra (1 eller större) ");
                                return;
                            }

                            await _vm.AddBookToInventoryAsync(storeId, quantity, isbn);
                            MessageBox.Show("Uppdatering lyckades");

                            break;

                        case "2": //Movebook between stores

                            if (!int.TryParse(QuantityToMoveBox.Text.Trim(), out var quantityToMove))
                            {
                                MessageBox.Show("Ogiltligt antal att flytta");
                                return;
                            }

                            bool removedSucces = await _vm.RemoveBooksFromInventoryAsync(storeId, quantityToMove, isbn);

                            if (removedSucces)
                            {

                                var toStore = _vm.Stores.Where(s => s.StoreId == (int)ToStoreBox.SelectedValue).FirstOrDefault();
                                
                                if (toStore is null)
                                {
                                    //Lägger tillbaka bok 
                                    await _vm.AddBookToInventoryAsync(storeId, quantityToMove, isbn);

                                    MessageBox.Show($"Något gick fel när bok skule flyttas till Affär");
                                    return;
                                }


                                await _vm.AddBookToInventoryAsync(toStore.StoreId, quantityToMove, isbn);
                            }

                            break;
                        default:
                            throw new Exception("Okänt kommando");


                    }




                    await _vm.ShowInventoryAsync(storeId);

                    AddInventoryGrid.Visibility = Visibility.Collapsed;
                    MoveInventoryGrid.Visibility = Visibility.Collapsed;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }



        private void AddBooksToInventory_Click(object sender, RoutedEventArgs e)
        {
            AddInventoryGrid.Visibility = Visibility.Visible;
        }

        private void MovebooksBtn_Click(object sender, RoutedEventArgs e)
        {
            MoveInventoryGrid.Visibility = Visibility.Visible;
        }
    }
}