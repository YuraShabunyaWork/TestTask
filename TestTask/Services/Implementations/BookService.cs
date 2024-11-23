using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext context;

        public BookService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Book> GetBook()
        {
            var result = await context.Books
                .OrderByDescending(book => book.Price * book.QuantityPublished)
                .FirstOrDefaultAsync();

            return result;
        }
        public async Task<List<Book>> GetBooks()
        {
            DateTime carolusRexReleaseDate = new DateTime(2012, 5, 25);
            var result = await context.Books.
                Where(book => book.Title.Contains("Red") && book.PublishDate > carolusRexReleaseDate)
                .ToListAsync();

            return result;
        }
    }
}
