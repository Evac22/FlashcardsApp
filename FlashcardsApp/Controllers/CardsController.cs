using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using FlashcardsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class CardsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CardsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCardViewModel model)
    {
        if (ModelState.IsValid)
        {
            var card = new Card
            {
                Question = model.Question,
                Answer = model.Answer,
                DeckId = model.DeckId
            };

            if (model.Image != null && model.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.Image.CopyToAsync(ms);
                    card.Image = ms.ToArray();
                }
            }

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }
}
