﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using pastemyst.DbContexts;

#nullable disable

namespace pastemyst.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230302143343_ImageContentType")]
    partial class ImageContentType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("pastemyst.Models.Image", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("bytes");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_type");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.HasKey("Id")
                        .HasName("pk_images");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("pastemyst.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("AvatarId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("avatar_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsContributor")
                        .HasColumnType("boolean")
                        .HasColumnName("is_contributor");

                    b.Property<bool>("IsSupporter")
                        .HasColumnType("boolean")
                        .HasColumnName("is_supporter");

                    b.Property<string>("ProviderId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("provider_id");

                    b.Property<string>("ProviderName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("provider_name");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("citext")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("AvatarId")
                        .HasDatabaseName("ix_users_avatar_id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_users_username");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("pastemyst.Models.User", b =>
                {
                    b.HasOne("pastemyst.Models.Image", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_images_avatar_id");

                    b.Navigation("Avatar");
                });
#pragma warning restore 612, 618
        }
    }
}
