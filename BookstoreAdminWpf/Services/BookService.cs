using BookstoreAdminWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookstoreAdminWpf.Services
{
    public class BookService
    {
        private readonly AppDbContext _db;

        public BookService(AppDbContext db)
        {
            _db = db;   
        }

        //Get all books with Writer and Genre (FK's)
        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = await _db.Books
                        .Include(w => w.Writer)
                        .Include(g=>g.Genre)
                        .Include(p=>p.Publisher)
                        .ToListAsync();
            return books;

        }

        //Get book by ID (isbn13)
        public async Task<Book> GetBookByIDAsync(string isbn13)
        {


            var book = await _db.Books
                        .Include(w => w.Writer)
                        .Include(g=> g.Genre)
                        .FirstOrDefaultAsync(x=> x.Isbn13 == isbn13);

           

           return book;
        }

        //Create book 
        public async Task CreateBookAsync(Book book)
        {
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();

        }

        //Update book (tex om pris ändras) 
        public async Task UpdateBookAsync(Book book, string isbn)
        {
            if (book.Isbn13 != isbn)
            {
                return ;
            }

            _db.Books.Update(book);
            await _db.SaveChangesAsync();
        }

        //Delete Book
        public async Task<bool> DeleteBookAsync(string isbn13)
        {
            var book = await _db.Books.FindAsync(isbn13);
            if(book == null)
            {
                return false;
            }
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
