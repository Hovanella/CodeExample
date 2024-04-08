﻿// <auto-generated />
#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    [DbContext(typeof(EquipmentDbContext))]
    [Migration("20230712065941_Equipment_AddSoftDeleteForAssign")]
    partial class Equipment_AddSoftDeleteForAssign
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("equipment")
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.Equipment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("ApproverId")
                        .HasColumnType("integer");

                    b.Property<string>("Characteristics")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<DateTime>("GuaranteeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PurchasePlace")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<double>("PurchasePrice")
                        .HasColumnType("double precision");

                    b.Property<double>("PurchasePriceUsd")
                        .HasColumnType("double precision");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .HasColumnType("tsvector");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("SearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SearchVector"), "GIN");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("Type");

                    b.ToTable("Equipments", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentAssign", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AssignedToUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<Guid>("EquipmentId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AssignedToUserId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("EquipmentAssigns", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentUser", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<string>("Department")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .HasColumnType("tsvector");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("Email");

                    b.HasIndex("SearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SearchVector"), "GIN");

                    b.ToTable("EquipmentUsers", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.Equipment", b =>
                {
                    b.HasOne("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentUser", "Approver")
                        .WithMany("ApprovedEquipments")
                        .HasForeignKey("ApproverId");

                    b.Navigation("Approver");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentAssign", b =>
                {
                    b.HasOne("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentUser", "User")
                        .WithMany("EquipmentAssigns")
                        .HasForeignKey("AssignedToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.Equipment", "Equipment")
                        .WithMany("Assigns")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.Equipment", b =>
                {
                    b.Navigation("Assigns");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentUser", b =>
                {
                    b.Navigation("ApprovedEquipments");

                    b.Navigation("EquipmentAssigns");
                });
#pragma warning restore 612, 618
        }
    }
}
