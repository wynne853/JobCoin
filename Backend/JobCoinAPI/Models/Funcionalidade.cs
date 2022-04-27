using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobCoinAPI.Models
{
	public class Funcionalidade
	{
		public Guid IdFuncionalidade { get; set; }
		public string NomeFuncionalidade { get; set; }
		public ICollection<PerfilFuncionalidade> Perfis { get; set; }
	}
}