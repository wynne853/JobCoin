using JobCoinAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobCoinAPI.Maps
{
	public static class VagaFavoritadaUsuarioMap
	{
		public static void Map(this EntityTypeBuilder<VagaFavoritadaUsuario> entity)
		{
			entity.ToTable("VAGAS_FAVORITADAS");

			entity.HasKey(vagaFavoritada => new { vagaFavoritada.IdVaga, vagaFavoritada.IdUsuario });

			entity.HasOne(vagaFavoritada => vagaFavoritada.Vaga)
				.WithMany(vaga => vaga.Usuarios)
				.HasForeignKey(vagaFavoritada => vagaFavoritada.IdVaga);

			entity.HasOne(vagaFavoritada => vagaFavoritada.Usuario)
				.WithMany(usuario => usuario.VagasFavoritadas)
				.HasForeignKey(vagaFavoritada => vagaFavoritada.IdUsuario);

			entity.Property(vagaFavoritada => vagaFavoritada.IdVaga)
				.HasColumnName("IdVaga")
				.IsRequired();

			entity.Property(vagaFavoritada => vagaFavoritada.IdUsuario)
				.HasColumnName("IdUsuario")
				.IsRequired();
		}
	}
}