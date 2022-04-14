using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using JobCoinAPI.Shared;
using JobCoinAPI.ViewModels.UsuarioViewModel;
using JobCoinAPI.Models;

namespace JobCoinAPI.Controllers
{
	[Route("v1")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{

		[HttpPost("usuario/create")]
		public async Task<IActionResult> Create([FromServices] DataContext context, [FromBody] CriacaoUsuarioViewModel criacaoUsuario)
		{
			var usuario = await context.Usuarios
				.AsNoTracking()
				.FirstOrDefaultAsync(usuario => (usuario.IdUsuario == criacaoUsuario.IdUsuario) || (usuario.Email == criacaoUsuario.Email));

			if (usuario != null || !ModelState.IsValid)
				return BadRequest();

			var perfil = await context.Perfis
				.AsNoTracking()
				.FirstOrDefaultAsync(perfil => perfil.IdPerfil == criacaoUsuario.IdPerfil);

			if (perfil == null)
				return BadRequest();

			var usuarioCreate = new Usuario
			{
				IdUsuario = criacaoUsuario.IdUsuario,
				IdPerfil = criacaoUsuario.IdPerfil,
				Perfil = perfil,
				Nome = criacaoUsuario.Nome,
				Email = criacaoUsuario.Email,
				Senha = Seguranca.GenerateHashPassword(criacaoUsuario.Senha)
			};

			try
			{
				await context.Usuarios.AddAsync(usuarioCreate);
				await context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return BadRequest();
			}

			return Ok(usuario);
		}

		[HttpGet("usuario/{id}")]
		public async Task<IActionResult> Get([FromServices] DataContext context, [FromRoute] Guid id)
		{
			var usuario = await context.Usuarios
				.AsNoTracking()
				.FirstOrDefaultAsync(usuario => usuario.IdUsuario == id);

			if (usuario == null)
				return NotFound();

			Mapper mapper = new Mapper();

			var perfil = await context.Perfis
				.AsNoTracking()
				.FirstOrDefaultAsync(perfil => perfil.IdPerfil == usuario.IdPerfil);

			if (perfil == null)
				return BadRequest();

			usuario.Perfil = perfil;
			var usuarioViewModel = mapper.ConverterParaConsultaUsuarioViewModel(usuario);

			return usuario == null ? NotFound() : Ok(usuarioViewModel);
		}

		[HttpGet]
		[Route("usuarios")]
		public async Task<IActionResult> GetAll([FromServices] DataContext context)
		{
			var usuarios = await context.Usuarios
				.AsNoTracking()
				.ToListAsync();

			return Ok(usuarios);
		}

	}
}