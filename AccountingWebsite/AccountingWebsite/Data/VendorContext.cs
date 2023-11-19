using AccountingWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingWebsite.Data
{
    public class VendorContext : DbContext
    {
        public DbSet<Vendor> Vendors { get; set; }

        public VendorContext(DbContextOptions<VendorContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<List<string>, string>(
                v => string.Join("||", v),
                v => v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList()
            );

            var comparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );

            modelBuilder.Entity<Vendor>()
                .Property(e => e.TransactionKeyphrases)
                .HasConversion(converter)
                .Metadata
                .SetValueComparer(comparer);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.ReceiptKeyphrases)
                .HasConversion(converter)
                .Metadata
                .SetValueComparer(comparer);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.ReceiptTotalKeyphrases)
                .HasConversion(converter)
                .Metadata
                .SetValueComparer(comparer);
        }
    }
}
