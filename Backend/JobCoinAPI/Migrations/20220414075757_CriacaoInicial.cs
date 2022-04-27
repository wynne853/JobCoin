using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobCoinAPI.Migrations
{
    public partial class CriacaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FUNCIONALIDADE",
                columns: table => new
                {
                    IdFuncionalidade = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeFuncionalidade = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCIONALIDADE", x => x.IdFuncionalidade);
                });

            migrationBuilder.CreateTable(
                name: "PERFIL",
                columns: table => new
                {
                    IdPerfil = table.Column<Guid>(type: "uuid", nullable: false),
                    NomePerfil = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFIL", x => x.IdPerfil);
                });

            migrationBuilder.CreateTable(
                name: "PERFIL_FUNCIONALIDADE",
                columns: table => new
                {
                    IdPerfil = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFuncionalidade = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFIL_FUNCIONALIDADE", x => new { x.IdPerfil, x.IdFuncionalidade });
                    table.ForeignKey(
                        name: "FK_PERFIL_FUNCIONALIDADE_FUNCIONALIDADE_IdFuncionalidade",
                        column: x => x.IdFuncionalidade,
                        principalTable: "FUNCIONALIDADE",
                        principalColumn: "IdFuncionalidade",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERFIL_FUNCIONALIDADE_PERFIL_IdPerfil",
                        column: x => x.IdPerfil,
                        principalTable: "PERFIL",
                        principalColumn: "IdPerfil",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdPerfil = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Senha = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_USUARIO_PERFIL_IdPerfil",
                        column: x => x.IdPerfil,
                        principalTable: "PERFIL",
                        principalColumn: "IdPerfil",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PERFIL_FUNCIONALIDADE_IdFuncionalidade",
                table: "PERFIL_FUNCIONALIDADE",
                column: "IdFuncionalidade");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_IdPerfil",
                table: "USUARIO",
                column: "IdPerfil");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PERFIL_FUNCIONALIDADE");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "FUNCIONALIDADE");

            migrationBuilder.DropTable(
                name: "PERFIL");
        }
    }
}
