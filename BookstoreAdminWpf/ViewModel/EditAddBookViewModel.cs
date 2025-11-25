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


        public async Task DeleteWriterAsync()
        {
            try
            {
                var writer = SelectedWriter;
                if (writer == null)
                {
                    MessageBox.Show("Välj en författare att ta bort");
                    return;
                }

                var mbrResult = MessageBox.Show($"Är du säker på att du vill ta bort Författaren, {writer.FullName}?", "Radera författare", MessageBoxButton.YesNo);

                if (mbrResult == MessageBoxResult.Yes)
                {
                    bool deleted = await _writerService.DeleteWriterAsync(writer.WriterId);
                    if (deleted)
                    {
                        Writers.Remove(writer);
                        MessageBox.Show($"Författaren {writer.FullName} raderad");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel vid bortagning av författaren\n" + ex.Message);
            
            }
        }

        public async Task DeleteGenreAsync()
        {
            try
            {
                var genre = SelectedGenre;
                if (genre == null)
                {
                    MessageBox.Show("Välj en genre att ta bort");
                    return;
                }

                var mbrResult = MessageBox.Show($"Är du säker på att du vill ta bort Genre, {genre.Name}?", "Radera Genre", MessageBoxButton.YesNo);

                if (mbrResult == MessageBoxResult.Yes)
                {
                    bool deleted = await _genreService.DeleteGenreAsync(genre.GenreId);
                    if (deleted)
                    {
                        Genres.Remove(genre);
                        MessageBox.Show($"Genren {genre.Name} raderad");
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel vid bortagning av Genren\n" + ex.Message);

            }
        }

        public async Task DeletePublisherAsync()
        {
            try
            {
                var publisher = SelectedPublisher;
                if (publisher == null)
                {
                    MessageBox.Show("Välj en publisher att ta bort");
                    return;
                }

                var mbrResult = MessageBox.Show($"Är du säker på att du vill ta bort Publisher, {publisher.Name}?", "Radera Publisher", MessageBoxButton.YesNo);

                if (mbrResult == MessageBoxResult.Yes)
                {
                    bool deleted = await _publisherService.DeletePublisherAsync(publisher.PublisherId);
                    if (deleted)
                    {
                        Publishers.Remove(publisher);
                        MessageBox.Show($"Publishern {publisher.Name} raderad");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel vid bortagning av Publisher\n" + ex.Message);

            }
        }
    }
}
