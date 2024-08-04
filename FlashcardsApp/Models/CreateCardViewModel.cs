using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlashcardsApp.Models
{
    public class CreateCardViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int DeckId { get; set; }
        public SelectList Decks { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
