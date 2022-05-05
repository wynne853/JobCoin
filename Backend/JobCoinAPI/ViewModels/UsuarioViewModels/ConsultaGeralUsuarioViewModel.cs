using System;
using JobCoinAPI.ViewModels.PerfilViewModels;

namespace JobCoinAPI.ViewModels.UsuarioViewModels
{
	public class ConsultaGeralUsuarioViewModel
	{
		public Guid IdUsuario { get; set; }

		public string Nome { get; set; }

		public string Email { get; set; }

		public ConsultaPerfilViewModel Perfil { get; set; }
	}
}