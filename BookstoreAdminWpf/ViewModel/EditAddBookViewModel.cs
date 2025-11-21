using BookstoreAdminWpf.Models;
using BookstoreAdminWpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookstoreAdminWpf.ViewModel
{
    public class EditAddBookViewModel : BaseViewModel
    {
        private ObservableCollection<Writer> _writers = new();
        public ObservableCollection<Writer> Writers
        {
            get => _writers;
            set
            {
                _writers = value;
                OnPropertyChanged();
            }
        }


        private Writer? _selectedWriter;
        public Writer? SelectedWriter
        {
            get => _selectedWriter;
            set
            {
                _selectedWriter = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Genre> _genres = new();
        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }


        private Genre? _selectedGenre;
        public Genre? SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre= value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Publisher> _publishers = new();
        public ObservableCollection<Publisher> Publishers
        {
            get => _publishers;
            set
            {
                _publishers = value;
                OnPropertyChanged();
            }
        }

        private Publisher? _selectedPublisher;
        public Publisher? SelectedPublisher
        {
            get => _selectedPublisher; 
            set
            {
                _selectedPublisher= value;
                OnPropertyChanged();
            }
        }

        public Book? Book { get; set; }
        public string TitleInput { get;  set; } = "";
        public string IsbnInput { get;  set; } = "";
        public decimal PriceInput { get;  set; }

        private GenreService _genreService;

        private WriterService _writerService;

        private PublisherService _publisherService;
        public EditAddBookViewModel(Book? book,
            WriterService writerService,
            GenreService genreService,
            PublisherService publisherService) 
        {
            Book = book;
            _writerService = writerService;
            _genreService = genreService;
            _publisherService = publisherService;
        }

        public async Task LoadAllWritersAsync()
        {
            var list = await _writerService.GetAllWritersAsync();
            Writers = new ObservableCollection<Writer>(list);

            if (Book != null && Book.WriterId != 0)
            {
                SelectedWriter = Writers
                    .FirstOrDefault(w => w.WriterId == Book.WriterId);
            }
        }

        public async Task LoadAllGenresAsync()
        {
            var list = await _genreService.GetAllGenresAsync();
            Genres = new ObservableCollection<Genre>(list);

            if(Book !=null && Book.GenreId != 0)
            {
                SelectedGenre = Genres.
                    FirstOrDefault(w => w.GenreId == Book.GenreId);
            }
        }

        public async Task LoadAllPublishersAsync()
        {
            var list = await _publisherService.GetAllPublishersAsync();

            Publishers = new ObservableCollection<Publisher>(list);

            if(Book != null && Book.PublisherId != 0)
            {
                SelectedPublisher = Publishers.
                    FirstOrDefault(p => p.PublisherId == Book.GenreId);
            }
        }

        public async Task SaveNewGenreAsync(Genre genre)
        {
            try
            {
                    await _genreService.AddNewGenreAsync(genre);
                    Genres.Add(genre);
                
            } catch (Exception ex)
            {
                MessageBox.Show("Misslyckades att spara ny genre till db.\n"+ex.Message);
            }
        }
        public async Task SaveNewWriterAsync(Writer writer)
        {
            try
            {
                await _writerService.AddNewWriterAsync(writer);
                Writers.Add(writer);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Misslyckades att spara ny writer till db.\n" + ex.Message);
            }
        }

        public async Task SaveNewPublisherAsync(Publisher publisher)
        {
            try
            {
                await _publisherService.AddNewPubslisherAsync(publisher);
                Publishers.Add(publisher);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Misslyckades att spara ny publisher till db.\n" + ex.Message);
            }
        }
    }
}
