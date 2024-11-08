﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Test12.DataAccess.Data;

#nullable disable

namespace UerInterface.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Clean.Cleaning", b =>
                {
                    b.Property<int>("CleaningID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CleaningID"));

                    b.Property<int?>("BrandFK")
                        .HasColumnType("int");

                    b.Property<double?>("CleaningOrder")
                        .HasColumnType("float");

                    b.Property<string>("DeviceName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Note")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CleaningID");

                    b.HasIndex("BrandFK");

                    b.ToTable("Cleaning", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Clean.CleaningSteps", b =>
                {
                    b.Property<int>("CleaStepsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CleaStepsID"));

                    b.Property<string>("CleaStepsImage")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("CleaStepsNum")
                        .HasColumnType("int");

                    b.Property<string>("CleaText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CleaningFK")
                        .HasColumnType("int");

                    b.HasKey("CleaStepsID");

                    b.HasIndex("CleaningFK");

                    b.ToTable("CleaningSteps", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Device_Tools.DevicesAndTools", b =>
                {
                    b.Property<int>("DevicesAndToolsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DevicesAndToolsID"));

                    b.Property<int?>("BrandFK")
                        .HasColumnType("int");

                    b.Property<double?>("DevicesAndToolsOrder")
                        .HasColumnType("float");

                    b.Property<string>("DevicesAndTools_Image")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DevicesAndTools_Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("DevicesAndTools_Num")
                        .HasColumnType("int");

                    b.HasKey("DevicesAndToolsID");

                    b.HasIndex("BrandFK");

                    b.ToTable("DevicesAndTools", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Food.FoodStuffs", b =>
                {
                    b.Property<int>("FoodStuffsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FoodStuffsID"));

                    b.Property<int?>("BrandFK")
                        .HasColumnType("int");

                    b.Property<string>("FoodStuffsImage")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FoodStuffsName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("FoodStuffsNum")
                        .HasColumnType("int");

                    b.Property<double?>("FoodStuffsOrder")
                        .HasColumnType("float");

                    b.HasKey("FoodStuffsID");

                    b.HasIndex("BrandFK");

                    b.ToTable("FoodStuffs", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.MainSections", b =>
                {
                    b.Property<int>("MainSectionsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MainSectionsID"));

                    b.Property<int?>("BrandFK")
                        .HasColumnType("int");

                    b.Property<double?>("MainSectionsOrder")
                        .HasColumnType("float");

                    b.Property<string>("SectionsImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SectionsName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MainSectionsID");

                    b.HasIndex("BrandFK");

                    b.ToTable("MainSections", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.PreparationIngredients", b =>
                {
                    b.Property<int>("PrepIngredientsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrepIngredientsID"));

                    b.Property<string>("PrepIngredientsName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PrepQuantity")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PrepUnit")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PreparationsFK")
                        .HasColumnType("int");

                    b.HasKey("PrepIngredientsID");

                    b.HasIndex("PreparationsFK");

                    b.ToTable("PreparationIngredients", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.PreparationSteps", b =>
                {
                    b.Property<int>("PrepStepsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrepStepsID"));

                    b.Property<string>("PrepImage")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("PrepStepsNum")
                        .HasColumnType("int");

                    b.Property<string>("PrepText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PreparationsFK")
                        .HasColumnType("int");

                    b.HasKey("PrepStepsID");

                    b.HasIndex("PreparationsFK");

                    b.ToTable("PreparationSteps", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.PreparationTools", b =>
                {
                    b.Property<int>("PrepToolsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrepToolsID"));

                    b.Property<string>("PrepTools")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PreparationsFK")
                        .HasColumnType("int");

                    b.HasKey("PrepToolsID");

                    b.HasIndex("PreparationsFK");

                    b.ToTable("PreparationTools", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.Preparations", b =>
                {
                    b.Property<int>("PreparationsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PreparationsID"));

                    b.Property<int?>("BrandFK")
                        .HasColumnType("int");

                    b.Property<string>("Expiry")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("NetWeight")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PreparationTime")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double?>("PreparationsOrder")
                        .HasColumnType("float");

                    b.Property<string>("Station")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("VersionNumber")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("prepareImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("prepareName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("PreparationsID");

                    b.HasIndex("BrandFK");

                    b.ToTable("Preparations", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Production.Production", b =>
                {
                    b.Property<int>("ProductionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductionID"));

                    b.Property<int?>("BrandFK")
                        .HasColumnType("int");

                    b.Property<string>("Expiry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreparationTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("ProductionOrder")
                        .HasColumnType("float");

                    b.Property<string>("Station")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VersionNumber")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ProductionID");

                    b.HasIndex("BrandFK");

                    b.ToTable("Production", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Production.ProductionIngredients", b =>
                {
                    b.Property<int>("ProdIngredientsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProdIngredientsID"));

                    b.Property<string>("ProdIngredientsName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ProdQuantity")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ProdUnit")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ProductionFK")
                        .HasColumnType("int");

                    b.HasKey("ProdIngredientsID");

                    b.HasIndex("ProductionFK");

                    b.ToTable("ProductionIngredients", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Production.ProductionSteps", b =>
                {
                    b.Property<int>("ProdStepsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProdStepsID"));

                    b.Property<string>("ProdSImage")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("ProdStepsNum")
                        .HasColumnType("int");

                    b.Property<string>("ProdText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductionFK")
                        .HasColumnType("int");

                    b.HasKey("ProdStepsID");

                    b.HasIndex("ProductionFK");

                    b.ToTable("ProductionSteps", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.Production.ProductionTools", b =>
                {
                    b.Property<int>("ProdToolsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProdToolsID"));

                    b.Property<string>("ProdTools")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ProductionFK")
                        .HasColumnType("int");

                    b.HasKey("ProdToolsID");

                    b.HasIndex("ProductionFK");

                    b.ToTable("ProductionTools", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.ReadyFood.ReadyProducts", b =>
                {
                    b.Property<int>("ReadyProductsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReadyProductsID"));

                    b.Property<int?>("BrandFK")
                        .HasColumnType("int");

                    b.Property<string>("ReadyProductsImage")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ReadyProductsName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double?>("ReadyProductsOrder")
                        .HasColumnType("float");

                    b.HasKey("ReadyProductsID");

                    b.HasIndex("BrandFK");

                    b.ToTable("ReadyProducts", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.trade_mark.Brands", b =>
                {
                    b.Property<int>("BrandID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrandID"));

                    b.Property<string>("BrandCoverImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrandFooterImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrandLogoImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrandName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ClientID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBY")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("Date1")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("BrandID");

                    b.HasIndex("ClientID");

                    b.ToTable("Brands", (string)null);
                });

            modelBuilder.Entity("Test12.Models.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpirationDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("isActive")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Test12.Models.Models.Clean.Cleaning", b =>
                {
                    b.HasOne("Test12.Models.Models.trade_mark.Brands", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandFK");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Test12.Models.Models.Clean.CleaningSteps", b =>
                {
                    b.HasOne("Test12.Models.Models.Clean.Cleaning", "Cleaning")
                        .WithMany()
                        .HasForeignKey("CleaningFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cleaning");
                });

            modelBuilder.Entity("Test12.Models.Models.Device_Tools.DevicesAndTools", b =>
                {
                    b.HasOne("Test12.Models.Models.trade_mark.Brands", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandFK");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Test12.Models.Models.Food.FoodStuffs", b =>
                {
                    b.HasOne("Test12.Models.Models.trade_mark.Brands", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandFK");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Test12.Models.Models.MainSections", b =>
                {
                    b.HasOne("Test12.Models.Models.trade_mark.Brands", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandFK");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.PreparationIngredients", b =>
                {
                    b.HasOne("Test12.Models.Models.Preparation.Preparations", "Preparation")
                        .WithMany("componentsCountPrint")
                        .HasForeignKey("PreparationsFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preparation");
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.PreparationSteps", b =>
                {
                    b.HasOne("Test12.Models.Models.Preparation.Preparations", "Preparation")
                        .WithMany("stepsCountPrint")
                        .HasForeignKey("PreparationsFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preparation");
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.PreparationTools", b =>
                {
                    b.HasOne("Test12.Models.Models.Preparation.Preparations", "Preparation")
                        .WithMany("toolsCountPrint")
                        .HasForeignKey("PreparationsFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preparation");
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.Preparations", b =>
                {
                    b.HasOne("Test12.Models.Models.trade_mark.Brands", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandFK");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Test12.Models.Models.Production.Production", b =>
                {
                    b.HasOne("Test12.Models.Models.trade_mark.Brands", "Brands")
                        .WithMany()
                        .HasForeignKey("BrandFK");

                    b.Navigation("Brands");
                });

            modelBuilder.Entity("Test12.Models.Models.Production.ProductionIngredients", b =>
                {
                    b.HasOne("Test12.Models.Models.Production.Production", "Production")
                        .WithMany("component2")
                        .HasForeignKey("ProductionFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Production");
                });

            modelBuilder.Entity("Test12.Models.Models.Production.ProductionSteps", b =>
                {
                    b.HasOne("Test12.Models.Models.Production.Production", "Production")
                        .WithMany("stepsCountPrint2")
                        .HasForeignKey("ProductionFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Production");
                });

            modelBuilder.Entity("Test12.Models.Models.Production.ProductionTools", b =>
                {
                    b.HasOne("Test12.Models.Models.Production.Production", "Production")
                        .WithMany("toolsCountPrint2")
                        .HasForeignKey("ProductionFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Production");
                });

            modelBuilder.Entity("Test12.Models.Models.ReadyFood.ReadyProducts", b =>
                {
                    b.HasOne("Test12.Models.Models.trade_mark.Brands", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandFK");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Test12.Models.Models.trade_mark.Brands", b =>
                {
                    b.HasOne("Test12.Models.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ClientID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Test12.Models.Models.Preparation.Preparations", b =>
                {
                    b.Navigation("componentsCountPrint");

                    b.Navigation("stepsCountPrint");

                    b.Navigation("toolsCountPrint");
                });

            modelBuilder.Entity("Test12.Models.Models.Production.Production", b =>
                {
                    b.Navigation("component2");

                    b.Navigation("stepsCountPrint2");

                    b.Navigation("toolsCountPrint2");
                });
#pragma warning restore 612, 618
        }
    }
}
