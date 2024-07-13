using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using FlashcardsApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlashcardsApp.Controllers
{
    public class DecksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DecksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDeckViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var deck = new Deck
            {
                Name = model.Name,
                Description = model.Description
            };

            _context.Decks.Add(deck);
            await _context.SaveChangesAsync();

            // Вывод отладочной информации
            System.Diagnostics.Debug.WriteLine("Deck created successfully!");

            return RedirectToAction("Index", "Home");
        }


    }
}
