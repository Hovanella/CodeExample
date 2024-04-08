﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CustomerName.Portal.Equipment.DataAccess.Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    [DbContext(typeof(EquipmentDbContext))]
    [Migration("20240208135236_EquipmentOdataView_AddFullNameToApproverAndActiveHolder_RemovedLastNameAndFullName")]
    partial class EquipmentOdataView_AddFullNameToApproverAndActiveHolder_RemovedLastNameAndFullName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("equipment")
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

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

                    b.Property<string>("InvoiceNumber")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("PurchaseCurrency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PurchasePlace")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PurchasePriceUsd")
                        .HasColumnType("numeric");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("TypeId")
                        .HasColumnType("character varying(32)");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("TypeId");

                    b.ToTable("Equipments", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentAssign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AssignedToUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

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

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AssembledAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAtUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<string>("DataHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.ToTable("EquipmentReports", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentReportRelevancePeriod", b =>
                {
                    b.Property<int>("EquipmentReportId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FromUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ToUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("EquipmentReportId", "FromUtc");

                    b.ToTable("EquipmentReportRelevancePeriods", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentType", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("EquipmentTypes", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentUser", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("Email");

                    b.HasIndex("IsDeleted");

                    b.ToTable("EquipmentUsers", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.Domain.OData.OdpEquipment", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("ActiveHolderDepartmentId")
                        .HasColumnType("text");

                    b.Property<string>("ActiveHolderFullName")
                        .HasColumnType("text");

                    b.Property<int?>("ActiveHolderId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ActiveHolderIssueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ActiveHolderReturnDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ApproverFullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ApproverId")
                        .HasColumnType("integer");

                    b.Property<string>("Characteristics")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("EquipmentTypeFullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EquipmentTypeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EquipmentTypeShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("GuaranteeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PurchaseCurrency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PurchasePlace")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PurchasePriceUsd")
                        .HasColumnType("numeric");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.ToView("EquipmentOdataView", "equipment");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.Equipment", b =>
                {
                    b.HasOne("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentUser", "Approver")
                        .WithMany("ApprovedEquipments")
                        .HasForeignKey("ApproverId");

                    b.HasOne("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");

                    b.Navigation("Approver");

                    b.Navigation("Type");
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

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentReportRelevancePeriod", b =>
                {
                    b.HasOne("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentReport", "EquipmentReport")
                        .WithMany("EquipmentReportRelevancePeriods")
                        .HasForeignKey("EquipmentReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EquipmentReport");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.Equipment", b =>
                {
                    b.Navigation("Assigns");
                });

            modelBuilder.Entity("CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities.EquipmentReport", b =>
                {
                    b.Navigation("EquipmentReportRelevancePeriods");
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
