﻿// <auto-generated />
using System;
using Dex.Events.Distributed.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Dex.Events.Distributed.Tests.Migrations
{
    [DbContext(typeof(TestDbContext))]
    partial class TestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Dex.Events.Distributed.Tests.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Years")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Dex.Cap.OnceExecutor.LastTransaction", b =>
                {
                    b.Property<Guid>("IdempotentKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("IdempotentKey");

                    b.HasIndex("Created");

                    b.ToTable("last_transaction", "cap");
                });

            modelBuilder.Entity("Dex.Cap.Outbox.Models.OutboxEnvelope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActivityId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LockExpirationTimeUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasComment("Preventive timeout (maximum lifetime of actuality 'LockId')");

                    b.Property<Guid?>("LockId")
                        .HasColumnType("uuid")
                        .HasComment("Idempotency key (unique key of the thread that captured the lock)");

                    b.Property<TimeSpan>("LockTimeout")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("interval")
                        .HasDefaultValue(new TimeSpan(0, 0, 0, 30, 0))
                        .HasComment("Maximum allowable blocking time");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Retries")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUtc");

                    b.HasIndex("Retries");

                    b.HasIndex("Status");

                    b.ToTable("outbox", "cap");
                });
#pragma warning restore 612, 618
        }
    }
}
