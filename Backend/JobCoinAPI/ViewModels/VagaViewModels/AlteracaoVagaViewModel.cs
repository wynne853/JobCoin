using System;
using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.VagaViewModels
{
	public class AlteracaoVagaViewModel
	{
		[Required]
		public Guid IdVaga { get; set; }

		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NomeVaga { get; set; }

		[Range(0, float.MaxValue, ErrorMessage = "O valor da vaga deve ser maior que 0.")]
		public float ValorVaga { get; set; }
	}
}
