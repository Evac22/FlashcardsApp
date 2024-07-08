

namespace FlashcardsApp.Domain.Entities;

public class Card
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int DeckId { get; set; }
    public Deck Deck { get; set; }   
    public byte[] Image { get; set; }
}
