using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.PerfilViewModels;

namespace JobCoinAPI.Mappers
{
	public class PerfilMapper
	{
		public static ConsultaPerfilViewModel ConverterParaConsultaViewModel(Perfil perfil)
		{
			return perfil == null ? null : new ConsultaPerfilViewModel
			{
				IdPerfil = perfil.IdPerfil,
				NomePerfil = perfil.NomePerfil
			};
		}

		public static IEnumerable<ConsultaPerfilViewModel> ConverterParaConsultaViewModel(IEnumerable<Perfil> perfis)
		{
			if (perfis == null || perfis.Count() == 0)
				return null;

			return perfis.Select(perfil => ConverterParaConsultaViewModel(perfil)).ToList();
		}
	}
}