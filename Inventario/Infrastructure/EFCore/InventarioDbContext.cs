using Data;
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
        public DbSet<ProductEntryLine> ProductEntryLines { get; set; }
        public DbSet<ProductEntry> ProductEntries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Category
            modelBuilder.Entity<Category>()
                 .HasIndex(c => c.Name).IsUnique();
            //Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Vendor)
                .WithMany()
                .HasForeignKey(p => p.VendorId);
            modelBuilder.Entity<Product>()
                 .HasIndex(p => p.Code).IsUnique();
            //modelBuilder.Entity<Product>()
            //    .HasOne(a => a.Brand)
            //    .WithMany()
            //    .HasForeignKey(a => a.BrandId);

            //Vendor
            modelBuilder.Entity<Vendor>()
                .HasIndex(v => v.CUIL).IsUnique();
            modelBuilder.Entity<Vendor>()
                .HasIndex(v => v.Mail).IsUnique();
            //    .HasOne(p => p.City)
            //    .WithMany()
            //    .HasForeignKey(p => p.CityId);

            //Price
            modelBuilder.Entity<Price>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);

            //Client
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Dni).IsUnique();
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Mail).IsUnique();
            //.hasone(p => p.city)
            //.withmany()
            //.hasforeignkey(p => p.cityid);

            //ProductEntryLine
            modelBuilder.Entity<ProductEntryLine>()
                .HasOne(pel => pel.Product)
                .WithMany()
                .HasForeignKey(pel => pel.ProductId);
            modelBuilder.Entity<ProductEntryLine>()
                .HasOne(pel => pel.ProductEntry)
                .WithMany(pe => pe.ProductEntryLines)
                .HasForeignKey(pel => pel.ProductEntryId);

            //ProductEntry
            modelBuilder.Entity<ProductEntry>()
                .HasMany(pe => pe.ProductEntryLines)
                .WithOne(pel => pel.ProductEntry)
                .HasForeignKey(pel => pel.ProductEntryId);

            //User
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Username).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Dni).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Mail).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Phone).IsUnique();

            //Purchase
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);
        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=InventarioDB;Trusted_Connection=True;");
        }
    }
}
