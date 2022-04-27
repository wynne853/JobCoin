using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobCoinAPI.ViewModels.UsuarioViewModel
{
	public class CriacaoUsuarioViewModel
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