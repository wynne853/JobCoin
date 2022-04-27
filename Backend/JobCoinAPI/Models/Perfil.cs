using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobCoinAPI.Models
{
	public class Perfil
	{
		public Guid IdPerfil { get; set; }
		public string NomePerfil { get; set; }
		public ICollection<Usuario> Usuarios { get; set; }
		public ICollection<PerfilFuncionalidade> Funcionalidades { get; set; }
	}
}
