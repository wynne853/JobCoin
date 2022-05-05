using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.VagaViewModels;

namespace JobCoinAPI.Mappers
{
	public class VagaMapper
	{
		public static ConsultaGeralVagaViewModel ConverterParaConsultaGeralVagaViewModel(Vaga vaga)
		{
			return vaga == null ? null : new ConsultaGeralVagaViewModel
			{
				IdVaga = vaga.IdVaga,
				NomeVaga = vaga.NomeVaga,
				DescricaoVaga = vaga.DescricaoVaga,
				ValorVaga = vaga.ValorVaga,
				IdUsuarioCriacaoVaga = vaga.IdUsuarioCriacaoVaga
			};
		}

		public static ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(Vaga vaga)
		{
			return vaga == null ? null : new ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel
			{
				IdVaga = vaga.IdVaga,
				NomeVaga = vaga.NomeVaga,
				DescricaoVaga = vaga.DescricaoVaga,
				ValorVaga = vaga.ValorVaga
			};
		}

		public static ConsultaGeralVagaViewModel ConverterParaConsultaGeralVagaViewModel(VagaFavoritadaUsuario vagaFavoritada)
		{
			return vagaFavoritada == null ? null : new ConsultaGeralVagaViewModel
			{
				IdVaga = (System.Guid)(vagaFavoritada.Vaga?.IdVaga),
				NomeVaga = vagaFavoritada.Vaga?.NomeVaga,
				DescricaoVaga = vagaFavoritada.Vaga?.DescricaoVaga,
				ValorVaga = (float)(vagaFavoritada.Vaga?.ValorVaga)
			};
		}

		public static ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(VagaFavoritadaUsuario vagaFavoritada)
		{
			return vagaFavoritada == null ? null : new ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel
			{
				IdVaga = vagaFavoritada.IdVaga,
				NomeVaga = vagaFavoritada.Vaga?.NomeVaga,
				DescricaoVaga = vagaFavoritada.Vaga?.DescricaoVaga,
				ValorVaga = (float)(vagaFavoritada.Vaga?.ValorVaga)
			};
		}

		public static ConsultaUnicaVagaViewModel ConverterParaConsultaUnicaVagaViewModel(Vaga vaga)
		{
			return vaga == null ? null : new ConsultaUnicaVagaViewModel
			{
				IdVaga = vaga.IdVaga,
				NomeVaga = vaga.NomeVaga,
				DescricaoVaga = vaga.DescricaoVaga,
				ValorVaga = vaga.ValorVaga,
				DataCriacaoVaga = vaga.DataCriacaoVaga,
				DataAtualizacaoVaga = vaga.DataAtualizacaoVaga,
				UsuarioCriacaoVaga = UsuarioMapper.ConverterParaConsultaGeralUsuarioViewModel(vaga.UsuarioCriacaoVaga)
			};
		}

		public static IEnumerable<ConsultaGeralVagaViewModel> ConverterParaConsultaGeralVagaViewModel(IEnumerable<Vaga> vagas)
		{
			if (vagas == null || vagas.Count() == 0)
				return null;

			return vagas.Select(vaga => ConverterParaConsultaGeralVagaViewModel(vaga)).ToList();
		}

		public static IEnumerable<ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel> ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(IEnumerable<Vaga> vagas)
		{
			if (vagas == null || vagas.Count() == 0)
				return null;

			return vagas.Select(vaga => ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(vaga)).ToList();
		}

		public static IEnumerable<ConsultaGeralVagaViewModel> ConverterParaConsultaGeralVagaViewModel(IEnumerable<VagaFavoritadaUsuario> vagasFavoritadas)
		{
			if (vagasFavoritadas == null || vagasFavoritadas.Count() == 0)
				return null;

			return vagasFavoritadas.Select(vagaFavoritada => ConverterParaConsultaGeralVagaViewModel(vagaFavoritada)).ToList();
		}

		public static IEnumerable<ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel> ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(IEnumerable<VagaFavoritadaUsuario> vagasFavoritadas)
		{
			if (vagasFavoritadas == null || vagasFavoritadas.Count() == 0)
				return null;

			return vagasFavoritadas.Select(vagaFavoritada => ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(vagaFavoritada)).ToList();
		}

		public static IEnumerable<ConsultaUnicaVagaViewModel> ConverterParaConsultaUnicaVagaViewModel(IEnumerable<Vaga> vagas)
		{
			if (vagas == null || vagas.Count() == 0)
				return null;

			return vagas.Select(vaga => ConverterParaConsultaUnicaVagaViewModel(vaga)).ToList();
		}
	}
}