using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookstoreAdminWpf.Models;
using BookstoreAdminWpf.ViewModel;
using BookstoreAdminWpf.Services;
using System.Globalization;
using System.Text.RegularExpressions;
namespace BookstoreAdminWpf
{
    /// <summary>
    /// Interaction logic for EditAddBookWindow.xaml
    /// </summary>
    public partial class EditAddBookWindow : Window
    {

        public EditAddBookViewModel Vm;
        public EditAddBookWindow(Book? book, WriterService writerService, GenreService genreService, PublisherService publisherService)
        {
            InitializeComponent();

            Vm = new EditAddBookViewModel(book, writerService, genreService, publisherService);
            DataContext = Vm;
            Loaded += LoadWritersAndGenresAsync;
        }

        //Load data
        private async void LoadWritersAndGenresAsync(object sender, RoutedEventArgs e)
        {
            await Vm.LoadAllWritersAsync();
            await Vm.LoadAllGenresAsync();
            await Vm.LoadAllPublishersAsync();
        }

        //Save new data
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn != null)
            {

                switch (btn.Tag.ToString())
                {
                    case "1":
                        SaveBookOnClick();
                        DialogResult = true;
                        Close();

                        break;

                    case "2":
                        await SaveGenreOnClick(); 
                        break;

                    case "3":
                        await SaveWriterOnClick();
                        break;
                    case "4":
                        await SavePublisherOnClick();

                        break;

                    default: return;

                }



               
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn != null)
            {
                switch (btn.Tag.ToString())
                {
                    case "1":
                        DialogResult = false;
                        Close();
                        break;
                    case "2":
                      NewGenreGrid.Visibility = Visibility.Collapsed;

                        break;
                    case "3":
                          NewWriterGrid.Visibility = Visibility.Collapsed;
                        break;
                    case"4":
                        NewPublisherGrid.Visibility = Visibility.Collapsed;
                        break;


                        default: return;
                }
            }

        }


       //Save edited book
        private void SaveBookOnClick()
        {
            if (Vm.Book is null)
                return;

            string rawIsbn = ISBN13Box.Text.Trim();
            string cleanedIsbn = rawIsbn.Replace("-", "");

            if (!Regex.IsMatch(cleanedIsbn, @"^\d{13}$"))
            {
                MessageBox.Show("ISBN13 måste bestå av exakt 13 siffror (bindestreck ignoreras).");
                return;
            }

            if (Vm.Book.Isbn13 == null)
            {
                Vm.Book.Isbn13 = cleanedIsbn;

            }
            Vm.Book.Title = TitleBox.Text;


            if (!decimal.TryParse(PriceBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                MessageBox.Show("Ogiltigt pris.");
                return;
            }

            Vm.Book.Price = price;

            if (Vm.SelectedGenre is null)
            {
                MessageBox.Show("Välj Genre");
                return;
            }
            Vm.Book.Genre = Vm.SelectedGenre;

            if (Vm.SelectedWriter is null)
            {
                MessageBox.Show("Välj Writer");
                return;
            }

            Vm.Book.Writer = Vm.SelectedWriter;

            if (Vm.SelectedPublisher is null)
            {
                MessageBox.Show("Välj publisher");
                return;
            }
            Vm.Book.Publisher = Vm.SelectedPublisher;

        }     

        //Save new Genre Move to VM?
        private async Task SaveGenreOnClick()
        {
            Genre newGenre = new Genre();

            newGenre.Name = NewGenreBox.Text;

            await Vm.SaveNewGenreAsync(newGenre);

            Vm.SelectedGenre = newGenre;

            NewGenreGrid.Visibility = Visibility.Collapsed;

        }

        //Save new Writer Move to VM?
        private async Task SaveWriterOnClick()
        {
            Writer newWriter = new Writer();

            newWriter.FirstName = NewWriterFirstNameBox.Text;
            newWriter.LastName = NewWriterLastNameBox.Text;
            newWriter.BirthdayAsDateTime = NewWriterBirthday.SelectedDate;
            
            await Vm.SaveNewWriterAsync(newWriter);

            Vm.SelectedWriter = newWriter;

            NewWriterGrid.Visibility = Visibility.Collapsed;

        }

        //Save new Publisher Move to VM?
        private async Task SavePublisherOnClick()
        {
            Publisher newPublisher = new Publisher();

            newPublisher.Name = NewPublisherBox.Text;

            await Vm.SaveNewPublisherAsync(newPublisher);

            Vm.SelectedPublisher = newPublisher;

            NewPublisherGrid.Visibility = Visibility.Collapsed;

        }
        

        //Edit visibility buttons 
        private  void AddNewGenreBtn_Click(object sender, RoutedEventArgs e)
        {
            NewGenreGrid.Visibility = Visibility.Visible;

        }

        private void AddNewWritherBtn_Click(object sender, RoutedEventArgs e)
        {
            NewWriterGrid.Visibility = Visibility.Visible;
        }

        private void AddNewPublisher_Click(object sender, RoutedEventArgs e)
        {
            NewPublisherGrid.Visibility = Visibility.Visible;
        }

        
        //Delete button for Writer, Genre and Publisher
        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
           var btn = sender as Button;

            if (btn != null)
            {
               var tag  = btn.Tag.ToString();

                switch(tag)
                {
                    case "1":
                        await Vm.DeleteGenreAsync();
                        break;

                    case "2":
                        await Vm.DeleteWriterAsync();
                        break;

                    case "3":
                        await Vm.DeletePublisherAsync();
                        break;

                    default: return;
                }
            }

            
          
        }
    }
}
