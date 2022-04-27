using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.FuncionalidadeViewModels;

namespace JobCoinAPI.Mappers
{
	public class FuncionalidadeMapper
	{
		public static ConsultaFuncionalidadeViewModel ConverterParaViewModel(Funcionalidade funcionalidade)
		{
			return new ConsultaFuncionalidadeViewModel
			{
				IdFuncionalidade = funcionalidade.IdFuncionalidade,
				NomeFuncionalidade = funcionalidade.NomeFuncionalidade
			};
		}

		public static ICollection<ConsultaFuncionalidadeViewModel> ConverterParaViewModel(ICollection<Funcionalidade> funcionalidades)
		{
			return funcionalidades?.Select(funcionalidade => ConverterParaViewModel(funcionalidade)).ToList();
		}
	}
}