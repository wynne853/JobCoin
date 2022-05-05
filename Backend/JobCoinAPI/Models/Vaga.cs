using System;
using System.Collections.Generic;

namespace JobCoinAPI.Models
{
	public class Vaga
	{
		public Guid IdVaga { get; set; }

		public string NomeVaga { get; set; }

		public string DescricaoVaga { get; set; }

		public float ValorVaga { get; set; }

		public Guid IdUsuarioCriacaoVaga { get; set; }

		public Usuario UsuarioCriacaoVaga { get; set; }

		public DateTime DataCriacaoVaga { get; set; }

		public DateTime DataAtualizacaoVaga { get; set; }

		public IEnumerable<VagaFavoritadaUsuario> Usuarios { get; set; }
	}
}