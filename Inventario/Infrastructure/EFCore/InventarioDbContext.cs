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
        public DbSet<ProductEntryLine> ProductEntryLines { get; set; }
        public DbSet<ProductEntry> ProductEntries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<MiscellaneousExpenses> MiscellaneousExpenses { get; set; }
        public DbSet<Cash> CashPayments { get; set; }
        public DbSet<CreditCard> CreditCardPayments { get; set; }
        public DbSet<DebitCard> DebitCardPayments { get; set; }
        public DbSet<ChequesPayment> ChequePayments { get; set; }
        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<OwnFees> OwnFeesPayments { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeRule> FeeRules { get; set; }
        public DbSet<Commission> Commissions { get; set; }

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
            modelBuilder.Entity<Product>()
                .Property(p => p.MinimumStock)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Product>()
                .Property(p => p.Stock)
                .HasColumnType("decimal(18,4)");

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
            modelBuilder.Entity<Price>()
                .Property(p => p.Value)
                .HasColumnType("decimal(18,4)");

            //Client
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Dni).IsUnique();
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Mail).IsUnique();

            //ProductEntryLine
            modelBuilder.Entity<ProductEntryLine>()
                .HasOne(pel => pel.Product)
                .WithMany()
                .HasForeignKey(pel => pel.ProductId);
            modelBuilder.Entity<ProductEntryLine>()
                .HasOne(pel => pel.ProductEntry)
                .WithMany(pe => pe.ProductEntryLines)
                .HasForeignKey(pel => pel.ProductEntryId);
            modelBuilder.Entity<ProductEntryLine>()
                .Property(pel => pel.Quantity)
                .HasColumnType("decimal(18,4)");

            //ProductEntry
            modelBuilder.Entity<ProductEntry>()
                .HasMany(pe => pe.ProductEntryLines)
                .WithOne(pel => pel.ProductEntry)
                .HasForeignKey(pel => pel.ProductEntryId);
            modelBuilder.Entity<ProductEntry>()
                .Property(pe => pe.Cost)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<ProductEntry>()
                .HasOne(pe => pe.Vendor)
                .WithMany()
                .HasForeignKey(pel => pel.VendorId);

            //User
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Username).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Dni).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Mail).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Phone).IsUnique();

            //Sale
            modelBuilder.Entity<Sale>()
                .HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId);
            modelBuilder.Entity<Sale>()
                .HasOne(p => p.Payment)
                .WithMany()
                .HasForeignKey(p => p.PaymentId);

            //Detail
            modelBuilder.Entity<Detail>()
                .HasOne(d => d.Sale)
                .WithMany(s => s.Details)
                .HasForeignKey(d => d.SaleId);
            modelBuilder.Entity<Detail>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<Detail>()
                .HasOne(d => d.FeeRule)
                .WithMany()
                .HasForeignKey(d => d.FeeRuleId);
            modelBuilder.Entity<Detail>()
                .Property(d => d.UnitPrice)
                .HasColumnType("decimal(18,4)");

            //Fee
            modelBuilder.Entity<Payment>()
                .Property(o => o.Amount)
                .HasColumnType("decimal(18,4)");

            //Fee
            modelBuilder.Entity<Fee>()
                .HasOne(f => f.OwnFees)
                .WithMany(x => x.FeeList)
                .HasForeignKey(f => f.OwnFeesId);
            modelBuilder.Entity<Fee>()
                .Property(o => o.Value)
                .HasColumnType("decimal(18,4)");

            //FeeRule
            modelBuilder.Entity<FeeRule>()
                .HasOne(fr => fr.Product)
                .WithMany()
                .HasForeignKey(fr => fr.ProductId);
            modelBuilder.Entity<FeeRule>()
                .Property(fr => fr.Percentage)
                .HasColumnType("decimal(18,4)");

            //Cheque
            modelBuilder.Entity<Cheque>()
                .HasOne(ch => ch.ChequesPayment)
                .WithMany(chP => chP.ListOfCheques)
                .HasForeignKey(ch => ch.ChequesPaymentId);
            modelBuilder.Entity<Cheque>()
                .HasIndex(ch => ch.Nro).IsUnique();
            modelBuilder.Entity<Cheque>()
                .Property(ch => ch.Value)
                .HasColumnType("decimal(18,4)");

            //Cash
            modelBuilder.Entity<Cash>()
                .Property(c => c.Discount)
                .HasColumnType("decimal(18,4)");

            //CreditCard
            modelBuilder.Entity<CreditCard>()
                .Property(c => c.Discount)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<CreditCard>()
                .Property(c => c.Surcharge)
                .HasColumnType("decimal(18,4)");

            //DebitCard
            modelBuilder.Entity<DebitCard>()
                .Property(c => c.Discount)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<DebitCard>()
                .Property(c => c.Surcharge)
                .HasColumnType("decimal(18,4)");

            //Commission
            modelBuilder.Entity<Commission>()
                .HasOne(c => c.Vendor)
                .WithMany()
                .HasForeignKey(c => c.VendorId);
            modelBuilder.Entity<Commission>()
                .HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId);
            modelBuilder.Entity<Commission>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Commission>()
                .Property(c => c.Value)
                .HasColumnType("decimal(18,4)");

            //MiscellaneousExpenses
            modelBuilder.Entity<MiscellaneousExpenses>()
                .Property(me => me.Value)
                .HasColumnType("decimal(18,4)");
        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=InventarioDB;Trusted_Connection=True;");
        }
    }
}
