using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobCoinAPI.Models
{
	public class Usuario
	{
		public Guid IdUsuario { get; set; }
		public Guid IdPerfil { get; set; }
		public Perfil Perfil { get; set; }
		public string Nome { get; set; }
		public string Email { get; set; }
		public string Senha { get; set; }
	}
}