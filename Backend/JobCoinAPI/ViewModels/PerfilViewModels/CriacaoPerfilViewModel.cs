using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.PerfilViewModels
{
	public class CriacaoPerfilViewModel
	{
		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NomePerfil { get; set; }
	}
}