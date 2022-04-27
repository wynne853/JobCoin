using System;
using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.PerfilViewModels
{
	public class AlteracaoPerfilViewModel
	{
		[Required]
		public Guid IdPerfil { get; set; }

		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NomePerfil { get; set; }
	}
}