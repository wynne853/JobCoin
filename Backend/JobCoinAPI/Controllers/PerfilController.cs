using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using JobCoinAPI.Mappers;
using JobCoinAPI.Models;
using JobCoinAPI.Shared;
using JobCoinAPI.ViewModels.PerfilFuncionalidadeViewModels;
using JobCoinAPI.ViewModels.PerfilViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
	[Route("v1/perfis")]
	public class PerfilController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostPerfilAsync(
			[FromServices] DataContext context,
			[FromBody] CriacaoPerfilViewModel perfilViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();
			
			try
			{
				var perfilByName = await context.Perfis
					.AsNoTracking()
					.Where(perfil => perfil.NomePerfil.ToLower().Equals(perfilViewModel.NomePerfil.ToLower()))
					.FirstOrDefaultAsync();

				if (perfilByName != null)
					return BadRequest("Já existe perfil cadastrado com o 'nome' informado.");

				perfilByName = new Perfil { IdPerfil = Guid.NewGuid(), NomePerfil = perfilViewModel.NomePerfil };

				await context.Perfis.AddAsync(perfilByName);
				await context.SaveChangesAsync();

				var retornoPerfilViewModel = PerfilMapper.ConverterParaConsultaViewModel(perfilByName);

				return Created($"v1/perfis/{retornoPerfilViewModel.IdPerfil}", retornoPerfilViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPost]
		[Route("{idPerfil}/adicionar-funcionalidade/{idFuncionalidade}")]
		public async Task<IActionResult> AdicionarFuncionalidadeAoPerfilAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid idPerfil,
			[FromRoute] Guid idFuncionalidade)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				if (idPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				if (idFuncionalidade.Equals(Guid.Empty))
					return BadRequest("O 'idFuncionalidade' informado se encontra vazio.");

				var perfilById = await context.Perfis
					.AsNoTracking()
					.Include(perfil => perfil.Funcionalidades)
					.Where(perfil => perfil.IdPerfil.Equals(idPerfil))
					.FirstOrDefaultAsync();

				if (perfilById == null)
					return BadRequest("Não existe perfil cadastrado com o 'idPerfil' informado.");

				var funcionalidadeById = await context.Funcionalidades
					.AsNoTracking()
					.Where(funcionalidade => funcionalidade.IdFuncionalidade.Equals(idFuncionalidade))
					.FirstOrDefaultAsync();

				if (funcionalidadeById == null)
					return BadRequest("Não existe funcionalidade cadastrada com o 'idFuncionalidade' informado.");

				var perfilFuncionalidadeById = await context.PerfisFuncionalidades
					.AsNoTracking()
					.Where(perfilFuncionalidade => perfilFuncionalidade.IdPerfil.Equals(idPerfil)
						&& perfilFuncionalidade.IdFuncionalidade.Equals(idFuncionalidade))
					.FirstOrDefaultAsync();

				if (perfilFuncionalidadeById != null)
					return BadRequest("Esse perfil já contém essa funcionalidade.");

				var novoPerfilFuncionalidade = new PerfilFuncionalidade
				{
					IdPerfil = idPerfil,
					IdFuncionalidade = idFuncionalidade
				};

				await context.PerfisFuncionalidades.AddAsync(novoPerfilFuncionalidade);

				perfilById.Funcionalidades.ToList().Add(novoPerfilFuncionalidade);

				await context.SaveChangesAsync();

				novoPerfilFuncionalidade.Perfil = perfilById;
				novoPerfilFuncionalidade.Funcionalidade = funcionalidadeById;

				var retornoPerfilFuncionalidadeViewModel = PerfilFuncionalidadeMapper.ConverterParaConsultaViewModel(novoPerfilFuncionalidade);

				return Created($"v1/perfis/{retornoPerfilFuncionalidadeViewModel.IdPerfil}/funcionalidades/{retornoPerfilFuncionalidadeViewModel.IdFuncionalidade}", retornoPerfilFuncionalidadeViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllPerfisAsync(
			[FromServices] DataContext context,
			[FromQuery] string nome,
			[FromQuery] string ordenar,
			[FromQuery] int pagina = 1,
			[FromQuery] int numeroItens = 10)
		{
			try
			{
				var consultaPerfis = context.Perfis
					.AsNoTracking();

				if (!string.IsNullOrEmpty(nome))
				{
					consultaPerfis = consultaPerfis
						.Where(perfil => perfil.NomePerfil.ToLower().Contains(nome.ToLower()));
				}

				var camposOrdenacao = string.IsNullOrEmpty(ordenar) ? new List<string>(0) : ordenar.Split(",").ToList();

				foreach (var campo in camposOrdenacao)
				{
					switch (campo)
					{
						case "nomePerfil":
						case "+nomePerfil":
							consultaPerfis = consultaPerfis
								.OrderBy(funcionalidade => funcionalidade.NomePerfil);
							break;
						case "-nomePerfil":
							consultaPerfis = consultaPerfis
								.OrderByDescending(funcionalidade => funcionalidade.NomePerfil);
							break;

						default:
							break;
					}
				}

				int numeroTotalItens = await consultaPerfis.CountAsync();

				var perfis = await Paginacao<Perfil>
					.PaginarConsulta(ref pagina, ref numeroItens, numeroTotalItens, consultaPerfis).ToListAsync();

				var perfisViewModels = PerfilMapper.ConverterParaConsultaViewModel(perfis);

				var retornoPerfis = Paginacao<ConsultaPerfilViewModel>
					.PegarPaginacao(numeroTotalItens, pagina, perfisViewModels);

				return perfisViewModels == null ? NoContent() : Ok(retornoPerfis);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("{idPerfil}/funcionalidades")]
		public async Task<IActionResult> GetAllPerfisFuncionalidadesAsync(
			[FromServices] DataContext context,
			[FromRoute] int idPerfil,
			[FromQuery] string nomeFuncionalidade,
			[FromQuery] string ordenar,
			[FromQuery] int pagina = 1,
			[FromQuery] int numeroItens = 10)
		{
			try
			{
				var consultaPerfisFuncionalidades = context.PerfisFuncionalidades
					.AsNoTracking()
					.Include(perfilFuncionalidade => perfilFuncionalidade.Perfil)
					.Include(perfilFuncionalidade => perfilFuncionalidade.Funcionalidade)
					.Where(perfilFuncionalidade => perfilFuncionalidade.Perfil.IdPerfil.Equals(idPerfil))
					.AsQueryable();

				if (!string.IsNullOrEmpty(nomeFuncionalidade))
				{
					consultaPerfisFuncionalidades = consultaPerfisFuncionalidades
						.Where(perfilFuncionalidade =>
							perfilFuncionalidade.Funcionalidade.NomeFuncionalidade.ToLower().Contains(nomeFuncionalidade.ToLower()));
				}

				var camposOrdenacao = string.IsNullOrEmpty(ordenar) ? new List<string>(0) : ordenar.Split(",").ToList();

				foreach (var campo in camposOrdenacao)
				{
					switch (campo)
					{
						case "nomeFuncionalidade":
						case "+nomeFuncionalidade":
							consultaPerfisFuncionalidades = consultaPerfisFuncionalidades
								.OrderBy(funcionalidade => funcionalidade.Funcionalidade.NomeFuncionalidade);
							break;
						case "-nomeFuncionalidade":
							consultaPerfisFuncionalidades = consultaPerfisFuncionalidades
								.OrderByDescending(funcionalidade => funcionalidade.Funcionalidade.NomeFuncionalidade);
							break;

						default:
							break;
					}
				}

				int numeroTotalItens = await consultaPerfisFuncionalidades.CountAsync();

				var perfisFuncionalidades = await Paginacao<PerfilFuncionalidade>
					.PaginarConsulta(ref pagina, ref numeroItens, numeroTotalItens, consultaPerfisFuncionalidades).ToListAsync();

				var perfisFuncionalidadesViewModels = PerfilFuncionalidadeMapper.ConverterParaConsultaViewModel(perfisFuncionalidades);

				var retornoPerfisFuncionalidades = Paginacao<ConsultaPerfilFuncionalidadeViewModel>
					.PegarPaginacao(numeroTotalItens, pagina, perfisFuncionalidadesViewModels);

				return perfisFuncionalidadesViewModels == null ? NoContent() : Ok(retornoPerfisFuncionalidades);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetPerfilAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid id)
		{
			try
			{
				var perfilById = await context.Perfis
					.AsNoTracking()
					.Include(perfil => perfil.Funcionalidades)
					.Where(perfil => perfil.IdPerfil.Equals(id))
					.FirstOrDefaultAsync();

				var perfilViewModel = PerfilMapper.ConverterParaConsultaViewModel(perfilById);

				return perfilViewModel == null ? NoContent() : Ok(perfilViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("{idPerfil}/funcionalidades/{idFuncionalidade}")]
		public async Task<IActionResult> GetPerfilFuncionalidadeAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid idPerfil,
			[FromRoute] Guid idFuncionalidade)
		{
			try
			{
				var perfilFuncionalidadeById = await context.PerfisFuncionalidades
					.AsNoTracking()
					.Include(perfilFuncionalidade => perfilFuncionalidade.Perfil)
					.Include(perfilFuncionalidade => perfilFuncionalidade.Funcionalidade)
					.Where(perfilFuncionalidade => perfilFuncionalidade.IdPerfil.Equals(idPerfil)
						&& perfilFuncionalidade.IdFuncionalidade.Equals(idFuncionalidade))
					.FirstOrDefaultAsync();

				var perfilFuncionalidadeViewModel = PerfilFuncionalidadeMapper.ConverterParaConsultaViewModel(perfilFuncionalidadeById);

				return perfilFuncionalidadeViewModel == null ? NoContent() : Ok(perfilFuncionalidadeViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> UpdatePerfilAsync(
			[FromServices] DataContext context,
			[FromBody] AlteracaoPerfilViewModel perfilViewModel,
			[FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				if (id.Equals(Guid.Empty))
					return BadRequest("O 'id' informado se encontra vazio.");

				var perfilById = await context.Perfis
					.AsNoTracking()
					.Include(perfil => perfil.Funcionalidades)
					.Where(perfil => perfil.IdPerfil.Equals(id))
					.FirstOrDefaultAsync();

				if (perfilById == null)
					return BadRequest("Não existe perfil cadastrado com o 'id' informado.");

				var perfilByName = await context.Perfis
					.AsNoTracking()
					.Include(perfil => perfil.Funcionalidades)
					.Where(perfil => !perfil.IdPerfil.Equals(id)
						&& perfil.NomePerfil.ToLower().Equals(perfilViewModel.NomePerfil.ToLower()))
					.FirstOrDefaultAsync();

				if (perfilByName != null)
					return BadRequest("Já existe outro perfil cadastrado com o 'nome' informado.");

				var novoPerfil = new Perfil 
				{
					IdPerfil = id,
					NomePerfil = perfilViewModel.NomePerfil,
					Funcionalidades = perfilById.Funcionalidades
				};

				context.Perfis.Update(novoPerfil);
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
		public async Task<IActionResult> DeletePerfilAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid id)
		{
			try
			{
				if (id.Equals(Guid.Empty))
					return BadRequest("O 'id' informado se encontra vazio.");

				var perfilById = await context.Perfis
					.AsNoTracking()
					.Where(perfil => perfil.IdPerfil.Equals(id))
					.FirstOrDefaultAsync();

				if (perfilById == null)
					return BadRequest("Não existe perfil cadastrado com o 'id' informado.");

				context.Perfis.Remove(perfilById);
				await context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpDelete]
		[Route("{idPerfil}/funcionalidades/{idFuncionalidade}")]
		public async Task<IActionResult> DeletePerfilFuncionalidadeAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid idPerfil,
			[FromRoute] Guid idFuncionalidade)
		{
			try
			{
				if (idPerfil.Equals(Guid.Empty))
					return BadRequest("O 'idPerfil' informado se encontra vazio.");

				if (idFuncionalidade.Equals(Guid.Empty))
					return BadRequest("O 'idFuncionalidade' informado se encontra vazio.");

				var perfilFuncionalidadeById = await context.PerfisFuncionalidades
					.AsNoTracking()
					.Where(perfilFuncionalidade => perfilFuncionalidade.IdPerfil.Equals(idPerfil)
						&& perfilFuncionalidade.IdFuncionalidade.Equals(idFuncionalidade))
					.FirstOrDefaultAsync();

				if (perfilFuncionalidadeById == null)
					return BadRequest("Esse perfil não tem essa funcionalidade.");

				context.PerfisFuncionalidades.Remove(perfilFuncionalidadeById);
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