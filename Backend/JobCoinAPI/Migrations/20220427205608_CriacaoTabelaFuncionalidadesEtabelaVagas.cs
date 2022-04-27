using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobCoinAPI.Migrations
{
    public partial class CriacaoTabelaFuncionalidadesEtabelaVagas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PerfilIdPerfil",
                table: "PERFIL_FUNCIONALIDADE",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VAGA",
                columns: table => new
                {
                    IdVaga = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeVaga = table.Column<string>(type: "text", nullable: false),
                    ValorVaga = table.Column<float>(type: "real", nullable: false),
                    IdUsuarioCriacaoVaga = table.Column<Guid>(type: "uuid", nullable: false),
                    DataCriacaoVaga = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacaoVaga = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UsuarioIdUsuario = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VAGA", x => x.IdVaga);
                    table.ForeignKey(
                        name: "FK_VAGA_USUARIO_IdUsuarioCriacaoVaga",
                        column: x => x.IdUsuarioCriacaoVaga,
                        principalTable: "USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VAGA_USUARIO_UsuarioIdUsuario",
                        column: x => x.UsuarioIdUsuario,
                        principalTable: "USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PERFIL_FUNCIONALIDADE_PerfilIdPerfil",
                table: "PERFIL_FUNCIONALIDADE",
                column: "PerfilIdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_VAGA_IdUsuarioCriacaoVaga",
                table: "VAGA",
                column: "IdUsuarioCriacaoVaga");

            migrationBuilder.CreateIndex(
                name: "IX_VAGA_UsuarioIdUsuario",
                table: "VAGA",
                column: "UsuarioIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_PERFIL_FUNCIONALIDADE_PERFIL_PerfilIdPerfil",
                table: "PERFIL_FUNCIONALIDADE",
                column: "PerfilIdPerfil",
                principalTable: "PERFIL",
                principalColumn: "IdPerfil",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PERFIL_FUNCIONALIDADE_PERFIL_PerfilIdPerfil",
                table: "PERFIL_FUNCIONALIDADE");

            migrationBuilder.DropTable(
                name: "VAGA");

            migrationBuilder.DropIndex(
                name: "IX_PERFIL_FUNCIONALIDADE_PerfilIdPerfil",
                table: "PERFIL_FUNCIONALIDADE");

            migrationBuilder.DropColumn(
                name: "PerfilIdPerfil",
                table: "PERFIL_FUNCIONALIDADE");
        }
    }
}
