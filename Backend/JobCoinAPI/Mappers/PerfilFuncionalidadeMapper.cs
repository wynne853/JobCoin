using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.PerfilFuncionalidadeViewModels;

namespace JobCoinAPI.Mappers
{
	public class PerfilFuncionalidadeMapper
	{
		public static ConsultaPerfilFuncionalidadeViewModel ConverterParaConsultaViewModel(PerfilFuncionalidade perfilFuncionalidade)
		{
			return perfilFuncionalidade == null ? null : new ConsultaPerfilFuncionalidadeViewModel
			{
				IdPerfil = perfilFuncionalidade.IdPerfil,
				NomePerfil = perfilFuncionalidade.Perfil?.NomePerfil,
				IdFuncionalidade = perfilFuncionalidade.IdFuncionalidade,
				NomeFuncionalidade = perfilFuncionalidade.Funcionalidade?.NomeFuncionalidade
			};
		}

		public static IEnumerable<ConsultaPerfilFuncionalidadeViewModel> ConverterParaConsultaViewModel(IEnumerable<PerfilFuncionalidade> perfisFuncionalidades)
		{
			if (perfisFuncionalidades == null || perfisFuncionalidades.Count() == 0)
				return null;

			return perfisFuncionalidades.Select(vaga => ConverterParaConsultaViewModel(vaga)).ToList();
		}
	}
}