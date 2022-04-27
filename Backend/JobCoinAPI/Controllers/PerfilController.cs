using System;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using JobCoinAPI.Mappers;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.PerfilViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobCoinAPI.Controllers
{
	[Route("v1/perfis")]
	[ApiController]
	public class PerfilController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromServices] DataContext context, [FromBody] CriacaoPerfilViewModel perfilViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();
			
			try
			{
				var perfil = await context.Perfis
				.AsNoTracking()
				.FirstOrDefaultAsync(perfil => perfil.NomePerfil.Equals(perfilViewModel.NomePerfil));

				if (perfil != null)
					return BadRequest("Já existe perfil cadastrado com o 'nome' informado.");

				perfil = new Perfil { IdPerfil = Guid.NewGuid(), NomePerfil = perfilViewModel.NomePerfil };

				await context.Perfis.AddAsync(perfil);
				await context.SaveChangesAsync();

				return Created($"v1/perfis/{perfil.IdPerfil}", perfil);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync([FromServices] DataContext context)
		{
			try
			{
				var perfis = await context.Perfis
				.AsNoTracking()
				.Include(perfil => perfil.Funcionalidades)
				.ToListAsync();

				var listaPerfilViewModel = (perfis == null || perfis.Count == 0) ? null : PerfilMapper.ConverterParaViewModel(perfis);

				return listaPerfilViewModel == null ? NoContent() : Ok(listaPerfilViewModel);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetByIdAsync([FromServices] DataContext context, [FromRoute] Guid idPerfil)
		{
			try
			{
				if (idPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				var perfil = await context.Perfis
					.AsNoTracking()
					.Include(perfil => perfil.Funcionalidades)
					.FirstOrDefaultAsync(perfil => perfil.IdPerfil.Equals(idPerfil));

				var perfilViewModel = perfil == null ? null : PerfilMapper.ConverterParaViewModel(perfil);

				return perfilViewModel == null ? NoContent() : Ok(perfilViewModel);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> UpdateAsync([FromServices] DataContext context, [FromBody] AlteracaoPerfilViewModel perfilViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				if (perfilViewModel.IdPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				var perfilById = await context.Perfis
					.AsNoTracking()
					.Include(perfil => perfil.Funcionalidades)
					.FirstOrDefaultAsync(perfil => perfil.IdPerfil.Equals(perfilViewModel.IdPerfil));

				if (perfilById == null)
					return BadRequest("Não existe perfil cadastrado com o 'perfilId' informado.");

				var perfilByName = await context.Perfis
					.AsNoTracking()
					.Include(perfil => perfil.Funcionalidades)
					.FirstOrDefaultAsync(perfil => !perfil.IdPerfil.Equals(perfilViewModel.IdPerfil)
					&& perfil.NomePerfil.Equals(perfilViewModel.NomePerfil));

				if (perfilByName != null)
					return BadRequest("Já existe outro perfil cadastrado com o 'nome' informado.");

				var novoPerfil = new Perfil 
				{
					IdPerfil = perfilViewModel.IdPerfil,
					NomePerfil = perfilViewModel.NomePerfil,
					Funcionalidades = perfilById.Funcionalidades
				};

				context.Perfis.Update(novoPerfil);
				await context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeleteAsync([FromServices] DataContext context, [FromRoute] Guid idPerfil)
		{
			try
			{
				if (idPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				var perfil = await context.Perfis
					.AsNoTracking()
					.FirstOrDefaultAsync(perfil => perfil.IdPerfil.Equals(idPerfil));

				if (perfil == null)
					return BadRequest("Não existe perfil cadastrado com o 'idPerfil' informado.");

				context.Perfis.Remove(perfil);
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