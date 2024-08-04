using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using FlashcardsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;

namespace FlashcardsApp.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cards/Create
        public IActionResult Create()
        {
            var model = new CreateCardViewModel
            {
                Decks = new SelectList(_context.Decks, "Id", "Name")
            };
            return View(model);
        }

        // POST: Cards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;

                if (model.ImageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                }

                var card = new Card
                {
                    Question = model.Question,
                    Answer = model.Answer,
                    DeckId = model.DeckId,
                    Image = imageData
                };

                _context.Cards.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Decks = new SelectList(_context.Decks, "Id", "Name", model.DeckId);
            return View(model);
        }
    }
}
