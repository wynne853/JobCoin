using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobCoinAPI.ViewModels.UsuarioViewModel
{
	public class AlteracaoUsuarioViewModel
	{
		public Guid IdPerfil { get; set; }
		public string Nome { get; set; }
		public string Senha { get; set; }
	}
}
