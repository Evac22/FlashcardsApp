using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FlashcardsApp.Models
{
    public class CreateCardViewModel
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public int DeckId { get; set; }

        public SelectList Decks { get; set; }

        public IFormFile ImageFile { get; set; }
    }

}
