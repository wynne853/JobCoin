using System;
using System.Collections.Generic;
using JobCoinAPI.ViewModels.PerfilViewModels;
using JobCoinAPI.ViewModels.VagaViewModels;

namespace JobCoinAPI.ViewModels.UsuarioViewModels
{
	public class ConsultaUnicaUsuarioViewModel
	{
		public Guid IdUsuario { get; set; }
		
		public string Nome { get; set; }
		
		public string Email { get; set; }

		public ConsultaPerfilViewModel Perfil { get; set; }

		public IEnumerable<ConsultaGeralVagaViewModel> VagasCriadas { get; set; }

		public IEnumerable<ConsultaGeralVagaViewModel> VagasFavoritadas { get; set; }
	}
}