using AccountingWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingWebsite.Data
{
	public class TransactionDataContext : DbContext
	{
		public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<TransactionEntry> TransactionEntries { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<TransactionItemModifier> TransactionItemModifiers { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<TransactionItemMapping> TransactionItemMappings { get; set; }

        public TransactionDataContext(DbContextOptions<TransactionDataContext> options) : base(options)
		{
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // List Converters
            var converter = new ValueConverter<List<string>, string>(
                v => string.Join("||", v),
                v => v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList()
            );

            var comparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );

            // Measurements
            modelBuilder.Entity<Measurement>()
                .HasOne(m => m.InnerMeasurement)
                .WithMany()
                .HasForeignKey(m => m.InnerMeasurementID);

            // Transaction Item Mapping
            modelBuilder.Entity<TransactionItemMapping>()
                .Property(e => e.Keyphrases)
                .HasConversion(converter)
                .Metadata
                .SetValueComparer(comparer);
        }
    }
}
