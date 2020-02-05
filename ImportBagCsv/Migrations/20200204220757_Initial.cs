using Microsoft.EntityFrameworkCore.Migrations;

namespace ImportBagCsv.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provincies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gemeenten",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true),
                    ProvincieId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gemeenten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gemeenten_Provincies_ProvincieId",
                        column: x => x.ProvincieId,
                        principalTable: "Provincies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plaatsen",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true),
                    GemeenteId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plaatsen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plaatsen_Gemeenten_GemeenteId",
                        column: x => x.GemeenteId,
                        principalTable: "Gemeenten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adressen",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Straat = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    PlaatsId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adressen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adressen_Plaatsen_PlaatsId",
                        column: x => x.PlaatsId,
                        principalTable: "Plaatsen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nummers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Huisnummer = table.Column<int>(nullable: false),
                    Huisletter = table.Column<string>(nullable: true),
                    Huisnummertoevoeging = table.Column<string>(nullable: true),
                    AdresId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nummers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nummers_Adressen_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adressen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adressen_PlaatsId",
                table: "Adressen",
                column: "PlaatsId");

            migrationBuilder.CreateIndex(
                name: "IX_Adressen_Straat_Postcode",
                table: "Adressen",
                columns: new[] { "Straat", "Postcode" });

            migrationBuilder.CreateIndex(
                name: "IX_Gemeenten_Naam",
                table: "Gemeenten",
                column: "Naam");

            migrationBuilder.CreateIndex(
                name: "IX_Gemeenten_ProvincieId",
                table: "Gemeenten",
                column: "ProvincieId");

            migrationBuilder.CreateIndex(
                name: "IX_Nummers_AdresId",
                table: "Nummers",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_Nummers_Huisnummer_Huisletter_Huisnummertoevoeging",
                table: "Nummers",
                columns: new[] { "Huisnummer", "Huisletter", "Huisnummertoevoeging" });

            migrationBuilder.CreateIndex(
                name: "IX_Plaatsen_GemeenteId",
                table: "Plaatsen",
                column: "GemeenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Provincies_Naam",
                table: "Provincies",
                column: "Naam");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nummers");

            migrationBuilder.DropTable(
                name: "Adressen");

            migrationBuilder.DropTable(
                name: "Plaatsen");

            migrationBuilder.DropTable(
                name: "Gemeenten");

            migrationBuilder.DropTable(
                name: "Provincies");
        }
    }
}
