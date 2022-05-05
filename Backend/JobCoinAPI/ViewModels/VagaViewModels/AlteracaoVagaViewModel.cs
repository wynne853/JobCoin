using System;
using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.VagaViewModels
{
	public class AlteracaoVagaViewModel
	{
		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NomeVaga { get; set; }

		[Required]
		public string DescricaoVaga { get; set; }

		[Range(0, float.MaxValue, ErrorMessage = "O valor da vaga não pode ser negativo.")]
		public float ValorVaga { get; set; }
	}
}