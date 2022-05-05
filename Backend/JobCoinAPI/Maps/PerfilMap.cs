using JobCoinAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobCoinAPI.Maps
{
	public static class PerfilMap
	{
		public static void Map(this EntityTypeBuilder<Perfil> entity)
		{
			entity.ToTable("PERFIS");

			entity.HasKey(perfil => perfil.IdPerfil);

			entity.Property(perfil => perfil.IdPerfil)
				.HasColumnName("IdPerfil")
				.IsRequired();

			entity.Property(perfil => perfil.NomePerfil)
				.HasColumnName("NomePerfil")
				.IsRequired();
		}
	}
}