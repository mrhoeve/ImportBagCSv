﻿// <auto-generated />
using ImportBagCsv.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImportBagCsv.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImportBagCsv.Models.Adres", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("PlaatsId")
                        .HasColumnType("bigint");

                    b.Property<string>("Postcode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Straat")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PlaatsId");

                    b.HasIndex("Straat", "Postcode");

                    b.ToTable("Adressen");
                });

            modelBuilder.Entity("ImportBagCsv.Models.Gemeente", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ProvincieId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Naam");

                    b.HasIndex("ProvincieId");

                    b.ToTable("Gemeenten");
                });

            modelBuilder.Entity("ImportBagCsv.Models.Nummer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AdresId")
                        .HasColumnType("bigint");

                    b.Property<string>("Huisletter")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Huisnummer")
                        .HasColumnType("int");

                    b.Property<string>("Huisnummertoevoeging")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AdresId");

                    b.HasIndex("Huisnummer", "Huisletter", "Huisnummertoevoeging");

                    b.ToTable("Nummers");
                });

            modelBuilder.Entity("ImportBagCsv.Models.Plaats", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GemeenteId")
                        .HasColumnType("bigint");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GemeenteId");

                    b.ToTable("Plaatsen");
                });

            modelBuilder.Entity("ImportBagCsv.Models.Provincie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Naam");

                    b.ToTable("Provincies");
                });

            modelBuilder.Entity("ImportBagCsv.Models.Adres", b =>
                {
                    b.HasOne("ImportBagCsv.Models.Plaats", "Plaats")
                        .WithMany("Adressen")
                        .HasForeignKey("PlaatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImportBagCsv.Models.Gemeente", b =>
                {
                    b.HasOne("ImportBagCsv.Models.Provincie", "Provincie")
                        .WithMany("Gemeenten")
                        .HasForeignKey("ProvincieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImportBagCsv.Models.Nummer", b =>
                {
                    b.HasOne("ImportBagCsv.Models.Adres", "Adres")
                        .WithMany("Nummers")
                        .HasForeignKey("AdresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImportBagCsv.Models.Plaats", b =>
                {
                    b.HasOne("ImportBagCsv.Models.Gemeente", "Gemeente")
                        .WithMany("Plaatsen")
                        .HasForeignKey("GemeenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
