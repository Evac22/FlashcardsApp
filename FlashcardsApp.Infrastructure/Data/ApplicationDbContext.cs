using Microsoft.EntityFrameworkCore;
using FlashcardsApp.Domain.Entities;

namespace FlashcardsApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Deck> Decks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {

        base.OnModelCreating(modelBuilder);


        // Configure the User entity

        modelBuilder.Entity<User>(entity =>

        {

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email).IsRequired();

        });


        // Configure the Card entity

        modelBuilder.Entity<Card>(entity =>

        {

            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Deck)

                  .WithMany(d => d.Cards)

                  .HasForeignKey(e => e.DeckId);

        });


        // Configure the Deck entity

        modelBuilder.Entity<Deck>(entity =>

        {

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired();

        });

    }
}
