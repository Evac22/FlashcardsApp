using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlashcardsApp.Pages.Cards;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Card Card { get; set; }

    public IActionResult OnGet()
    {
        ViewData["DeckId"] = new SelectList(_context.Decks, "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ViewData["DeckId"] = new SelectList(_context.Decks, "Id", "Name", Card.DeckId);
            return Page();
        }

        _context.Cards.Add(Card);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
