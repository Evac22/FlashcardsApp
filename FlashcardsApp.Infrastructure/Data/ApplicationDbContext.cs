using Microsoft.EntityFrameworkCore;
using FlashcardsApp.Domain.Entities;

namespace FlashcardsApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Deck)
                      .WithMany(d => d.Cards)
                      .HasForeignKey(e => e.DeckId);
            });

            modelBuilder.Entity<Deck>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}
