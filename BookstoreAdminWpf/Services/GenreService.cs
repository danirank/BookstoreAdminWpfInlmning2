using BookstoreAdminWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAdminWpf.Services
{
    public class GenreService
    {
        private readonly AppDbContext _db;

        public GenreService(AppDbContext db)
        {
            _db = db;
        }
        //Get all Genres 
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            var result = await _db.Genres.ToListAsync();

            return result;
        }

        //Add Genre 

        public async Task AddNewGenreAsync(Genre genre)
        {
            await _db.Genres.AddAsync(genre);
            await _db.SaveChangesAsync();

        }

        //Delete Genre
        public async Task<bool> DeleteGenreAsync(int genreId)
        {
            var genre = await _db.Genres.FindAsync(genreId);

            if (genre is null)
            {
                return false;
            }

            _db.Genres.Remove(genre);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
