using System;
using System.Linq;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using JobCoinAPI.Shared;
using JobCoinAPI.ViewModels.LoginViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobCoinAPI.Controllers
{
	/// <response code="200">Ok</response>
	/// <response code="400">Bad Request</response>
	/// <response code="404">Not Found</response>
	/// <response code="500">Internal Server Error</response>
	[ApiController]
	[Route("v1/login")]
	public class LoginController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> Authenticate(
			[FromServices] DataContext context,
			[FromServices] Autenticacao autenticacao,
			[FromBody] LoginUsuarioViewModel loginUsuarioViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var senha = Seguranca.GeradorSenhaHash(loginUsuarioViewModel.Senha);

				var usuarioByLogin = await context.Usuarios
					.AsNoTracking()
					.Where(usuario => usuario.Email.ToLower().Equals(loginUsuarioViewModel.Email.ToLower())
						&& usuario.Senha.Equals(senha))
					.FirstOrDefaultAsync();

				return usuarioByLogin == null ? BadRequest("Usuário ou senha inválidos.") : Ok(Seguranca.GeradorToken(autenticacao, usuarioByLogin));
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}