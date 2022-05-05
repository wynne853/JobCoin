using System;

namespace JobCoinAPI.ViewModels.VagaViewModels
{
	public class ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel
	{
		public Guid IdVaga { get; set; }

		public string NomeVaga { get; set; }

		public string DescricaoVaga { get; set; }

		public float ValorVaga { get; set; }
	}
}