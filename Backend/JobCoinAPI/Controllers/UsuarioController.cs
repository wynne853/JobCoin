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

namespace JobCoinAPI.Controllers
{
	[Route("v1/usuarios")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromServices] DataContext context, [FromBody] CriacaoUsuarioViewModel usuarioViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var usuario = await context.Usuarios
				.AsNoTracking()
				.FirstOrDefaultAsync(usuario => usuario.Email.Equals(usuarioViewModel.Email));

				if (usuario != null)
					return BadRequest("Já existe usuário cadastrado com o 'email' informado.");

				if (usuarioViewModel.IdPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				var perfil = await context.Perfis
					.AsNoTracking()
					.FirstOrDefaultAsync(perfil => perfil.IdPerfil.Equals(usuarioViewModel.IdPerfil));

				if (perfil == null)
					return BadRequest("Não existe perfil cadastrado com o 'idPerfil' informado.");

				usuario = new Usuario
				{
					IdUsuario = Guid.NewGuid(),
					IdPerfil = usuarioViewModel.IdPerfil,
					Nome = usuarioViewModel.Nome,
					Email = usuarioViewModel.Email,
					Senha = Seguranca.GenerateHashPassword(usuarioViewModel.Senha),
					VagasCriadas = new List<Vaga>(),
					VagasFavoritadas = new List<Vaga>()
				};

				var retornoUsuarioViewModel = UsuarioMapper.ConverterParaViewModel(usuario);

				await context.Usuarios.AddAsync(usuario);
				await context.SaveChangesAsync();

				return Created($"v1/usuarios/{usuario.IdUsuario}", retornoUsuarioViewModel);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync([FromServices] DataContext context)
		{
			var usuarios = await context.Usuarios
				.AsNoTracking()
				.Include(usuario => usuario.Perfil)
				.ToListAsync();

			var listaUsuarioViewModel = (usuarios == null || usuarios.Count == 0) ? null : UsuarioMapper.ConverterParaViewModel(usuarios);

			return listaUsuarioViewModel == null ? NoContent() : Ok(listaUsuarioViewModel);
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetByIdAsync([FromServices] DataContext context, [FromRoute] Guid idUsuario)
		{
			try
			{
				if (idUsuario.Equals(Guid.Empty))
					return BadRequest("O 'idUsuario' informado se encontra vazio.");

				var usuario = await context.Usuarios
				.AsNoTracking()
				.Include(usuario => usuario.Perfil)
				.Include(usuario => usuario.VagasCriadas)
				.Include(usuario => usuario.VagasFavoritadas)
				.FirstOrDefaultAsync(usuario => usuario.IdUsuario.Equals(idUsuario));

				var usuarioViewModel = usuario == null ? null : UsuarioMapper.ConverterParaViewModel(usuario);

				return usuarioViewModel == null ? NoContent() : Ok(usuarioViewModel);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> UpdateAsync([FromServices] DataContext context, [FromBody] AlteracaoUsuarioViewModel usuarioViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				if (usuarioViewModel.IdUsuario.Equals(Guid.Empty))
					return BadRequest("O 'idUsuario' informado se encontra vazio.");

				var usuarioById = await context.Usuarios
					.AsNoTracking()
					.FirstOrDefaultAsync(usuario => usuario.IdUsuario.Equals(usuarioViewModel.IdUsuario));

				if (usuarioById == null)
					return BadRequest("Não existe usuário cadastrado com o 'idUsuario' informado.");

				var usuarioByEmail = await context.Usuarios
					.AsNoTracking()
					.Include(usuario => usuario.Perfil)
					.Include(usuario => usuario.VagasCriadas)
					.Include(usuario => usuario.VagasFavoritadas)
					.FirstOrDefaultAsync(usuario => !usuario.IdUsuario.Equals(usuarioViewModel.IdUsuario)
					&& usuario.Email.Equals(usuarioViewModel.Email));

				if (usuarioByEmail != null)
					return BadRequest("Já existe outro usuário cadastrado com o 'email' informado.");

				var novoUsuario = new Usuario
				{
					IdUsuario = usuarioViewModel.IdUsuario,
					IdPerfil = usuarioViewModel.IdPerfil,
					Nome = usuarioViewModel.Nome,
					Email = usuarioViewModel.Email,
					Senha = Seguranca.GenerateHashPassword(usuarioViewModel.Senha),
					VagasCriadas = usuarioById.VagasCriadas,
					VagasFavoritadas = usuarioById.VagasFavoritadas
				};

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeleteAsync([FromServices] DataContext context, [FromRoute] Guid idUsuario)
		{
			try
			{
				if (idUsuario.Equals(Guid.Empty))
					return BadRequest("O 'idUsuario' informado se encontra vazio.");

				var usuario = await context.Usuarios
					.AsNoTracking()
					.FirstOrDefaultAsync(usuario => usuario.IdUsuario.Equals(idUsuario));

				if (usuario == null)
					return BadRequest("Não existe usuário cadastrado com o 'idUsuario' informado.");

				context.Usuarios.Remove(usuario);
				await context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}
	}
}