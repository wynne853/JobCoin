using System;
using System.Collections.Generic;

namespace JobCoinAPI.Models
{
	public class Perfil
	{
		public Guid IdPerfil { get; set; }

		public string NomePerfil { get; set; }

		public ICollection<PerfilFuncionalidade> Funcionalidades { get; set; }
	}
}