using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.BookOperations.GetBooks
{
    public class GetBooksQuery
    {
        private readonly BookStoreDbContext _context;

        public GetBooksQuery(BookStoreDbContext context)
        {
            _context = context;
        }

        public List<BooksViewModel> Handle()
        {
            var bookList = _context.Books.OrderBy(x => x.Id).ToList<Book>();
            List<BooksViewModel> vm = new List<BooksViewModel>(bookList.Count);
            foreach (var book in bookList)
            {
                vm.Add(new BooksViewModel{
                    Title = book.Title,
                    GenreName = ((GenreEnum)book.GenreId).ToString(),
                    PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy"),
                    PageCount = book.PageCount
                });
            }
            return vm;
        }
    }

    public class BooksViewModel
    {
        public string? Title { get; set; }
        public string? GenreName { get; set; }
        public int PageCount { get; set; }
        public string? PublishDate { get; set; }
    }
}