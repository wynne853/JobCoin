using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.VagaViewModels;

namespace JobCoinAPI.Mappers
{
	public class VagaMapper
	{
		public static ConsultaVagaViewModel ConverterParaViewModel(Vaga vaga)
		{
			return new ConsultaVagaViewModel
			{
				IdVaga = vaga.IdVaga,
				NomeVaga = vaga.NomeVaga,
				ValorVaga = vaga.ValorVaga,
				IdUsuarioCriacaoVaga = vaga.IdUsuarioCriacaoVaga,
				DataCriacaoVaga = vaga.DataCriacaoVaga,
				DataAtualizacaoVaga = vaga.DataAtualizacaoVaga
			};
		}

		public static IEnumerable<ConsultaVagaViewModel> ConverterParaViewModel(IEnumerable<Vaga> vagas)
		{
			return vagas?.Select(vaga => ConverterParaViewModel(vaga)).ToList();
		}
	}
}