//using AspNetCore;
using BookstoreProjectData;
using BookstoreProjectData.Entities;
using BookstoreWebApp.Models.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace BookstoreWebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly BookstoreContext context;
        public EventsController(BookstoreContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var events = await context.Events.Select(o => new EventsIndexViewModel
            {
                Id = o.Id,
                Name = o.Name,
                DateAndTime = o.DateAndTime,
                AuthorId = o.AuthorId
            }).ToListAsync();
            
            return View(events);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var authors = await context.Authors.OrderBy(a=>a.Id).ToListAsync();
            ViewBag.Authors = new SelectList(authors, "Id", "FullName");

            return View(new EventsCreateViewModel()); //or authors?
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventsCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var eventt = new Event
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                DateAndTime = model.DateAndTime,
                AuthorId = model.AuthorId
            };

            await context.Events.AddAsync(eventt);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EventsDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventt = await context.Events.FindAsync(model.Id);
            if(eventt == null) { return NotFound(); }
            
            context.Events.Remove(eventt);
            await context.SaveChangesAsync();
            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var eventt = await context.Events.FindAsync(id);
            if (eventt == null) { return NotFound(); }

            var model = new EventsIndexViewModel
            {
                Id = eventt.Id,
                Name = eventt.Name,
                DateAndTime = eventt.DateAndTime,
                AuthorId = eventt.AuthorId
            };

            return View(model); 
        }
        [HttpPost]
        public async Task<IActionResult>Edit(EventsIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventt = await context.Events.FindAsync(model.Id);
            if (eventt == null) { return NotFound(); }

            eventt.Name = model.Name;
            eventt.DateAndTime = model.DateAndTime;
            eventt.AuthorId = model.AuthorId;

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

} 
