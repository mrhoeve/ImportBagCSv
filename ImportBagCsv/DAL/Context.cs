using System;
using System.Collections.Generic;
using System.Text;
using ImportBagCsv.Models;
using Microsoft.EntityFrameworkCore;

namespace ImportBagCsv.DAL
{
    public class Context : DbContext
    {
        public DbSet<Nummer> Nummers { get; set; }
        public DbSet<Adres> Adressen { get; set; }
        public DbSet<Plaats> Plaatsen { get; set; }
        public DbSet<Gemeente> Gemeenten { get; set; }
        public DbSet<Provincie> Provincies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nummer>()
                .HasIndex(n => n.Huisnummer)
                .IncludeProperties(n => new { n.Huisletter, n.Huisnummertoevoeging});

            modelBuilder.Entity<Adres>()
                .HasIndex(a => a.Postcode)
                .IncludeProperties(a => a.Straat);

            modelBuilder.Entity<Gemeente>()
                .HasIndex(g => g.Naam);

            modelBuilder.Entity<Provincie>()
                .HasIndex(p => p.Naam);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Postcode;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
