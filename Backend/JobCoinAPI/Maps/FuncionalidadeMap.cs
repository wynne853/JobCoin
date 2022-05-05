using JobCoinAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobCoinAPI.Maps
{
	public static class FuncionalidadeMap
	{
		public static void Map(this EntityTypeBuilder<Funcionalidade> entity)
		{
			entity.ToTable("FUNCIONALIDADES");

			entity.HasKey(funcionalidade => funcionalidade.IdFuncionalidade);

			entity.Property(funcionalidade => funcionalidade.IdFuncionalidade)
				.HasColumnName("IdFuncionalidade")
				.IsRequired();

			entity.Property(funcionalidade => funcionalidade.NomeFuncionalidade)
				.HasColumnName("NomeFuncionalidade")
				.IsRequired();
		}
	}
}