using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobCoinAPI.Data;
using JobCoinAPI.Mappers;
using JobCoinAPI.Models;
using JobCoinAPI.Shared;
using JobCoinAPI.ViewModels.VagaViewModels;
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
	[Route("v1/vagas")]
	public class VagaController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync(
			[FromServices] DataContext context,
			[FromBody] CriacaoVagaViewModel vagaViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var idUsuario = Guid.Parse(User.Claims.FirstOrDefault(i => i.Type.Contains("nameidentifier")).Value);

				var usuarioById = idUsuario.Equals(Guid.Empty) ? null :
					await context.Usuarios
						.AsNoTracking()
						.Include(usuario => usuario.Perfil)
						.Include(usuario => usuario.VagasCriadas)
						.Include(usuario => usuario.VagasFavoritadas)
						.AsSplitQuery()
						.Where(usuario => usuario.IdUsuario.Equals(idUsuario))
						.FirstOrDefaultAsync();

				if (usuarioById == null)
					return BadRequest("Falha na identificação do usuário.");

				var data = DateTime.Now;

				var vaga = new Vaga
				{
					IdVaga = Guid.NewGuid(),
					NomeVaga = vagaViewModel.NomeVaga,
					DescricaoVaga = vagaViewModel.DescricaoVaga,
					ValorVaga = vagaViewModel.ValorVaga,
					IdUsuarioCriacaoVaga = idUsuario,
					DataCriacaoVaga = data,
					DataAtualizacaoVaga = data,
					Usuarios = new List<VagaFavoritadaUsuario>()
				};

				await context.Vagas.AddAsync(vaga);

				usuarioById.VagasCriadas.ToList().Add(vaga);

				await context.SaveChangesAsync();
				
				var retornoVagaViewModel = VagaMapper.ConverterParaConsultaGeralVagaViewModel(vaga);

				return Created($"v1/vagas/{retornoVagaViewModel.IdUsuarioCriacaoVaga}", retornoVagaViewModel);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPost]
		[Route("{idVaga}/favoritar")]
		public async Task<IActionResult> FavoritarVagaAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid idVaga)
		{
			try
			{
				if (idVaga.Equals(Guid.Empty))
					return BadRequest("O 'idVaga' informado se encontra vazio.");

				var idUsuario = Guid.Parse(User.Claims.FirstOrDefault(i => i.Type.Contains("nameidentifier")).Value);

				var usuarioById = idUsuario.Equals(Guid.Empty) ? null :
					await context.Usuarios
						.AsNoTracking()
						.Include(usuario => usuario.Perfil)
						.Include(usuario => usuario.VagasCriadas)
						.Include(usuario => usuario.VagasFavoritadas)
						.AsSplitQuery()
						.Where(usuario => usuario.IdUsuario.Equals(idUsuario))
						.FirstOrDefaultAsync();

				if (usuarioById == null)
					return BadRequest("Falha na identificação do usuário.");

				var vagaById = await context.Vagas
					.AsNoTracking()
					.Include(vaga => vaga.UsuarioCriacaoVaga)
					.Include(vaga => vaga.Usuarios)
					.Where(vaga => vaga.IdVaga.Equals(idVaga))
					.FirstOrDefaultAsync();

				if (vagaById == null)
					return BadRequest("Não existe vaga cadastrada com o 'idVaga' informado.");

				var vagaFavoritadaById = await context.VagasFavoritadas
					.AsNoTracking()
					.Where(vagaFavorita => vagaFavorita.IdUsuario.Equals(usuarioById.IdUsuario)
						&& vagaFavorita.IdVaga.Equals(idVaga))
					.FirstOrDefaultAsync();

				if (vagaFavoritadaById != null)
					return Ok();

				var novaVagaFavoritada = new VagaFavoritadaUsuario
				{
					IdUsuario = idUsuario,
					IdVaga = idVaga
				};

				await context.VagasFavoritadas.AddAsync(novaVagaFavoritada);

				usuarioById.VagasFavoritadas.ToList().Add(novaVagaFavoritada);
				vagaById.Usuarios.ToList().Add(novaVagaFavoritada);

				await context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync(
			[FromServices] DataContext context,
			[FromQuery] string nomeVaga,
			[FromQuery] string descricaoVaga,
			[FromQuery] float valorMenorQue,
			[FromQuery] float valorMaiorQue,
			[FromQuery] string ordenar,
			[FromQuery] int pagina = 1,
			[FromQuery] int numeroItens = 10)
		{
			try
			{
				var consultaVagas = context.Vagas
					.AsNoTracking();

				if (!string.IsNullOrEmpty(nomeVaga))
				{
					consultaVagas = consultaVagas
						.Where(vaga =>
							vaga.NomeVaga.ToLower().Contains(nomeVaga.ToLower()));
				}

				if (!string.IsNullOrEmpty(descricaoVaga))
				{
					consultaVagas = consultaVagas
						.Where(vaga =>
							vaga.DescricaoVaga.ToLower().Contains(descricaoVaga.ToLower()));
				}

				if (valorMenorQue > 0)
				{
					consultaVagas = consultaVagas
						.Where(vaga =>
							vaga.ValorVaga < valorMenorQue);
				}

				if (valorMaiorQue > 0)
				{
					consultaVagas = consultaVagas
						.Where(vaga =>
							vaga.ValorVaga > valorMaiorQue);
				}

				var camposOrdenacao = string.IsNullOrEmpty(ordenar) ? new List<string>(0) : ordenar.Split(",").ToList();

				foreach (var campo in camposOrdenacao)
				{
					switch (campo)
					{
						case "nomeVaga":
						case "+nomeVaga":
							consultaVagas = consultaVagas
								.OrderBy(vaga => vaga.NomeVaga);
							break;
						case "-nomeVaga":
							consultaVagas = consultaVagas
								.OrderByDescending(vaga => vaga.NomeVaga);
							break;

						case "descricaoVaga":
						case "+descricaoVaga":
							consultaVagas = consultaVagas
								.OrderBy(vaga => vaga.DescricaoVaga);
							break;
						case "-descricaoVaga":
							consultaVagas = consultaVagas
								.OrderByDescending(vaga => vaga.DescricaoVaga);
							break;

						case "valorVaga":
						case "+valorVaga":
							consultaVagas = consultaVagas
								.OrderBy(vaga => vaga.ValorVaga);
							break;
						case "-valorVaga":
							consultaVagas = consultaVagas
								.OrderByDescending(vaga => vaga.ValorVaga);
							break;

						default:
							break;
					}
				}

				int numeroTotalItens = await consultaVagas.CountAsync();

				var vagas = await Paginacao<Vaga>
					.PaginarConsulta(ref pagina, ref numeroItens, numeroTotalItens, consultaVagas).ToListAsync();

				var vagasViewModels = VagaMapper.ConverterParaConsultaGeralVagaViewModel(vagas);

				var retornoVagas = Paginacao<ConsultaGeralVagaViewModel>
					.PegarPaginacao(numeroTotalItens, pagina, vagasViewModels);

				return vagasViewModels == null ? NoContent() : Ok(retornoVagas);
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("criadas/{idUsuario}")]
		public async Task<IActionResult> GetAllCriadasAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid idUsuario,
			[FromQuery] string nomeVaga,
			[FromQuery] string descricaoVaga,
			[FromQuery] float valorMenorQue,
			[FromQuery] float valorMaiorQue,
			[FromQuery] string ordenar,
			[FromQuery] int pagina = 1,
			[FromQuery] int numeroItens = 10)
		{
			try
			{
				var consultaVagasCriadas = context.Vagas
					.AsNoTracking()
					.Where(vagaCriada => vagaCriada.IdUsuarioCriacaoVaga.Equals(idUsuario));

				if (!string.IsNullOrEmpty(nomeVaga))
				{
					consultaVagasCriadas = consultaVagasCriadas
						.Where(vagaCriada =>
							vagaCriada.NomeVaga.ToLower().Contains(nomeVaga.ToLower()));
				}

				if (!string.IsNullOrEmpty(descricaoVaga))
				{
					consultaVagasCriadas = consultaVagasCriadas
						.Where(vagaCriada =>
							vagaCriada.DescricaoVaga.ToLower().Contains(descricaoVaga.ToLower()));
				}

				if (valorMenorQue > 0)
				{
					consultaVagasCriadas = consultaVagasCriadas
						.Where(vagaCriada =>
							vagaCriada.ValorVaga < valorMenorQue);
				}

				if (valorMaiorQue > 0)
				{
					consultaVagasCriadas = consultaVagasCriadas
						.Where(vagaCriada =>
							vagaCriada.ValorVaga > valorMaiorQue);
				}

				var camposOrdenacao = string.IsNullOrEmpty(ordenar) ? new List<string>(0) : ordenar.Split(",").ToList();

				foreach (var campo in camposOrdenacao)
				{
					switch (campo)
					{
						case "nomeVaga":
						case "+nomeVaga":
							consultaVagasCriadas = consultaVagasCriadas
								.OrderBy(vagaCriada => vagaCriada.NomeVaga);
							break;
						case "-nomeVaga":
							consultaVagasCriadas = consultaVagasCriadas
								.OrderByDescending(vagaCriada => vagaCriada.NomeVaga);
							break;

						case "descricaoVaga":
						case "+descricaoVaga":
							consultaVagasCriadas = consultaVagasCriadas
								.OrderBy(vagaCriada => vagaCriada.DescricaoVaga);
							break;
						case "-descricaoVaga":
							consultaVagasCriadas = consultaVagasCriadas
								.OrderByDescending(vagaCriada => vagaCriada.DescricaoVaga);
							break;

						case "valorVaga":
						case "+valorVaga":
							consultaVagasCriadas = consultaVagasCriadas
								.OrderBy(vagaCriada => vagaCriada.ValorVaga);
							break;
						case "-valorVaga":
							consultaVagasCriadas = consultaVagasCriadas
								.OrderByDescending(vagaCriada => vagaCriada.ValorVaga);
							break;

						default:
							break;
					}
				}

				int numeroTotalItens = await consultaVagasCriadas.CountAsync();

				var vagasCriadas = await Paginacao<Vaga>
					.PaginarConsulta(ref pagina, ref numeroItens, numeroTotalItens, consultaVagasCriadas).ToListAsync();

				var vagasCriadasViewModels = VagaMapper.ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(vagasCriadas);

				var retornoVagasCriadas = Paginacao<ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel>
					.PegarPaginacao(numeroTotalItens, pagina, vagasCriadasViewModels);

				return vagasCriadasViewModels == null ? NoContent() : Ok(retornoVagasCriadas);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("favoritadas/{idUsuario}")]
		public async Task<IActionResult> GetAllFavoritadasAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid idUsuario,
			[FromQuery] string nomeVaga,
			[FromQuery] string descricaoVaga,
			[FromQuery] float valorMenorQue,
			[FromQuery] float valorMaiorQue,
			[FromQuery] string ordenar,
			[FromQuery] int pagina = 1,
			[FromQuery] int numeroItens = 10)
		{
			try
			{
				var consultaVagasFavoritadas = context.VagasFavoritadas
					.AsNoTracking()
					.Include(vagaFavoritada => vagaFavoritada.Usuario)
					.Include(vagaFavoritada => vagaFavoritada.Vaga)
					.Where(vagaFavoritada => vagaFavoritada.IdUsuario.Equals(idUsuario))
					.AsQueryable();

				if (!string.IsNullOrEmpty(nomeVaga))
				{
					consultaVagasFavoritadas = consultaVagasFavoritadas
						.Where(vagaFavoritada =>
							vagaFavoritada.Vaga.NomeVaga.ToLower().Contains(nomeVaga.ToLower()));
				}

				if (!string.IsNullOrEmpty(descricaoVaga))
				{
					consultaVagasFavoritadas = consultaVagasFavoritadas
						.Where(vagaFavoritada =>
							vagaFavoritada.Vaga.DescricaoVaga.ToLower().Contains(descricaoVaga.ToLower()));
				}

				if (valorMenorQue > 0)
				{
					consultaVagasFavoritadas = consultaVagasFavoritadas
						.Where(vagaFavoritada =>
							vagaFavoritada.Vaga.ValorVaga < valorMenorQue);
				}

				if (valorMaiorQue > 0)
				{
					consultaVagasFavoritadas = consultaVagasFavoritadas
						.Where(vagaFavoritada =>
							vagaFavoritada.Vaga.ValorVaga > valorMaiorQue);
				}

				var camposOrdenacao = string.IsNullOrEmpty(ordenar) ? new List<string>(0) : ordenar.Split(",").ToList();

				foreach (var campo in camposOrdenacao)
				{
					switch (campo)
					{
						case "nomeVaga":
						case "+nomeVaga":
							consultaVagasFavoritadas = consultaVagasFavoritadas
								.OrderBy(vagaFavoritada => vagaFavoritada.Vaga.NomeVaga);
							break;
						case "-nomeVaga":
							consultaVagasFavoritadas = consultaVagasFavoritadas
								.OrderByDescending(vagaFavoritada => vagaFavoritada.Vaga.NomeVaga);
							break;

						case "descricaoVaga":
						case "+descricaoVaga":
							consultaVagasFavoritadas = consultaVagasFavoritadas
								.OrderBy(vagaFavoritada => vagaFavoritada.Vaga.DescricaoVaga);
							break;
						case "-descricaoVaga":
							consultaVagasFavoritadas = consultaVagasFavoritadas
								.OrderByDescending(vagaFavoritada => vagaFavoritada.Vaga.DescricaoVaga);
							break;

						case "valorVaga":
						case "+valorVaga":
							consultaVagasFavoritadas = consultaVagasFavoritadas
								.OrderBy(vagaFavoritada => vagaFavoritada.Vaga.ValorVaga);
							break;
						case "-valorVaga":
							consultaVagasFavoritadas = consultaVagasFavoritadas
								.OrderByDescending(vagaFavoritada => vagaFavoritada.Vaga.ValorVaga);
							break;

						default:
							break;
					}
				}

				int numeroTotalItens = await consultaVagasFavoritadas.CountAsync();

				var vagasFavoritadas = await Paginacao<VagaFavoritadaUsuario>
					.PaginarConsulta(ref pagina, ref numeroItens, numeroTotalItens, consultaVagasFavoritadas).ToListAsync();

				var vagasFavoritadasViewModels = VagaMapper.ConverterParaConsultaGeralVagaCriadaOuFavoritadaVagaViewModel(vagasFavoritadas);

				var retornoVagasFavoritadas = Paginacao<ConsultaGeralVagaCriadaOuFavoritadaVagaViewModel>
					.PegarPaginacao(numeroTotalItens, pagina, vagasFavoritadasViewModels);

				return vagasFavoritadasViewModels == null ? NoContent() : Ok(retornoVagasFavoritadas);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid id)
		{
			try
			{
				var vagaById = await context.Vagas
					.AsNoTracking()
					.Include(vaga => vaga.UsuarioCriacaoVaga)
					.Include(vaga => vaga.Usuarios)
					.Where(vaga => vaga.IdVaga.Equals(id))
					.FirstOrDefaultAsync();

				var vagaViewModel = VagaMapper.ConverterParaConsultaUnicaVagaViewModel(vagaById);

				return vagaViewModel == null ? NoContent() : Ok(vagaViewModel);
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
			[FromBody] AlteracaoVagaViewModel vagaViewModel,
			[FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var idUsuario = Guid.Parse(User.Claims.FirstOrDefault(i => i.Type.Contains("nameidentifier")).Value);

				if (idUsuario.Equals(Guid.Empty))
					return BadRequest("Falha na identificação do usuário.");

				if (id.Equals(Guid.Empty))
					return BadRequest("O 'id' informado se encontra vazio.");

				var vagaById = await context.Vagas
					.AsNoTracking()
					.Include(vaga => vaga.UsuarioCriacaoVaga)
					.Include(vaga => vaga.Usuarios)
					.Where(vaga => vaga.IdVaga.Equals(id))
					.FirstOrDefaultAsync();

				if (vagaById == null)
					return BadRequest("Não existe vaga cadastrada com o 'id' informado.");

				vagaById.NomeVaga = vagaViewModel.NomeVaga;
				vagaById.DescricaoVaga = vagaViewModel.DescricaoVaga;
				vagaById.ValorVaga = vagaViewModel.ValorVaga;
				vagaById.DataAtualizacaoVaga = DateTime.Now;

				context.Vagas.Update(vagaById);
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

				var vagaById = await context.Vagas
					.AsNoTracking()
					.Where(vaga => vaga.IdVaga.Equals(id))
					.FirstOrDefaultAsync();

				if (vagaById == null)
					return BadRequest("Não existe vaga cadastrada com o 'id' informado.");

				context.Vagas.Remove(vagaById);
				await context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpDelete]
		[Route("{idVaga}/desfavoritar")]
		public async Task<IActionResult> DesfavoritarVagaAsync(
			[FromServices] DataContext context,
			[FromRoute] Guid idVaga)
		{
			try
			{
				if (idVaga.Equals(Guid.Empty))
					return BadRequest("O 'idVaga' informado se encontra vazio.");

				var idUsuario = Guid.Parse(User.Claims.FirstOrDefault(i => i.Type.Contains("nameidentifier")).Value);

				var usuarioById = idUsuario.Equals(Guid.Empty) ? null :
					await context.Usuarios
						.AsNoTracking()
						.Include(usuario => usuario.Perfil)
						.Include(usuario => usuario.VagasCriadas)
						.Include(usuario => usuario.VagasFavoritadas)
						.AsSplitQuery()
						.Where(usuario => usuario.IdUsuario.Equals(idUsuario))
						.FirstOrDefaultAsync();

				if (usuarioById == null)
					return BadRequest("Falha na identificação do usuário.");

				var vagaById = await context.Vagas
					.AsNoTracking()
					.Include(vaga => vaga.UsuarioCriacaoVaga)
					.Include(vaga => vaga.Usuarios)
					.Where(vaga => vaga.IdVaga.Equals(idVaga))
					.FirstOrDefaultAsync();

				if (vagaById == null)
					return BadRequest("Não existe vaga cadastrada com o 'idVaga' informado.");

				var vagaFavoritadaById = await context.VagasFavoritadas
					.AsNoTracking()
					.Where(vagaFavoritada => vagaFavoritada.IdUsuario.Equals(usuarioById.IdUsuario)
						&& vagaFavoritada.IdVaga.Equals(idVaga))
					.FirstOrDefaultAsync();

				if (vagaFavoritadaById == null)
					return Ok();

				usuarioById.VagasFavoritadas.ToList().Remove(vagaFavoritadaById);
				vagaById.Usuarios.ToList().Remove(vagaFavoritadaById);

				context.VagasFavoritadas.Remove(vagaFavoritadaById);
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