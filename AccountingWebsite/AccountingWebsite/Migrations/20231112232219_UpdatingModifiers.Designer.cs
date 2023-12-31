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
    [Migration("20231112232219_UpdatingModifiers")]
    partial class UpdatingModifiers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("AccountingWebsite.Models.Measurement", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int?>("InnerMeasurementID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCase")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsContainer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortName")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("InnerMeasurementID");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("AccountingWebsite.Models.Receipt", b =>
                {
                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionID")
                        .HasColumnType("TEXT");

                    b.HasKey("FileName");

                    b.ToTable("Receipts");
                });

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

                    b.Property<bool>("IsCheck")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCredit")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCreditPayment")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPurchase")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Memo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostingDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PurchasingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("VendorName")
                        .HasColumnType("TEXT");

                    b.HasKey("TransactionID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MeasurementID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionID")
                        .HasColumnType("TEXT");

                    b.Property<int>("TransactionItemID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("MeasurementID");

                    b.HasIndex("TransactionID");

                    b.HasIndex("TransactionItemID");

                    b.ToTable("TransactionEntries");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("TransactionItems");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionItemMapping", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Keyphrases")
                        .HasColumnType("TEXT");

                    b.Property<string>("VendorName")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ItemID");

                    b.ToTable("TransactionItemMappings");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionItemModifier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MeasurementID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModifierID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ParentModifierID")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("TEXT");

                    b.Property<int>("TransactionEntryID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("MeasurementID");

                    b.HasIndex("ModifierID");

                    b.HasIndex("ParentModifierID");

                    b.HasIndex("TransactionEntryID");

                    b.ToTable("TransactionItemModifiers");
                });

            modelBuilder.Entity("AccountingWebsite.Models.Measurement", b =>
                {
                    b.HasOne("AccountingWebsite.Models.Measurement", "InnerMeasurement")
                        .WithMany()
                        .HasForeignKey("InnerMeasurementID");

                    b.Navigation("InnerMeasurement");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionEntry", b =>
                {
                    b.HasOne("AccountingWebsite.Models.Measurement", "Measurement")
                        .WithMany()
                        .HasForeignKey("MeasurementID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountingWebsite.Models.Transaction", "Transaction")
                        .WithMany("TransactionEntries")
                        .HasForeignKey("TransactionID");

                    b.HasOne("AccountingWebsite.Models.TransactionItem", "Item")
                        .WithMany()
                        .HasForeignKey("TransactionItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Measurement");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionItemMapping", b =>
                {
                    b.HasOne("AccountingWebsite.Models.TransactionItem", "Item")
                        .WithMany()
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionItemModifier", b =>
                {
                    b.HasOne("AccountingWebsite.Models.Measurement", "Measurement")
                        .WithMany()
                        .HasForeignKey("MeasurementID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountingWebsite.Models.TransactionItem", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountingWebsite.Models.TransactionItemModifier", "ParentModifier")
                        .WithMany("Modifiers")
                        .HasForeignKey("ParentModifierID");

                    b.HasOne("AccountingWebsite.Models.TransactionEntry", "TransactionEntry")
                        .WithMany("Modifiers")
                        .HasForeignKey("TransactionEntryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Measurement");

                    b.Navigation("Modifier");

                    b.Navigation("ParentModifier");

                    b.Navigation("TransactionEntry");
                });

            modelBuilder.Entity("AccountingWebsite.Models.Transaction", b =>
                {
                    b.Navigation("TransactionEntries");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionEntry", b =>
                {
                    b.Navigation("Modifiers");
                });

            modelBuilder.Entity("AccountingWebsite.Models.TransactionItemModifier", b =>
                {
                    b.Navigation("Modifiers");
                });
#pragma warning restore 612, 618
        }
    }
}
