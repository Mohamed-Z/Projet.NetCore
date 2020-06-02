﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projet2_Archivage.Models;

namespace Projet2_Archivage.Migrations
{
    [DbContext(typeof(ArchiveContext))]
    partial class ArchiveContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Projet2_Archivage.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("confirmation");

                    b.Property<string>("email")
                        .IsRequired();

                    b.Property<string>("nom")
                        .IsRequired();

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("prenom")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("admins");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Calendrier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Date");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("calendriers");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Enseignant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("email");

                    b.Property<int?>("fil_id");

                    b.Property<string>("nom");

                    b.Property<string>("prenom");

                    b.HasKey("Id");

                    b.HasIndex("fil_id");

                    b.ToTable("enseignants");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Etudiant", b =>
                {
                    b.Property<int>("cne");

                    b.Property<string>("cin");

                    b.Property<string>("email");

                    b.Property<int?>("id_fil");

                    b.Property<string>("nom");

                    b.Property<double?>("note");

                    b.Property<string>("prenom");

                    b.Property<string>("tel");

                    b.HasKey("cne");

                    b.HasIndex("id_fil");

                    b.ToTable("etudiants");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Content");

                    b.Property<int>("Length");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.Property<string>("date_disp");

                    b.Property<int?>("groupe_Id");

                    b.Property<int?>("id_tp");

                    b.HasKey("Id");

                    b.HasIndex("groupe_Id");

                    b.HasIndex("id_tp");

                    b.ToTable("files");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Filiere", b =>
                {
                    b.Property<int>("Id_filiere")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nom_filiere");

                    b.HasKey("Id_filiere");

                    b.ToTable("filieres");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Groupe", b =>
                {
                    b.Property<int>("id_grp")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("date_stnc");

                    b.Property<int?>("id_ens");

                    b.Property<int?>("id_filiere");

                    b.Property<int?>("id_soc");

                    b.HasKey("id_grp");

                    b.HasIndex("id_ens");

                    b.HasIndex("id_filiere");

                    b.HasIndex("id_soc");

                    b.ToTable("groupes");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.GroupeMembre", b =>
                {
                    b.Property<int>("id_gm")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("grp_id");

                    b.Property<int?>("id_et");

                    b.HasKey("id_gm");

                    b.HasIndex("grp_id");

                    b.HasIndex("id_et");

                    b.ToTable("groupeMembres");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Societe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("description");

                    b.Property<string>("email_enc");

                    b.Property<int>("id_f");

                    b.Property<string>("nom");

                    b.Property<string>("nom_enc");

                    b.Property<string>("sujet");

                    b.Property<string>("tel");

                    b.Property<string>("tel_enc");

                    b.Property<string>("ville");

                    b.HasKey("Id");

                    b.HasIndex("id_f");

                    b.ToTable("societes");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Type_file", b =>
                {
                    b.Property<int>("id_type")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("date_depot");

                    b.Property<string>("nom_type");

                    b.HasKey("id_type");

                    b.ToTable("type_Files");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Enseignant", b =>
                {
                    b.HasOne("Projet2_Archivage.Models.Filiere", "Filiere")
                        .WithMany("enseignants")
                        .HasForeignKey("fil_id");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Etudiant", b =>
                {
                    b.HasOne("Projet2_Archivage.Models.Filiere", "Filiere")
                        .WithMany("etudiants")
                        .HasForeignKey("id_fil");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.File", b =>
                {
                    b.HasOne("Projet2_Archivage.Models.Groupe", "Groupe")
                        .WithMany("Files")
                        .HasForeignKey("groupe_Id");

                    b.HasOne("Projet2_Archivage.Models.Type_file", "Type_file")
                        .WithMany("files")
                        .HasForeignKey("id_tp");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Groupe", b =>
                {
                    b.HasOne("Projet2_Archivage.Models.Enseignant", "Enseignant")
                        .WithMany("groupes")
                        .HasForeignKey("id_ens");

                    b.HasOne("Projet2_Archivage.Models.Filiere", "Filiere")
                        .WithMany("groupes")
                        .HasForeignKey("id_filiere");

                    b.HasOne("Projet2_Archivage.Models.Societe", "Societe")
                        .WithMany("groupes")
                        .HasForeignKey("id_soc");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.GroupeMembre", b =>
                {
                    b.HasOne("Projet2_Archivage.Models.Groupe", "Groupe")
                        .WithMany("GroupeMembres")
                        .HasForeignKey("grp_id");

                    b.HasOne("Projet2_Archivage.Models.Etudiant", "Etudiant")
                        .WithMany("GroupeMembres")
                        .HasForeignKey("id_et");
                });

            modelBuilder.Entity("Projet2_Archivage.Models.Societe", b =>
                {
                    b.HasOne("Projet2_Archivage.Models.File", "File")
                        .WithMany()
                        .HasForeignKey("id_f")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
