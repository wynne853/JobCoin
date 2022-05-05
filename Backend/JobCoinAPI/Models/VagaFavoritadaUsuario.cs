using System;

namespace JobCoinAPI.Models
{
	public class VagaFavoritadaUsuario
	{
		public Guid IdVaga { get; set; }

		public Vaga Vaga { get; set; }

		public Guid IdUsuario { get; set; }

		public Usuario Usuario { get; set; }
	}
}