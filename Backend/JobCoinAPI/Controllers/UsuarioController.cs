using System;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobCoinAPI.Shared;
using JobCoinAPI.ViewModels.UsuarioViewModels;
using JobCoinAPI.Models;
using JobCoinAPI.Mappers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace JobCoinAPI.Controllers
{
	/// <response code="200">Ok</response>
	/// <response code="201">Created</response>
	/// <response code="204">No Content</response>
	/// <response code="400">Bad Request</response>
	/// <response code="401">Unauthorized</response>
	/// <response code="404">Not Found</response>
	/// <response code="500">Internal Server Error</response>
	[ApiController]
	[Authorize]
	[Route("v1/usuarios")]
	public class UsuarioController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync(
			[FromServices] DataContext context,
			[FromBody] CriacaoUsuarioViewModel usuarioViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var usuarioByName = await context.Usuarios
					.AsNoTracking()
					.Where(usuario => usuario.Email.ToLower().Equals(usuarioViewModel.Email.ToLower()))
					.FirstOrDefaultAsync();

				if (usuarioByName != null)
					return BadRequest("Já existe usuário cadastrado com o 'email' informado.");

				if (usuarioViewModel.IdPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				var perfilById = await context.Perfis
					.AsNoTracking()
					.Where(perfil => perfil.IdPerfil.Equals(usuarioViewModel.IdPerfil))
					.FirstOrDefaultAsync();

				if (perfilById == null)
					return BadRequest("Não existe perfil cadastrado com o 'idPerfil' informado.");

				usuarioByName = new Usuario
				{
					IdUsuario = Guid.NewGuid(),
					IdPerfil = usuarioViewModel.IdPerfil,
					Nome = usuarioViewModel.Nome,
					Email = usuarioViewModel.Email,
					Senha = Seguranca.GeradorSenhaHash(usuarioViewModel.Senha),
					VagasCriadas = new List<Vaga>(),
					VagasFavoritadas = new List<VagaFavoritadaUsuario>()
				};

				await context.Usuarios.AddAsync(usuarioByName);
				await context.SaveChangesAsync();

				usuarioByName.Perfil = perfilById;
				var retornoUsuarioViewModel = UsuarioMapper.ConverterParaConsultaGeralUsuarioViewModel(usuarioByName);

				return Created($"v1/usuarios/{usuarioByName.IdUsuario}", retornoUsuarioViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync(
			[FromServices] DataContext context,
			[FromQuery] string nomeUsuario,
			[FromQuery] string emailUsuario,
			[FromQuery] string nomePerfilUsuario,
			[FromQuery] string ordenar,
			[FromQuery] int pagina = 1,
			[FromQuery] int numeroItens = 10)
		{
			try
			{
				var consultaUsuarios = context.Usuarios
					.AsNoTracking()
					.Include(usuario => usuario.Perfil)
					.AsQueryable();

				if (!string.IsNullOrEmpty(nomeUsuario))
				{
					consultaUsuarios = consultaUsuarios
						.Where(usuario =>
							usuario.Nome.ToLower().Contains(nomeUsuario.ToLower()));
				}

				if (!string.IsNullOrEmpty(emailUsuario))
				{
					consultaUsuarios = consultaUsuarios
						.Where(usuario =>
							usuario.Email.ToLower().Contains(emailUsuario.ToLower()));
				}

				if (!string.IsNullOrEmpty(nomePerfilUsuario))
				{
					consultaUsuarios = consultaUsuarios
						.Where(usuario =>
							usuario.Perfil.NomePerfil.ToLower().Contains(nomePerfilUsuario.ToLower()));
				}

				var camposOrdenacao = string.IsNullOrEmpty(ordenar) ? new List<string>(0) : ordenar.Split(",").ToList();

				foreach (var campo in camposOrdenacao)
				{
					switch (campo)
					{
						case "nomeUsuario":
						case "+nomeUsuario":
							consultaUsuarios = consultaUsuarios
								.OrderBy(usuario => usuario.Nome);
							break;
						case "-nomeUsuario":
							consultaUsuarios = consultaUsuarios
								.OrderByDescending(usuario => usuario.Nome);
							break;

						case "emailUsuario":
						case "+emailUsuario":
							consultaUsuarios = consultaUsuarios
								.OrderBy(usuario => usuario.Email);
							break;
						case "-emailUsuario":
							consultaUsuarios = consultaUsuarios
								.OrderByDescending(usuario => usuario.Email);
							break;

						case "nomePerfilUsuario":
						case "+nomePerfilUsuario":
							consultaUsuarios = consultaUsuarios
								.OrderBy(usuario => usuario.Perfil.NomePerfil);
							break;
						case "-nomePerfilUsuario":
							consultaUsuarios = consultaUsuarios
								.OrderByDescending(usuario => usuario.Perfil.NomePerfil);
							break;
							
						default:
							break;
					}
				}

				int numeroTotalItens = await consultaUsuarios.CountAsync();

				var usuarios = await Paginacao<Usuario>
					.PaginarConsulta(ref pagina, ref numeroItens, numeroTotalItens, consultaUsuarios).ToListAsync();

				var usuariosViewModels = UsuarioMapper.ConverterParaConsultaGeralUsuarioViewModel(usuarios);

				var retornoUsuarios = Paginacao<ConsultaGeralUsuarioViewModel>
					.PegarPaginacao(numeroTotalItens, pagina, usuariosViewModels);

				return usuariosViewModels == null ? NoContent() : Ok(retornoUsuarios);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetByIdAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid id)
		{
			try
			{
				var usuarioById = await context.Usuarios
					.Include(usuario => usuario.Perfil)
					.Include(usuario => usuario.VagasCriadas)
					.Include(usuario => usuario.VagasFavoritadas)
					.ThenInclude(vagasFavoritada => vagasFavoritada.Vaga)
					.Include(usuario => usuario.VagasFavoritadas)
					.ThenInclude(vagasFavoritada => vagasFavoritada.Usuario)
					.AsSplitQuery()
					.Where(usuario => usuario.IdUsuario.Equals(id))
					.FirstOrDefaultAsync();

				var usuarioViewModel = UsuarioMapper.ConverterParaConsultaUnicaUsuarioViewModel(usuarioById);

				return usuarioViewModel == null ? NoContent() : Ok(usuarioViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> UpdateAsync(
			[FromServices] DataContext context,
			[FromBody] AlteracaoUsuarioViewModel usuarioViewModel,
			[FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				if (id.Equals(Guid.Empty))
					return BadRequest("O 'id' informado se encontra vazio.");

				if (usuarioViewModel.IdPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				var usuarioById = await context.Usuarios
					.AsNoTracking()
					.Include(usuario => usuario.Perfil)
					.Include(usuario => usuario.VagasCriadas)
					.Include(usuario => usuario.VagasFavoritadas)
					.AsSplitQuery()
					.Where(usuario => usuario.IdUsuario.Equals(id))
					.FirstOrDefaultAsync();

				if (usuarioById == null)
					return BadRequest("Não existe usuário cadastrado com o 'id' informado.");

				var usuarioByEmail = await context.Usuarios
					.AsNoTracking()
					.Where(usuario => !usuario.IdUsuario.Equals(id)
						&& usuario.Email.ToLower().Equals(usuarioViewModel.Email.ToLower()))
					.FirstOrDefaultAsync();

				if (usuarioByEmail != null)
					return BadRequest("Já existe outro usuário cadastrado com o 'email' informado.");

				var perfilById = await context.Perfis
					.AsNoTracking()
					.Where(perfil => perfil.IdPerfil.Equals(usuarioViewModel.IdPerfil))
					.FirstOrDefaultAsync();

				if (perfilById == null)
					return BadRequest("Não existe perfil cadastrado com o 'idPerfil' informado.");

				usuarioById.IdPerfil = usuarioViewModel.IdPerfil;
				usuarioById.Nome = usuarioViewModel.Nome;
				usuarioById.Email = usuarioViewModel.Email;
				usuarioById.Senha = Seguranca.GeradorSenhaHash(usuarioViewModel.Senha);

				context.Usuarios.Update(usuarioById);
				await context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeleteAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid id)
		{
			try
			{
				if (id.Equals(Guid.Empty))
					return BadRequest("O 'id' informado se encontra vazio.");

				var usuarioById = await context.Usuarios
					.AsNoTracking()
					.Where(usuario => usuario.IdUsuario.Equals(id))
					.FirstOrDefaultAsync();

				if (usuarioById == null)
					return BadRequest("Não existe usuário cadastrado com o 'id' informado.");

				context.Usuarios.Remove(usuarioById);
				await context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}