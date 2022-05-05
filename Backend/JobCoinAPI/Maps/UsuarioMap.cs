using JobCoinAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobCoinAPI.Maps
{
	public static class UsuarioMap
	{
		public static void Map(this EntityTypeBuilder<Usuario> entity)
		{
			entity.ToTable("USUARIOS");

			entity.HasKey(usuario => usuario.IdUsuario);

			entity.HasOne(usuario => usuario.Perfil)
				.WithMany()
				.HasForeignKey(usuario => usuario.IdPerfil);

			entity.Property(usuario => usuario.IdUsuario)
				.HasColumnName("IdUsuario")
				.IsRequired();

			entity.Property(usuario => usuario.IdPerfil)
				.HasColumnName("IdPerfil")
				.IsRequired();

			entity.Property(usuario => usuario.Nome)
				.HasColumnName("Nome")
				.IsRequired();

			entity.Property(usuario => usuario.Email)
				.HasColumnName("Email")
				.IsRequired();

			entity.Property(usuario => usuario.Senha)
				.HasColumnName("Senha")
				.IsRequired();
		}
	}
}