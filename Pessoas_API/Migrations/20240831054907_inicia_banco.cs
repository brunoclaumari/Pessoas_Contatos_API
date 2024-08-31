using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pessoas_API.Migrations
{
    /// <inheritdoc />
    public partial class inicia_banco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbPessoa",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbPessoa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbContato",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "text", nullable: false),
                    telefone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    whatsapp = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    pessoa_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbContato", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbContato_tbPessoa_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "tbPessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tbPessoa",
                columns: new[] { "id", "email", "nome" },
                values: new object[,]
                {
                    { 1L, "mariasanta@gmail.com", "Maria Santana" },
                    { 2L, "jcarlos@gmail.com", "João Carlos da Costa" },
                    { 3L, "ricardopr@gmail.com", "Ricardo Pereira dos Santos" }
                });

            migrationBuilder.InsertData(
                table: "tbContato",
                columns: new[] { "id", "email", "nome", "pessoa_id", "telefone", "whatsapp" },
                values: new object[,]
                {
                    { 1L, "joana@gmail.com", "Joana Santana", 1L, "(11) 91111-1111", "" },
                    { 2L, "raquelst@gmail.com", "Raquel Santana", 1L, "(11) 92222-2222", "" },
                    { 3L, "jalberto@gmail.com", "João Alberto Santana", 1L, "(11) 93333-3333", "" },
                    { 4L, "viniribeiro@gmail.com", "Vinícius Ribeiro", 2L, "(11) 94444-4444", "" },
                    { 5L, "josevieira@gmail.com", "José Vieira", 2L, "(11) 95555-5555", "" },
                    { 6L, "leilasanches@gmail.com", "Leila Sanches", 3L, "(11) 96666-6666", "(11) 96666-6666" },
                    { 7L, "luanafr@gmail.com", "Luana de Freitas", 3L, "(11) 97777-7777", "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbContato_pessoa_id",
                table: "tbContato",
                column: "pessoa_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbContato");

            migrationBuilder.DropTable(
                name: "tbPessoa");
        }
    }
}
