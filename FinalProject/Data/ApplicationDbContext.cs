using System;
using System.Collections.Generic;
using System.Text;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<CardImage> CardImages { get; set; }
        public DbSet<CardPrice> CardPrices { get; set; }
        public DbSet<CardSet> CardSets { get; set; }
        public DbSet<DeckCardMapping> DeckCardMappings { get; set; }
        public DbSet<UserInventory> UserInventories { get; set; }
        public DbSet<InventoryCardMapping> InventoryCardMappings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DeckCardMapping>()
                .HasKey(c => new { c.DeckId, c.CardId });
            modelBuilder.Entity<InventoryCardMapping>()
                .HasKey(c => new { c.UserInventoryId, c.CardId });

        }
    }

}
