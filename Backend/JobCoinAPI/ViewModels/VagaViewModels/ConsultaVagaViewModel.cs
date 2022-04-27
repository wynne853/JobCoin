using System;

namespace JobCoinAPI.ViewModels.VagaViewModels
{
	public class ConsultaVagaViewModel
	{
		public Guid IdVaga { get; set; }

		public string NomeVaga { get; set; }

		public float ValorVaga { get; set; }

		public Guid IdUsuarioCriacaoVaga { get; set; }

		public DateTime DataCriacaoVaga { get; set; }

		public DateTime DataAtualizacaoVaga { get; set; }
	}
}
