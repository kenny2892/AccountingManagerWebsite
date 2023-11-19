﻿// <auto-generated />
using System;
using AccountingWebsite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountingWebsite.Migrations
{
    [DbContext(typeof(TransactionDataContext))]
    [Migration("20231028020337_IsCredit")]
    partial class IsCredit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("AccountingWebsite.Models.Transaction", b =>
                {
                    b.Property<string>("TransactionID")
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<double>("Balance")
                        .HasColumnType("REAL");

                    b.Property<string>("Bank")
                        .HasColumnType("TEXT");

                    b.Property<string>("CategoryOne")
                        .HasColumnType("TEXT");

                    b.Property<string>("CategoryTwo")
                        .HasColumnType("TEXT");

                    b.Property<string>("CheckNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtendedDescription")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCredit")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPurchase")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Memo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostingDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PurchasingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiptRelativeFilePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("VendorName")
                        .HasColumnType("TEXT");

                    b.HasKey("TransactionID");

                    b.ToTable("Transatcions");
                });
#pragma warning restore 612, 618
        }
    }
}
