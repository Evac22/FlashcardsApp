namespace FlashcardsApp.Models
{
    public class CreateCardViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public IFormFile Image { get; set; }
        public int DeckId { get; set; }
    }
}
