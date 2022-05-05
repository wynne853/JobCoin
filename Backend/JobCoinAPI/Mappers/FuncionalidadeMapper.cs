using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.FuncionalidadeViewModels;

namespace JobCoinAPI.Mappers
{
	public class FuncionalidadeMapper
	{
		public static ConsultaFuncionalidadeViewModel ConverterParaConsultaViewModel(Funcionalidade funcionalidade)
		{
			return funcionalidade == null ? null : new ConsultaFuncionalidadeViewModel
			{
				IdFuncionalidade = funcionalidade.IdFuncionalidade,
				NomeFuncionalidade = funcionalidade.NomeFuncionalidade
			};
		}

		public static IEnumerable<ConsultaFuncionalidadeViewModel> ConverterParaConsultaViewModel(IEnumerable<Funcionalidade> funcionalidades)
		{
			if (funcionalidades == null || funcionalidades.Count() == 0)
				return null;

			return funcionalidades.Select(funcionalidade => ConverterParaConsultaViewModel(funcionalidade)).ToList();
		}
	}
}