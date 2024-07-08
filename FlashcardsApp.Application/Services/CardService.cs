
using FlashcardsApp.Application.Interfaces;
using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlashcardsApp.Application.Services
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _context;

        public CardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            return await _context.Cards.ToListAsync();
        }

        public async Task<Card> GetCardByIdAsync(int id)
        {
            return await _context.Cards.FindAsync(id);
        }

        public async Task AddCardAsync(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCardAsync(Card card)
        {
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCardAsync(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
            }
        }
    }
}
