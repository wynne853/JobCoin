namespace JobCoinAPI.ViewModels.LoginViewModels
{
	public class TokenViewModel
	{
		public bool Autenticado { get; set; }

		public string DataCriacao { get; set; }

		public string DataExpiracao { get; set; }

		public string Token { get; set; }
	}
}