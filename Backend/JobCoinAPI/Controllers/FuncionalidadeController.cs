using System;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using JobCoinAPI.Mappers;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.FuncionalidadeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobCoinAPI.Controllers
{
	[Route("v1/funcionalidades")]
	[ApiController]
	public class FuncionalidadeController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromServices] DataContext context, [FromBody] CriacaoFuncionalidadeViewModel funcionalidadeViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var funcionalidade = await context.Funcionalidades
				.AsNoTracking()
				.FirstOrDefaultAsync(funcionalidade => funcionalidade.NomeFuncionalidade.Equals(funcionalidadeViewModel.NomeFuncionalidade));

				if (funcionalidade != null)
					return BadRequest("Já existe funcionalidade cadastrada com o 'nome' informado.");

				funcionalidade = new Funcionalidade { IdFuncionalidade = Guid.NewGuid(), NomeFuncionalidade = funcionalidadeViewModel.NomeFuncionalidade };

				await context.Funcionalidades.AddAsync(funcionalidade);
				await context.SaveChangesAsync();

				return Created($"v1/funcionalidades/{funcionalidade.IdFuncionalidade}", funcionalidade);
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
				var funcionalidades = await context.Funcionalidades
				.AsNoTracking()
				.ToListAsync();

				var listaFuncionalidadeViewModel = (funcionalidades == null || funcionalidades.Count == 0) ? null : FuncionalidadeMapper.ConverterParaViewModel(funcionalidades);

				return listaFuncionalidadeViewModel == null ? NoContent() : Ok(listaFuncionalidadeViewModel);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetByIdAsync([FromServices] DataContext context, [FromRoute] Guid idFuncionalidade)
		{
			try
			{
				if (idFuncionalidade.Equals(Guid.Empty))
					return BadRequest("O 'idFuncionalidade' informado se encontra vazio.");

				var funcionalidade = await context.Funcionalidades
					.AsNoTracking()
					.FirstOrDefaultAsync(funcionalidade => funcionalidade.IdFuncionalidade.Equals(idFuncionalidade));

				var funcionalidadeViewModel = funcionalidade == null ? null : FuncionalidadeMapper.ConverterParaViewModel(funcionalidade);

				return funcionalidadeViewModel == null ? NoContent() : Ok(funcionalidadeViewModel);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> UpdateAsync([FromServices] DataContext context, [FromBody] AlteracaoFuncionalidadeViewModel funcionalidadeViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				if (funcionalidadeViewModel.IdFuncionalidade.Equals(Guid.Empty))
					return BadRequest("O 'idFuncionalidade' informado se encontra vazio.");

				var funcionalidadeById = await context.Funcionalidades
					.AsNoTracking()
					.FirstOrDefaultAsync(funcionalidade => funcionalidade.IdFuncionalidade.Equals(funcionalidadeViewModel.IdFuncionalidade));

				if (funcionalidadeById == null)
					return BadRequest("Não existe funcionalidade cadastrada com o 'idFuncionalidade' informado.");

				var funcionalidadeByName = await context.Funcionalidades
					.AsNoTracking()
					.FirstOrDefaultAsync(funcionalidade => !funcionalidade.IdFuncionalidade.Equals(funcionalidadeViewModel.IdFuncionalidade)
					&& funcionalidade.NomeFuncionalidade.Equals(funcionalidadeViewModel.NomeFuncionalidade));

				if (funcionalidadeByName != null)
					return BadRequest("Já existe outra funcionalidade cadastrada com o 'nome' informado.");

				var novaFuncionalidade = new Funcionalidade
				{
					IdFuncionalidade = funcionalidadeViewModel.IdFuncionalidade,
					NomeFuncionalidade = funcionalidadeViewModel.NomeFuncionalidade
				};

				context.Funcionalidades.Update(novaFuncionalidade);
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
		public async Task<IActionResult> DeleteAsync([FromServices] DataContext context, [FromRoute] Guid idFuncionalidade)
		{
			try
			{
				if (idFuncionalidade.Equals(Guid.Empty))
					return BadRequest("O 'idFuncionalidade' informado se encontra vazio.");

				var funcionalidade = await context.Funcionalidades
					.AsNoTracking()
					.FirstOrDefaultAsync(funcionalidade => funcionalidade.IdFuncionalidade.Equals(idFuncionalidade));

				if (funcionalidade == null)
					return BadRequest("Não existe funcionalidade cadastrada com o 'idFuncionalidade' informado.");

				context.Funcionalidades.Remove(funcionalidade);
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
