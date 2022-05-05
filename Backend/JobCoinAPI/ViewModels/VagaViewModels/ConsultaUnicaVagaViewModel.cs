using System;
using JobCoinAPI.ViewModels.UsuarioViewModels;

namespace JobCoinAPI.ViewModels.VagaViewModels
{
	public class ConsultaUnicaVagaViewModel
	{
		public Guid IdVaga { get; set; }

		public string NomeVaga { get; set; }

		public string DescricaoVaga { get; set; }

		public float ValorVaga { get; set; }

		public DateTime DataCriacaoVaga { get; set; }

		public DateTime DataAtualizacaoVaga { get; set; }

		public ConsultaGeralUsuarioViewModel UsuarioCriacaoVaga { get; set; }
	}
}