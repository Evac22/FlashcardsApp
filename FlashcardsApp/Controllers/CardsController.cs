using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlashcardsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlashcardsApp.Controllers
{
    [Authorize]
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IActionResult Create(int deckId)
        //{
        //    var model = new CreateCardViewModel
        //    {
        //        DeckId = deckId,
        //        Decks = new SelectList(_context.Decks, "Id", "Name")
        //    };
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateCardViewModel
            {
                Decks = new SelectList(_context.Decks, "Id", "Name")
            };

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(CreateCardViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Вывод ошибок валидации в консоль
        //        foreach (var state in ModelState)
        //        {
        //            foreach (var error in state.Value.Errors)
        //            {
        //                Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
        //            }
        //        }

        //        model.Decks = new SelectList(_context.Decks, "Id", "Name", model.DeckId);
        //        return View(model);
        //    }

        //    var card = new Card
        //    {
        //        Question = model.Question,
        //        Answer = model.Answer,
        //        DeckId = model.DeckId,
        //        Image = model.ImageFile != null ? await ConvertToBytes(model.ImageFile) : null
        //    };

        //    _context.Add(card);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Details", "Decks", new { id = model.DeckId });
        //}
        public async Task<IActionResult> Create(CreateCardViewModel model)
        {
            // Temporary bypass validation for debugging purposes
            if (true || ModelState.IsValid)
            {
                var card = new Card
                {
                    Question = model.Question,
                    Answer = model.Answer,
                    DeckId = model.DeckId,
                    Image = model.ImageFile != null ? await ConvertToBytes(model.ImageFile) : null
                };

                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Decks", new { id = model.DeckId });
            }

            model.Decks = new SelectList(_context.Decks, "Id", "Name", model.DeckId); // Ensure the Decks property is set if the model state is invalid
            return View(model);
        }

        private async Task<byte[]> ConvertToBytes(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}
