using WebApi.DBOperations;
using WebApi.Entities;

namespace TestSetup
{
    public static class Books
    {
        public static void AddBooks(this BookStoreDbContext context)
        {
            context.Books.AddRange(
                new Book
                {
                    Title = "Lean Startup",                                    
                    PageCount = 200,
                    PublishDate = new DateTime(2011, 06, 12),
                    GenreId = 1, // Personal Growth
                    AuthorId = 1 // Eric Ries
                },
                new Book
                {
                    Title = "Herland",                                    
                    PageCount = 250,
                    PublishDate = new DateTime(1915, 05, 23),
                    GenreId = 2, // Science Fiction
                    AuthorId = 2 // Charlotte Perkins Gilman
                },
                new Book
                {
                    Title = "Dune",                                    
                    PageCount = 540,
                    PublishDate = new DateTime(1965, 08, 21),
                    GenreId = 2, // Science Fiction
                    AuthorId = 3 // Frank Herbert
                }
            );
        }
    }
}