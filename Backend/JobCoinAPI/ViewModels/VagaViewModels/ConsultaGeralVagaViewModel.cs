using System;

namespace JobCoinAPI.ViewModels.VagaViewModels
{
	public class ConsultaGeralVagaViewModel
	{
		public Guid IdVaga { get; set; }

		public string NomeVaga { get; set; }

		public string DescricaoVaga { get; set; }

		public float ValorVaga { get; set; }

		public Guid IdUsuarioCriacaoVaga { get; set; }
	}
}