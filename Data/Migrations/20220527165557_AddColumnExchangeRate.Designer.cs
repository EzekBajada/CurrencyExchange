﻿// <auto-generated />
using System;
using CurrencyExchange.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CurrencyExchange.Data.Migrations
{
    [DbContext(typeof(CurrencyExchangeDbContext))]
    [Migration("20220527165557_AddColumnExchangeRate")]
    partial class AddColumnExchangeRate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CurrencyExchange.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"), 1L, 1);

                    b.Property<string>("BaseCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CurrencyExchange.Models.CurrencyExchangeHistory", b =>
                {
                    b.Property<int>("CurrencyExchangeHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CurrencyExchangeHistoryId"), 1L, 1);

                    b.Property<double>("AmountIn")
                        .HasColumnType("float");

                    b.Property<double>("AmountOut")
                        .HasColumnType("float");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<double>("ExchangRate")
                        .HasColumnType("float");

                    b.Property<DateTime>("ExecutedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FromCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CurrencyExchangeHistoryId");

                    b.HasIndex("ClientId");

                    b.ToTable("CurrencyExchangeHistories");
                });

            modelBuilder.Entity("CurrencyExchange.Models.CurrencyExchangeHistory", b =>
                {
                    b.HasOne("CurrencyExchange.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });
#pragma warning restore 612, 618
        }
    }
}