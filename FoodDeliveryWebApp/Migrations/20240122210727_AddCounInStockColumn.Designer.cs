﻿// <auto-generated />
using FoodDeliveryWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FoodDeliveryWebApp.Migrations
{
    [DbContext(typeof(FoodDeliveryWebAppDbContext))]
    [Migration("20240122210727_AddCounInStockColumn")]
    partial class AddCounInStockColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("FoodDeliveryWebApp.Models.Cart", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("ItemsPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("ShippingPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("TaxPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("FoodDeliveryWebApp.Models.OrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CartId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CountInStock")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<long>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Qty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("FoodDeliveryWebApp.Models.OrderItem", b =>
                {
                    b.HasOne("FoodDeliveryWebApp.Models.Cart", null)
                        .WithMany()
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
