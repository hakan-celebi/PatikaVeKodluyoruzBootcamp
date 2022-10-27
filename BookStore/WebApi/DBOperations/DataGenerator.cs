using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DBOperations
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                {
                    return;
                }
                context.Genres.AddRange(
                    new Genre{
                        Name = "Personal Growth"
                    },
                    new Genre{
                        Name = "Science Fiction"
                    },
                    new Genre{
                        Name = "Novel"
                    }
                );
                context.Authors.AddRange(
                    new Author{
                        FirstName = "Eric",
                        LastName = "Ries",
                        DateOfBirth = new DateTime(1978, 09, 22)
                    },
                    new Author{
                        FirstName = "Charlotte",
                        MiddleName = "Perkins",
                        LastName = "Gilman",
                        DateOfBirth = new DateTime(1860, 07, 03)
                    },
                    new Author{
                        FirstName = "Frank",
                        LastName = "Herbert",
                        DateOfBirth = new DateTime(1920, 10, 08)
                    }
                );
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
                context.SaveChanges();
            }
        }
    }
}