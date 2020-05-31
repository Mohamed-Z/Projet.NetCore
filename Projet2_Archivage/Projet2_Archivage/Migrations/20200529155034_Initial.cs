using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projet2_Archivage.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nom = table.Column<string>(nullable: false),
                    prenom = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    password = table.Column<string>(maxLength: 20, nullable: false),
                    confirmation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "calendriers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "enseignants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nom = table.Column<string>(nullable: true),
                    prenom = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enseignants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "filieres",
                columns: table => new
                {
                    Id_filiere = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nom_filiere = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filieres", x => x.Id_filiere);
                });

            migrationBuilder.CreateTable(
                name: "societes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nom = table.Column<string>(nullable: true),
                    tel = table.Column<string>(nullable: true),
                    ville = table.Column<string>(nullable: true),
                    nom_enc = table.Column<string>(nullable: true),
                    email_enc = table.Column<string>(nullable: true),
                    tel_enc = table.Column<string>(nullable: true),
                    sujet = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    id_f = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_societes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "type_Files",
                columns: table => new
                {
                    id_type = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nom_type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_type_Files", x => x.id_type);
                });

            migrationBuilder.CreateTable(
                name: "etudiants",
                columns: table => new
                {
                    cne = table.Column<int>(nullable: false),
                    nom = table.Column<string>(nullable: true),
                    prenom = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    tel = table.Column<string>(nullable: true),
                    cin = table.Column<string>(nullable: true),
                    id_fil = table.Column<int>(nullable: true),
                    note = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etudiants", x => x.cne);
                    table.ForeignKey(
                        name: "FK_etudiants_filieres_id_fil",
                        column: x => x.id_fil,
                        principalTable: "filieres",
                        principalColumn: "Id_filiere",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "groupes",
                columns: table => new
                {
                    id_grp = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    id_ens = table.Column<int>(nullable: true),
                    id_filiere = table.Column<int>(nullable: true),
                    id_soc = table.Column<int>(nullable: true),
                    date_stnc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupes", x => x.id_grp);
                    table.ForeignKey(
                        name: "FK_groupes_enseignants_id_ens",
                        column: x => x.id_ens,
                        principalTable: "enseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_groupes_filieres_id_filiere",
                        column: x => x.id_filiere,
                        principalTable: "filieres",
                        principalColumn: "Id_filiere",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_groupes_societes_id_soc",
                        column: x => x.id_soc,
                        principalTable: "societes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true),
                    date_disp = table.Column<string>(nullable: true),
                    groupe_Id = table.Column<int>(nullable: true),
                    id_tp = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_files_groupes_groupe_Id",
                        column: x => x.groupe_Id,
                        principalTable: "groupes",
                        principalColumn: "id_grp",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_files_type_Files_id_tp",
                        column: x => x.id_tp,
                        principalTable: "type_Files",
                        principalColumn: "id_type",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "groupeMembres",
                columns: table => new
                {
                    id_gm = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    grp_id = table.Column<int>(nullable: true),
                    id_et = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupeMembres", x => x.id_gm);
                    table.ForeignKey(
                        name: "FK_groupeMembres_groupes_grp_id",
                        column: x => x.grp_id,
                        principalTable: "groupes",
                        principalColumn: "id_grp",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_groupeMembres_etudiants_id_et",
                        column: x => x.id_et,
                        principalTable: "etudiants",
                        principalColumn: "cne",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_etudiants_id_fil",
                table: "etudiants",
                column: "id_fil");

            migrationBuilder.CreateIndex(
                name: "IX_files_groupe_Id",
                table: "files",
                column: "groupe_Id");

            migrationBuilder.CreateIndex(
                name: "IX_files_id_tp",
                table: "files",
                column: "id_tp");

            migrationBuilder.CreateIndex(
                name: "IX_groupeMembres_grp_id",
                table: "groupeMembres",
                column: "grp_id");

            migrationBuilder.CreateIndex(
                name: "IX_groupeMembres_id_et",
                table: "groupeMembres",
                column: "id_et");

            migrationBuilder.CreateIndex(
                name: "IX_groupes_id_ens",
                table: "groupes",
                column: "id_ens");

            migrationBuilder.CreateIndex(
                name: "IX_groupes_id_filiere",
                table: "groupes",
                column: "id_filiere");

            migrationBuilder.CreateIndex(
                name: "IX_groupes_id_soc",
                table: "groupes",
                column: "id_soc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "calendriers");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "groupeMembres");

            migrationBuilder.DropTable(
                name: "type_Files");

            migrationBuilder.DropTable(
                name: "groupes");

            migrationBuilder.DropTable(
                name: "etudiants");

            migrationBuilder.DropTable(
                name: "enseignants");

            migrationBuilder.DropTable(
                name: "societes");

            migrationBuilder.DropTable(
                name: "filieres");
        }
    }
}
