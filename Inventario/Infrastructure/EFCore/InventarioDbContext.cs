using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EFCore
{
    public class InventarioDbContext: DbContext
    {
        public InventarioDbContext(DbContextOptions<InventarioDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Vendor)
                .WithMany()
                .HasForeignKey(p => p.VendorId);
            //modelBuilder.Entity<Product>()
            //    .HasOne(a => a.Brand)
            //    .WithMany()
            //    .HasForeignKey(a => a.BrandId);

            //Vendor
            //modelBuilder.Entity<Vendor>()
            //    .HasOne(p => p.City)
            //    .WithMany()
            //    .HasForeignKey(p => p.CityId);

            //Price
            modelBuilder.Entity<Price>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);

            //Client
            //modelBuilder.Entity<Client>()
            //    .HasOne(p => p.City)
            //    .WithMany()
            //    .HasForeignKey(p => p.CityId);

            //StockMovement
            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Product)
                .WithMany()
                .HasForeignKey(sm => sm.ProductId);

        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=InventarioDB;Trusted_Connection=True;");
        }
    }
}
