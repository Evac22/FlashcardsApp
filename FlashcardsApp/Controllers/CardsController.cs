using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using FlashcardsApp.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlashcardsApp.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class CardsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<CreateCardViewModel> _validator;

    public CardsController(ApplicationDbContext context, IValidator<CreateCardViewModel> validator)
    {
        _context = context;
        _validator = validator;
    }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCardViewModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);

        if (validationResult.IsValid)
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


        foreach (var error in validationResult.Errors)
        {
            Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}");
        }

        
        model.Decks = new SelectList(_context.Decks, "Id", "Name", model.DeckId);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
            {
        var card = await _context.Cards.FindAsync(id);
        if(card == null)
                {
            return NotFound();
        }

        var model = new CreateCardViewModel
        {
            Question = card.Question,
            Answer = card.Answer,
            DeckId = card.DeckId,
            Decks = new SelectList(_context.Decks, "Id", "Name", card.DeckId)
        };

        return View(model);
                }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateCardViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
            }

        var validationResult = await _validator.ValidateAsync(model);

        if (validationResult.IsValid)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            card.Question = model.Question;
            card.Answer = model.Answer;
            card.DeckId = model.DeckId;
            card.Image = model.ImageFile != null ? await ConvertToBytes(model.ImageFile) : card.Image;

            try
            {
                _context.Update(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cards.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", "Decks", new { id = model.DeckId });
        }

        foreach (var error in validationResult.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        model.Decks = new SelectList(_context.Decks, "Id", "Name", model.DeckId);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card == null)
        {
            return NotFound();
        }

        return View(card);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card == null)
        {
            return NotFound();
        }

        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Decks", new { id = card.DeckId });
    }

    private async Task<byte[]> ConvertToBytes(IFormFile imageFile)
    {
        using (var memoryStream = new MemoryStream())
        {
            await imageFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
