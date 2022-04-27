using System;
using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.FuncionalidadeViewModels
{
	public class AlteracaoFuncionalidadeViewModel
	{
		[Required]
		public Guid IdFuncionalidade { get; set; }

		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NomeFuncionalidade { get; set; }
	}
}
