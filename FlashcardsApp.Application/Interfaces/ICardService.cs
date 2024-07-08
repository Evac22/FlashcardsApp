
using FlashcardsApp.Domain.Entities;

namespace FlashcardsApp.Application.Interfaces
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetAllCardsAsync();
        Task<Card> GetCardByIdAsync(int id);
        Task AddCardAsync(Card card);
        Task UpdateCardAsync(Card card);
        Task DeleteCardAsync(int id);
    }
}
