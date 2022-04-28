using System;
using System.Collections.Generic;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.PerfilViewModels;
using JobCoinAPI.ViewModels.VagaViewModels;

namespace JobCoinAPI.ViewModels.UsuarioViewModels
{
	public class RetornoUsuarioViewModel
	{
		public Guid Id { get; set; }

		public Guid IdPerfil { get; set; }

		public string Nome { get; set; }

		public string Email { get; set; }
	}
}