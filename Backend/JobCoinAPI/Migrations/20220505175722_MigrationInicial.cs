using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobCoinAPI.Migrations
{
    public partial class MigrationInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FUNCIONALIDADES",
                columns: table => new
                {
                    IdFuncionalidade = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeFuncionalidade = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCIONALIDADES", x => x.IdFuncionalidade);
                });

            migrationBuilder.CreateTable(
                name: "PERFIS",
                columns: table => new
                {
                    IdPerfil = table.Column<Guid>(type: "uuid", nullable: false),
                    NomePerfil = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFIS", x => x.IdPerfil);
                });

            migrationBuilder.CreateTable(
                name: "PERFIL_FUNCIONALIDADES",
                columns: table => new
                {
                    IdPerfil = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFuncionalidade = table.Column<Guid>(type: "uuid", nullable: false),
                    PerfilIdPerfil = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFIL_FUNCIONALIDADES", x => new { x.IdPerfil, x.IdFuncionalidade });
                    table.ForeignKey(
                        name: "FK_PERFIL_FUNCIONALIDADES_FUNCIONALIDADES_IdFuncionalidade",
                        column: x => x.IdFuncionalidade,
                        principalTable: "FUNCIONALIDADES",
                        principalColumn: "IdFuncionalidade",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERFIL_FUNCIONALIDADES_PERFIS_IdPerfil",
                        column: x => x.IdPerfil,
                        principalTable: "PERFIS",
                        principalColumn: "IdPerfil",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERFIL_FUNCIONALIDADES_PERFIS_PerfilIdPerfil",
                        column: x => x.PerfilIdPerfil,
                        principalTable: "PERFIS",
                        principalColumn: "IdPerfil",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS",
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
                    table.PrimaryKey("PK_USUARIOS", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_USUARIOS_PERFIS_IdPerfil",
                        column: x => x.IdPerfil,
                        principalTable: "PERFIS",
                        principalColumn: "IdPerfil",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VAGAS",
                columns: table => new
                {
                    IdVaga = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeVaga = table.Column<string>(type: "text", nullable: false),
                    DescricaoVaga = table.Column<string>(type: "text", nullable: false),
                    ValorVaga = table.Column<float>(type: "real", nullable: false),
                    IdUsuarioCriacaoVaga = table.Column<Guid>(type: "uuid", nullable: false),
                    DataCriacaoVaga = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacaoVaga = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VAGAS", x => x.IdVaga);
                    table.ForeignKey(
                        name: "FK_VAGAS_USUARIOS_IdUsuarioCriacaoVaga",
                        column: x => x.IdUsuarioCriacaoVaga,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VAGAS_FAVORITADAS",
                columns: table => new
                {
                    IdVaga = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VAGAS_FAVORITADAS", x => new { x.IdVaga, x.IdUsuario });
                    table.ForeignKey(
                        name: "FK_VAGAS_FAVORITADAS_USUARIOS_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VAGAS_FAVORITADAS_VAGAS_IdVaga",
                        column: x => x.IdVaga,
                        principalTable: "VAGAS",
                        principalColumn: "IdVaga",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PERFIL_FUNCIONALIDADES_IdFuncionalidade",
                table: "PERFIL_FUNCIONALIDADES",
                column: "IdFuncionalidade");

            migrationBuilder.CreateIndex(
                name: "IX_PERFIL_FUNCIONALIDADES_PerfilIdPerfil",
                table: "PERFIL_FUNCIONALIDADES",
                column: "PerfilIdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_IdPerfil",
                table: "USUARIOS",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_VAGAS_IdUsuarioCriacaoVaga",
                table: "VAGAS",
                column: "IdUsuarioCriacaoVaga");

            migrationBuilder.CreateIndex(
                name: "IX_VAGAS_FAVORITADAS_IdUsuario",
                table: "VAGAS_FAVORITADAS",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PERFIL_FUNCIONALIDADES");

            migrationBuilder.DropTable(
                name: "VAGAS_FAVORITADAS");

            migrationBuilder.DropTable(
                name: "FUNCIONALIDADES");

            migrationBuilder.DropTable(
                name: "VAGAS");

            migrationBuilder.DropTable(
                name: "USUARIOS");

            migrationBuilder.DropTable(
                name: "PERFIS");
        }
    }
}
