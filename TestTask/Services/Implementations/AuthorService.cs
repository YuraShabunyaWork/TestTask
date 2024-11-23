using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext context;

        public AuthorService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Author> GetAuthor()
        {
            var length = await context.Books.MaxAsync(a => a.Title.Length);

            var result = await context.Authors
                .Where(author => context.Books
                    .Any(book => book.Title.Length == length))
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Author>> GetAuthors()
        {
            var result = await context.Authors
                .Where(author => context.Books
                    .Count(book => book.PublishDate.Year > 2015 && book.AuthorId == author.Id) % 2 == 0)
                .Where(author => context.Books
                    .Count(book => book.PublishDate.Year > 2015 && book.AuthorId == author.Id) > 0)
                .ToListAsync();

            return result;
        }
        
    }
}
