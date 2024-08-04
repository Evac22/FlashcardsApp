using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Application.Interfaces;

namespace FlashcardsApp.Pages.Cards
{
    public class CreateModel : PageModel
    {
        private readonly ICardService _cardService;

        [BindProperty]
        public Card Card { get; set; }

        [BindProperty]
        public IFormFile ImageUpload { get; set; }

        public CreateModel(ICardService cardService)
        {
            _cardService = cardService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    Card.Image = memoryStream.ToArray();
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", ImageUpload.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fileStream);
                }
            }

            await _cardService.AddCardAsync(Card);

            return RedirectToPage("./Index");
        }
    }
}
