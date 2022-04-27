using System;
using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.UsuarioViewModels
{
	public class AlteracaoUsuarioViewModel
	{
		[Required]
		public Guid IdUsuario { get; set; }

		[Required]
		public Guid IdPerfil { get; set; }
		
		[Required]
		public string Nome { get; set; }	
		
		[Required]
		public string Email { get; set; }
		
		[Required]
		public string Senha { get; set; }
	}
}