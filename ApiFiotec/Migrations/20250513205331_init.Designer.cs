﻿// <auto-generated />
using System;
using ApiFiotec.Infraestruture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiFiotec.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250513205331_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApiFiotec.Models.Estado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NomeUf")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("uf");

                    b.HasKey("Id");

                    b.ToTable("tbl_estados");
                });

            modelBuilder.Entity("ApiFiotec.Models.Municipio", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("NomeMunicipio")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("municipio");

                    b.HasKey("Id");

                    b.HasIndex("EstadoId");

                    b.ToTable("tbl_municipios");
                });

            modelBuilder.Entity("ApiFiotec.Models.Relatorio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Arbovirose")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("arbovirose");

                    b.Property<int>("CodigoIbge")
                        .HasColumnType("int")
                        .HasColumnName("codigo_ibge");

                    b.Property<string>("DadosRelatorio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("dados_relatorio");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2")
                        .HasColumnName("data");

                    b.Property<string>("Municipio")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("municipio");

                    b.Property<int>("SemanaInicio")
                        .HasColumnType("int")
                        .HasColumnName("semana_inicio");

                    b.Property<int>("SemanaTermino")
                        .HasColumnType("int")
                        .HasColumnName("semana_termino");

                    b.Property<Guid>("SolicitanteId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("solicitanteId");

                    b.HasKey("Id");

                    b.HasIndex("SolicitanteId");

                    b.ToTable("tbl_relatorios");
                });

            modelBuilder.Entity("ApiFiotec.Models.Solicitante", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)")
                        .HasColumnName("cpf");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Cpf" }, "idx_cpf")
                        .IsUnique();

                    b.ToTable("tbl_solicitantes");
                });

            modelBuilder.Entity("ApiFiotec.Models.Municipio", b =>
                {
                    b.HasOne("ApiFiotec.Models.Estado", "Estado")
                        .WithMany("Municipios")
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("ApiFiotec.Models.Relatorio", b =>
                {
                    b.HasOne("ApiFiotec.Models.Solicitante", "Solicitante")
                        .WithMany()
                        .HasForeignKey("SolicitanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Solicitante");
                });

            modelBuilder.Entity("ApiFiotec.Models.Estado", b =>
                {
                    b.Navigation("Municipios");
                });
#pragma warning restore 612, 618
        }
    }
}
