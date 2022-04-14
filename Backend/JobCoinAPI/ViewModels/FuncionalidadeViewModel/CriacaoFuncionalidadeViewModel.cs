using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobCoinAPI.ViewModels.FuncionalidadeViewModel
{
	public class CriacaoFuncionalidadeViewModel
	{
		public Guid IdUsuario { get; set; }
		public Guid IdPerfil { get; set; }
		public string Nome { get; set; }
		public string Email { get; set; }
		public string Senha { get; set; }
	}
}
