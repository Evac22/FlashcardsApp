using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using FlashcardsApp.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    private async Task<byte[]> ConvertToBytes(IFormFile imageFile)
    {
        using (var memoryStream = new MemoryStream())
        {
            await imageFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
