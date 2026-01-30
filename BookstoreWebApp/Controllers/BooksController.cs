using BookstoreProjectData.Entities;
using BookstoreProjectData;
using BookstoreWebApp.Models.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookstoreWebApp.Models.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace BookstoreWebApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookstoreContext context;
        public BooksController(BookstoreContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await context.Books.Select(b => new BooksIndexViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                CoverImageUrl = b.CoverImageUrl,
                AuthorId = b.AuthorId,
                PromotionId = b.PromotionId

            }).ToListAsync();

            return View(books);
           
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var authors = await context.Authors.OrderBy(a => a.Id).ToListAsync();
            ViewBag.Authors = new SelectList(authors, "Id", "FullName");

            var genres = await context.Genres.OrderBy(a => a.Id).ToListAsync();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");

            var promotions = await context.Promotions.OrderBy(a => a.Id).ToListAsync();
            ViewBag.Promotions = new SelectList(promotions, "Id", "Percent");

            return View(new BooksCreateViewModel()); ; //or authors?
        }

        [HttpPost]
        public async Task<IActionResult> Create(BooksCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Price = model.Price,
                CoverImageUrl = model.CoverImageUrl,
                Synopsis = model.Synopsis,
                AuthorId = model.AuthorId,
                GenreId = model.GenreId,
                PromotionId = model.PromotionId,
                Publishers_Books = model.Publishers_Books
            };

            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BooksDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var book = await context.Books.FindAsync(model.Id);
            if (book == null) { return NotFound(); }

            context.Books.Remove(book);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null) { return NotFound(); }

            var model = new BooksEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Price = book.Price,
                CoverImageUrl = book.CoverImageUrl,
                Synopsis = book.Synopsis,
                AuthorId = book.AuthorId,
                GenreId = book.GenreId,
                PromotionId = book.PromotionId,
                Publishers_Books = book.Publishers_Books
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BooksEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var book = await context.Books.FindAsync(model.Id);
            if (book == null) { return NotFound(); }

            book.Title = model.Title;
            book.Price = model.Price;
            book.CoverImageUrl = model.CoverImageUrl;
            book.Synopsis = model.Synopsis;
            book.AuthorId = book.AuthorId;
            book.GenreId = book.GenreId;
            book.PromotionId = book.PromotionId;
            book.Publishers_Books = book.Publishers_Books;

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await context.Books.Where(book=>book.Id == id).Select(book=>new BooksDetailsViewModel
            {
                Id = book.Id,
                Synopsis = book.Synopsis,
                GenreId = book.GenreId,
                Orders_Books = book.Orders_Books,
                Publishers_Books = book.Publishers_Books,
                Reviews = book.Reviews
            }).FirstOrDefaultAsync();

            if (book == null) { return NotFound(); };
            return View(book);
        }
    }
}
