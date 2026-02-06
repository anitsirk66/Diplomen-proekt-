using BookstoreProjectData.Entities;
using BookstoreProjectData;
using BookstoreWebApp.Models.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookstoreWebApp.Models.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BookstoreWebApp.Models.Reviews;

namespace BookstoreWebApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookstoreContext context;
        public BooksController(BookstoreContext _context)
        {
            context = _context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await context.Books.Select(b => new BooksIndexViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                CoverImageUrl = b.CoverImageUrl,
                AuthorName = b.Author.FullName,
                PromotionPercent = b.Promotion.Percent

            }).ToListAsync();

            return View(books);
           
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var authors = await context.Authors.OrderBy(a => a.FullName).ToListAsync();
            ViewBag.Authors = new SelectList(authors, "Id", "FullName");

            var genres = await context.Genres.OrderBy(a => a.Name).ToListAsync();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");

            var promotions = await context.Promotions.OrderBy(a => a.Percent).ToListAsync();
            ViewBag.Promotions = new SelectList(promotions, "Id", "Percent");

            return View(new BooksCreateViewModel()); ; 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(BooksCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var authors = await context.Authors.OrderBy(a => a.FullName).ToListAsync();
                ViewBag.Authors = new SelectList(authors, "Id", "FullName");

                var genres = await context.Genres.OrderBy(a => a.Name).ToListAsync();
                ViewBag.Genres = new SelectList(genres, "Id", "Name");

                var promotions = await context.Promotions.OrderBy(a => a.Percent).ToListAsync();
                ViewBag.Promotions = new SelectList(promotions, "Id", "Percent");

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
                PromotionId = model.PromotionId
            };

            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await context.Books
                .Where(b => b.Id == id)
                .Select(b => new BooksDeleteViewModel
                {
                    Id = b.Id
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
                PromotionId = book.PromotionId
            };

            var authors = await context.Authors.OrderBy(a => a.FullName).ToListAsync();
            ViewBag.Authors = new SelectList(authors, "Id", "FullName");

            var genres = await context.Genres.OrderBy(a => a.Name).ToListAsync();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");

            var promotions = await context.Promotions.OrderBy(a => a.Percent).ToListAsync();
            ViewBag.Promotions = new SelectList(promotions, "Id", "Percent");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(BooksEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var authors = await context.Authors.OrderBy(a => a.FullName).ToListAsync();
                ViewBag.Authors = new SelectList(authors, "Id", "FullName");

                var genres = await context.Genres.OrderBy(a => a.Name).ToListAsync();
                ViewBag.Genres = new SelectList(genres, "Id", "Name");

                var promotions = await context.Promotions.OrderBy(a => a.Percent).ToListAsync();
                ViewBag.Promotions = new SelectList(promotions, "Id", "Percent");

                return View(model);
            }

            var book = await context.Books.FindAsync(model.Id);
            if (book == null) { return NotFound(); }

            book.Title = model.Title;
            book.Price = model.Price;
            book.CoverImageUrl = model.CoverImageUrl;
            book.Synopsis = model.Synopsis;
            book.AuthorId = model.AuthorId;
            book.GenreId = model.GenreId;
            book.PromotionId = model.PromotionId;

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await context.Books.Where(book => book.Id == id).Select(book => new BooksDetailsViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Synopsis = book.Synopsis,
                GenreName = book.Genre.Name,
                Reviews = book.Reviews.Select(r=> new ReviewIndexViewModel
                {
                    Id = r.Id, Username = r.User.UserName, Text = r.Text, DateAndTime = r.DateAndTime, BookId = book.Id
                }).ToList()
            }).FirstOrDefaultAsync();

            if (book == null) { return NotFound(); };
            return View(book);
        }
    }
}
