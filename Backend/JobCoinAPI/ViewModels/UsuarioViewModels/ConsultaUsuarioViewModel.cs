using System;
using System.Collections.Generic;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.PerfilViewModels;
using JobCoinAPI.ViewModels.VagaViewModels;

namespace JobCoinAPI.ViewModels.UsuarioViewModels
{
	public class ConsultaUsuarioViewModel
	{
		public Guid Id { get; set; }
		
		public ConsultaPerfilViewModel Perfil { get; set; }
		
		public string Nome { get; set; }
		
		public string Email { get; set; }

		public IEnumerable<ConsultaVagaViewModel> VagasCriadas { get; set; }

		public IEnumerable<ConsultaVagaViewModel> VagasFavoritadas { get; set; }
	}
}