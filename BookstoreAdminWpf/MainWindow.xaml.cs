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
        public MainWindow()
        {
            InitializeComponent();
            
            var db = new AppDbContext();
            var bookService = new BookService(db);
            var storeService = new StoreService(db);
            
             _vm = new MainViewModel(bookService,storeService);

            DataContext = _vm;

            Loaded += MainWindow_Loaded;
           
        }

       
       private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _vm.LoadBooksAsync();
            await _vm.LoadAllStoresAsync();
        }

        private async void StoresGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StoresGrid.SelectedItem is Store store)
            {
                InventoryHeader.Text = "Lager: "+ store.Name;
                var id = store.StoreId;
                await _vm.ShowInventoryAsync(id);
            }
        }
    }
}