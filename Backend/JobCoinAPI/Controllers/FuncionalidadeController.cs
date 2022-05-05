using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using JobCoinAPI.Mappers;
using JobCoinAPI.Models;
using JobCoinAPI.Shared;
using JobCoinAPI.ViewModels.FuncionalidadeViewModels;
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
	[Route("v1/funcionalidades")]
	public class FuncionalidadeController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync(
			[FromServices] DataContext context,
			[FromBody] CriacaoFuncionalidadeViewModel funcionalidadeViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var funcionalidadeByName = await context.Funcionalidades
					.AsNoTracking()
					.Where(funcionalidade => funcionalidade.NomeFuncionalidade.ToLower().Equals(funcionalidadeViewModel.NomeFuncionalidade.ToLower()))
					.FirstOrDefaultAsync();

				if (funcionalidadeByName != null)
					return BadRequest("Já existe funcionalidade cadastrada com o 'nome' informado.");

				funcionalidadeByName = new Funcionalidade { IdFuncionalidade = Guid.NewGuid(), NomeFuncionalidade = funcionalidadeViewModel.NomeFuncionalidade };

				await context.Funcionalidades.AddAsync(funcionalidadeByName);
				await context.SaveChangesAsync();

				var retornoFuncionalidadeViewModel = FuncionalidadeMapper.ConverterParaConsultaViewModel(funcionalidadeByName);

				return Created($"v1/funcionalidades/{retornoFuncionalidadeViewModel.IdFuncionalidade}", retornoFuncionalidadeViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync(
			[FromServices] DataContext context,
			[FromQuery] string nome,
			[FromQuery] string ordenar,
			[FromQuery] int pagina = 1,
			[FromQuery] int numeroItens = 10)
		{
			try
			{
				var consultaFuncionalidades = context.Funcionalidades
					.AsNoTracking();

				if (!string.IsNullOrEmpty(nome))
				{
					consultaFuncionalidades = consultaFuncionalidades
						.Where(funcionalidade =>
							funcionalidade.NomeFuncionalidade.ToLower().Contains(nome.ToLower()));
				}

				var camposOrdenacao = string.IsNullOrEmpty(ordenar) ? new List<string>(0) : ordenar.Split(",").ToList();

				foreach (var campo in camposOrdenacao)
				{
					switch (campo)
					{
						case "nomeFuncionalidade":
						case "+nomeFuncionalidade":
							consultaFuncionalidades = consultaFuncionalidades
								.OrderBy(funcionalidade => funcionalidade.NomeFuncionalidade);
							break;
						case "-nomeFuncionalidade":
							consultaFuncionalidades = consultaFuncionalidades
								.OrderByDescending(funcionalidade => funcionalidade.NomeFuncionalidade);
							break;

						default:
							break;
					}
				}

				int numeroTotalItens = await consultaFuncionalidades.CountAsync();

				var funcionalidades = await Paginacao<Funcionalidade>
					.PaginarConsulta(ref pagina, ref numeroItens, numeroTotalItens, consultaFuncionalidades).ToListAsync();

				var funcionalidadesViewModels = FuncionalidadeMapper.ConverterParaConsultaViewModel(funcionalidades);

				var retornoFuncionalidades = Paginacao<ConsultaFuncionalidadeViewModel>
					.PegarPaginacao(numeroTotalItens, pagina, funcionalidadesViewModels);

				return funcionalidadesViewModels == null ? NoContent() : Ok(retornoFuncionalidades);
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
				var funcionalidadeById = await context.Funcionalidades
					.AsNoTracking()
					.Where(funcionalidade => funcionalidade.IdFuncionalidade.Equals(id))
					.FirstOrDefaultAsync();

				var funcionalidadeViewModel = FuncionalidadeMapper.ConverterParaConsultaViewModel(funcionalidadeById);

				return funcionalidadeViewModel == null ? NoContent() : Ok(funcionalidadeViewModel);
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
			[FromBody] AlteracaoFuncionalidadeViewModel funcionalidadeViewModel,
			[FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				if (id.Equals(Guid.Empty))
					return BadRequest("O 'id' informado se encontra vazio.");

				var funcionalidadeById = await context.Funcionalidades
					.AsNoTracking()
					.Where(funcionalidade => funcionalidade.IdFuncionalidade.Equals(id))
					.FirstOrDefaultAsync();

				if (funcionalidadeById == null)
					return BadRequest("Não existe funcionalidade cadastrada com o 'id' informado.");

				var funcionalidadeByName = await context.Funcionalidades
					.AsNoTracking()
					.Where(funcionalidade => !funcionalidade.IdFuncionalidade.Equals(id)
						&& funcionalidade.NomeFuncionalidade.ToLower().Equals(funcionalidadeViewModel.NomeFuncionalidade.ToLower()))
					.FirstOrDefaultAsync();

				if (funcionalidadeByName != null)
					return BadRequest("Já existe outra funcionalidade cadastrada com o 'nome' informado.");

				var novaFuncionalidade = new Funcionalidade
				{
					IdFuncionalidade = id,
					NomeFuncionalidade = funcionalidadeViewModel.NomeFuncionalidade
				};

				context.Funcionalidades.Update(novaFuncionalidade);
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

				var funcionalidadeById = await context.Funcionalidades
					.AsNoTracking()
					.Where(funcionalidade => funcionalidade.IdFuncionalidade.Equals(id))
					.FirstOrDefaultAsync();

				if (funcionalidadeById == null)
					return BadRequest("Não existe funcionalidade cadastrada com o 'id' informado.");

				context.Funcionalidades.Remove(funcionalidadeById);
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