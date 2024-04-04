﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerLibrary.Data;

#nullable disable

namespace ServerLibrary.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240404111506_InitialCreate1")]
    partial class InitialCreate1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BaseLibrary.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("BaseLibrary.Entities.Employee4RetailStore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employee4RetailStores");
                });

            modelBuilder.Entity("BaseLibrary.Entities.IoTDevice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeesId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StoreAreaId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeesId");

                    b.HasIndex("StoreAreaId");

                    b.HasIndex("StoreId");

                    b.ToTable("ioTDevices");
                });

            modelBuilder.Entity("BaseLibrary.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("BaseLibrary.Entities.StoreArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StoreAreas");
                });

            modelBuilder.Entity("BaseLibrary.Entities.IoTDevice", b =>
                {
                    b.HasOne("BaseLibrary.Entities.Employee4RetailStore", "Employees")
                        .WithMany("ioTDDevices")
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BaseLibrary.Entities.StoreArea", "StoreArea")
                        .WithMany("ioTDDevices")
                        .HasForeignKey("StoreAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BaseLibrary.Entities.Store", "Store")
                        .WithMany("ioTDDevices")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employees");

                    b.Navigation("Store");

                    b.Navigation("StoreArea");
                });

            modelBuilder.Entity("BaseLibrary.Entities.Employee4RetailStore", b =>
                {
                    b.Navigation("ioTDDevices");
                });

            modelBuilder.Entity("BaseLibrary.Entities.Store", b =>
                {
                    b.Navigation("ioTDDevices");
                });

            modelBuilder.Entity("BaseLibrary.Entities.StoreArea", b =>
                {
                    b.Navigation("ioTDDevices");
                });
#pragma warning restore 612, 618
        }
    }
}
