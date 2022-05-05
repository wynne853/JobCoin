using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.FuncionalidadeViewModels
{
	public class AlteracaoFuncionalidadeViewModel
	{

		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NomeFuncionalidade { get; set; }
	}
}