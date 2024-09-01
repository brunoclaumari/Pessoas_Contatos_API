using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pessoas_API.Migrations
{
    /// <inheritdoc />
    public partial class atualiza_email_unico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbPessoa_email",
                table: "tbPessoa",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbContato_email",
                table: "tbContato",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbPessoa_email",
                table: "tbPessoa");

            migrationBuilder.DropIndex(
                name: "IX_tbContato_email",
                table: "tbContato");
        }
    }
}
