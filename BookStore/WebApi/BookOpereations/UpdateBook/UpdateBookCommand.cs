using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.BookOperations.UpdateBook
{
    public class UpdateBookCommand
    {
        private readonly BookStoreDbContext _context;
        public int? Id { get; set; }
        public UpdateBookModel Model { get; set; }

        public UpdateBookCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            if (Model is null)
                throw new InvalidOperationException("The book data is empty");
            if (Id is null)
                throw new InvalidOperationException("Id is empty");
            var existingBook = _context.Books.SingleOrDefault(x => x.Id == Id);
            if(existingBook is null)
                throw new InvalidOperationException("The book is not exist");
            existingBook.GenreId = Model.GenreId != default ? Model.GenreId : existingBook.GenreId;
            existingBook.PageCount = Model.PageCount != default ? Model.PageCount : existingBook.PageCount;
            existingBook.PublishDate = Model.PublishDate != default ? Model.PublishDate : existingBook.PublishDate;
            existingBook.Title = Model.Title != default ? Model.Title : existingBook.Title;
            _context.SaveChanges();
        }

        public class UpdateBookModel
        {
            public string? Title { get; set; }
            public int GenreId { get; set; }
            public int PageCount { get; set; }
            public DateTime PublishDate { get; set; }
        }
    }
}