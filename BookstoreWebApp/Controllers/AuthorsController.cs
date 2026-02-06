using BookstoreProjectData;
using BookstoreProjectData.Entities;
using BookstoreWebApp.Models.Authors;
using BookstoreWebApp.Models.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookstoreWebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly BookstoreContext context;
        public AuthorsController(BookstoreContext _context)
        {
            context = _context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var authors = context.Authors.Select(b => new AuthorsIndexViewModel
            {
                Id = b.Id,
                FullName = b.FullName,
                Nationality = b.Nationality,
                Biography = b.Biography,
                ImageUrl = "/img/noPfp.jpg"
            }).ToList();

            return View(authors);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AuthorsCreateViewModel()); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(AuthorsCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var author = new Author
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Nationality = model.Nationality,
                Biography = model.Biography
            };

            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await context.Authors
                .Where(a => a.Id == id)
                .Select(b => new AuthorsDeleteViewModel
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
        public async Task<IActionResult> Delete(AuthorsDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var author = await context.Authors.FindAsync(model.Id);
            if (author == null) { return NotFound(); }

            context.Authors.Remove(author);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var author = await context.Authors.FindAsync(id);
            if (author == null) { return NotFound(); }

            var model = new AuthorsEditViewModel
            {
                Id = author.Id,
                FullName = author.FullName,
                Nationality = author.Nationality,
                Biography = author.Biography
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(AuthorsEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var author = await context.Authors.FindAsync(model.Id);
            if (author == null) { return NotFound(); }

            author.FullName = model.FullName;
            author.Nationality = model.Nationality;
            author.Biography = model.Biography;

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
