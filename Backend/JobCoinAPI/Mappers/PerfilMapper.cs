using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.PerfilViewModels;

namespace JobCoinAPI.Mappers
{
	public class PerfilMapper
	{
		public static ConsultaPerfilViewModel ConverterParaViewModel(Perfil perfil)
		{
			return new ConsultaPerfilViewModel
			{
				IdPerfil = perfil.IdPerfil,
				NomePerfil = perfil.NomePerfil
			};
		}

		public static ICollection<ConsultaPerfilViewModel> ConverterParaViewModel(ICollection<Perfil> perfis)
		{
			return perfis?.Select(perfil => ConverterParaViewModel(perfil)).ToList();
		}
	}
}