﻿// <auto-generated />
using AccountingWebsite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountingWebsite.Migrations.Statement
{
    [DbContext(typeof(StatementContext))]
    partial class StatementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("AccountingWebsite.Models.StatementMapping", b =>
                {
                    b.Property<int>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AmountHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("BalanceHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("Bank")
                        .HasColumnType("TEXT");

                    b.Property<string>("CategoryOneHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("CategoryTwoHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("CheckNumberHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("DescriptionHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtendedDescriptionHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsCheckHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsCreditHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsCreditPaymentHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsPurchaseHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("MemoHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostingDateHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("PurchasingDateHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReferenceNumberHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionIdHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("VendorNameHeader")
                        .HasColumnType("TEXT");

                    b.HasKey("Index");

                    b.ToTable("StatementMappings");
                });
#pragma warning restore 612, 618
        }
    }
}
