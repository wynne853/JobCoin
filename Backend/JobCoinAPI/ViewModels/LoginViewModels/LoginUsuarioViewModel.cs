using System.ComponentModel.DataAnnotations;

namespace JobCoinAPI.ViewModels.LoginViewModels
{
	public class LoginUsuarioViewModel
	{
		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Senha { get; set; }
	}
}