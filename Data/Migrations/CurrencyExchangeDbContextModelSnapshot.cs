﻿// <auto-generated />
using System;
using CurrencyExchange.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CurrencyExchange.Data.Migrations
{
    [DbContext(typeof(CurrencyExchangeDbContext))]
    partial class CurrencyExchangeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("AmountIn")
                        .HasColumnType("int");

                    b.Property<int>("AmountOut")
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExecutedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FromCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CurrencyExchangeHistoryId");

                    b.ToTable("CurrencyExchangeHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
