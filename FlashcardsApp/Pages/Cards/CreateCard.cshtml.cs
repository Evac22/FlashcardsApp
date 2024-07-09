using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using FlashcardsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlashcardsApp.Pages.Cards
{
    public class CreateCardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CreateCardModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public CreateCardViewModel Card { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            byte[] imageData = null;

            if(Card.Image != null)
            {
                using (var memmoryStream = new MemoryStream())
                {
                    await Card.Image.CopyToAsync(memmoryStream);
                    imageData = memmoryStream.ToArray();
                }
            }

            var card = new Card
            {
                Question = Card.Question,
                Answer = Card.Answer,
                DeckId = Card.DeckId,
                Image = imageData
            };

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
