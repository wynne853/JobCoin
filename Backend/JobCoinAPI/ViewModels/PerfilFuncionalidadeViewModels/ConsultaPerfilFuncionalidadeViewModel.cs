using System;

namespace JobCoinAPI.ViewModels.PerfilFuncionalidadeViewModels
{
	public class ConsultaPerfilFuncionalidadeViewModel
	{
		public Guid IdPerfil { get; set; }

		public string NomePerfil { get; set; }

		public Guid IdFuncionalidade { get; set; }

		public string NomeFuncionalidade { get; set; }
	}
}