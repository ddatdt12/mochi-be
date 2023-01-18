﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MochiApi.Models;

#nullable disable

namespace MochiApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221229100256_TransactionModel")]
    partial class TransactionModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MochiApi.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("WalletId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("MochiApi.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("WalletId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MochiApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "test@gmail.com",
                            Password = "123123123"
                        },
                        new
                        {
                            Id = 2,
                            Email = "test2@gmail.com",
                            Password = "123123123"
                        },
                        new
                        {
                            Id = 3,
                            Email = "test3@gmail.com",
                            Password = "123123123"
                        },
                        new
                        {
                            Id = 4,
                            Email = "test4@gmail.com",
                            Password = "123123123"
                        });
                });

            modelBuilder.Entity("MochiApi.Models.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Balance")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Wallet");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Balance = 100000,
                            IsDefault = true,
                            Name = "Ví",
                            Type = 0
                        },
                        new
                        {
                            Id = 2,
                            Balance = 200000,
                            IsDefault = true,
                            Name = "Ví",
                            Type = 0
                        },
                        new
                        {
                            Id = 3,
                            Balance = 300000,
                            IsDefault = true,
                            Name = "Ví",
                            Type = 0
                        },
                        new
                        {
                            Id = 4,
                            Balance = 400000,
                            IsDefault = true,
                            Name = "Ví",
                            Type = 0
                        });
                });

            modelBuilder.Entity("MochiApi.Models.WalletMember", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId", "WalletId");

                    b.HasIndex("WalletId");

                    b.ToTable("WalletMember");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            WalletId = 1,
                            JoinAt = new DateTime(2022, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = 0
                        },
                        new
                        {
                            UserId = 2,
                            WalletId = 2,
                            JoinAt = new DateTime(2022, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = 0
                        },
                        new
                        {
                            UserId = 3,
                            WalletId = 3,
                            JoinAt = new DateTime(2022, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = 0
                        },
                        new
                        {
                            UserId = 4,
                            WalletId = 4,
                            JoinAt = new DateTime(2022, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = 0
                        });
                });

            modelBuilder.Entity("MochiApi.Models.Category", b =>
                {
                    b.HasOne("MochiApi.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MochiApi.Models.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("MochiApi.Models.Transaction", b =>
                {
                    b.HasOne("MochiApi.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MochiApi.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MochiApi.Models.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Creator");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("MochiApi.Models.WalletMember", b =>
                {
                    b.HasOne("MochiApi.Models.Wallet", "Wallet")
                        .WithMany("WalletMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MochiApi.Models.User", "User")
                        .WithMany("WalletMembers")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("MochiApi.Models.User", b =>
                {
                    b.Navigation("WalletMembers");
                });

            modelBuilder.Entity("MochiApi.Models.Wallet", b =>
                {
                    b.Navigation("WalletMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
