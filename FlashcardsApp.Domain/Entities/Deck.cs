

namespace FlashcardsApp.Domain.Entities;

public class Deck
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Card> Cards { get; set;}
}
