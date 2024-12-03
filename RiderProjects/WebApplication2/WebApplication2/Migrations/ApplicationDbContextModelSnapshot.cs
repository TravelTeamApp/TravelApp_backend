﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication2.Data;

#nullable disable

namespace WebApplication2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("WebApplication2.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PlaceId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("PlaceId");

                    b.HasIndex("UserID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("WebApplication2.Models.Favorite", b =>
                {
                    b.Property<int>("FavoriteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("FavoriteId"));

                    b.Property<int?>("PlaceId")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FavoriteId");

                    b.HasIndex("PlaceId");

                    b.HasIndex("UserID");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("WebApplication2.Models.Place", b =>
                {
                    b.Property<int>("PlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PlaceId"));

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("PlaceAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("PlaceName")
                        .HasColumnType("longtext");

                    b.Property<int?>("PlaceTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.HasKey("PlaceId");

                    b.HasIndex("PlaceTypeId");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("WebApplication2.Models.PlaceType", b =>
                {
                    b.Property<int>("PlaceTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PlaceTypeId"));

                    b.Property<string>("PlaceTypeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("PlaceTypeId");

                    b.ToTable("PlaceTypes");
                });

            modelBuilder.Entity("WebApplication2.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<int?>("Score")
                        .HasColumnType("int");

                    b.Property<string>("TCKimlik")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApplication2.Models.UserPlaceType", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("PlaceTypeId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "PlaceTypeId");

                    b.HasIndex("PlaceTypeId");

                    b.ToTable("UserPlaceTypes");
                });

            modelBuilder.Entity("WebApplication2.Models.VisitedPlace", b =>
                {
                    b.Property<int>("VisitedPlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("VisitedPlaceId"));

                    b.Property<int?>("PlaceId")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("VisitedPlaceId");

                    b.HasIndex("PlaceId");

                    b.HasIndex("UserID");

                    b.ToTable("VisitedPlaces");
                });

            modelBuilder.Entity("WebApplication2.Models.Comment", b =>
                {
                    b.HasOne("WebApplication2.Models.Place", "Place")
                        .WithMany("Comments")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication2.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication2.Models.Favorite", b =>
                {
                    b.HasOne("WebApplication2.Models.Place", "Place")
                        .WithMany("Favorites")
                        .HasForeignKey("PlaceId");

                    b.HasOne("WebApplication2.Models.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserID");

                    b.Navigation("Place");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication2.Models.Place", b =>
                {
                    b.HasOne("WebApplication2.Models.PlaceType", "PlaceType")
                        .WithMany()
                        .HasForeignKey("PlaceTypeId");

                    b.Navigation("PlaceType");
                });

            modelBuilder.Entity("WebApplication2.Models.UserPlaceType", b =>
                {
                    b.HasOne("WebApplication2.Models.PlaceType", "PlaceType")
                        .WithMany("UserPlaceTypes")
                        .HasForeignKey("PlaceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication2.Models.User", "User")
                        .WithMany("UserPlaceTypes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlaceType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication2.Models.VisitedPlace", b =>
                {
                    b.HasOne("WebApplication2.Models.Place", "Place")
                        .WithMany("VisitedPlaces")
                        .HasForeignKey("PlaceId");

                    b.HasOne("WebApplication2.Models.User", "User")
                        .WithMany("VisitedPlaces")
                        .HasForeignKey("UserID");

                    b.Navigation("Place");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication2.Models.Place", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Favorites");

                    b.Navigation("VisitedPlaces");
                });

            modelBuilder.Entity("WebApplication2.Models.PlaceType", b =>
                {
                    b.Navigation("UserPlaceTypes");
                });

            modelBuilder.Entity("WebApplication2.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Favorites");

                    b.Navigation("UserPlaceTypes");

                    b.Navigation("VisitedPlaces");
                });
#pragma warning restore 612, 618
        }
    }
}
