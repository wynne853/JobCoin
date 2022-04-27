using System;

namespace JobCoinAPI.Models
{
	public class PerfilFuncionalidade
	{
		public Guid IdPerfil { get; set; }
		
		public Perfil Perfil { get; set; }
		
		public Guid IdFuncionalidade { get; set; }
		
		public Funcionalidade Funcionalidade { get; set; }
	}
}