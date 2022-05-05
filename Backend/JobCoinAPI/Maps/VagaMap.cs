using JobCoinAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobCoinAPI.Maps
{
	public static class VagaMap
	{
		public static void Map(this EntityTypeBuilder<Vaga> entity)
		{
			entity.ToTable("VAGAS");

			entity.HasKey(vaga => vaga.IdVaga);

			entity.HasOne(vaga => vaga.UsuarioCriacaoVaga)
				.WithMany(usuario => usuario.VagasCriadas)
				.HasForeignKey(vaga => vaga.IdUsuarioCriacaoVaga);

			entity.Property(vaga => vaga.IdVaga)
				.HasColumnName("IdVaga")
				.IsRequired();

			entity.Property(vaga => vaga.IdUsuarioCriacaoVaga)
				.HasColumnName("IdUsuarioCriacaoVaga")
				.IsRequired();

			entity.Property(vaga => vaga.NomeVaga)
				.HasColumnName("NomeVaga")
				.IsRequired();

			entity.Property(vaga => vaga.DescricaoVaga)
				.HasColumnName("DescricaoVaga")
				.IsRequired();

			entity.Property(vaga => vaga.ValorVaga)
				.HasColumnName("ValorVaga")
				.IsRequired();

			entity.Property(vaga => vaga.DataCriacaoVaga)
				.HasColumnName("DataCriacaoVaga")
				.IsRequired();

			entity.Property(vaga => vaga.DataAtualizacaoVaga)
				.HasColumnName("DataAtualizacaoVaga")
				.IsRequired();
		}
	}
}