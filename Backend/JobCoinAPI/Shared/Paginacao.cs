using System;
using System.Collections.Generic;
using System.Linq;

namespace JobCoinAPI.Shared
{
	public class Paginacao<T>
	{
		public int NumeroPaginas { get; set; }
		
		public int Pagina { get; set; }

		public IEnumerable<T> ItensPagina { get; set; }

		public static IQueryable<T> PaginarConsulta(ref int pagina, ref int numeroItensPorPagina, int numeroTotalItens, IQueryable<T> consulta)
		{
			pagina = (pagina <= 0) ? 1 : pagina;
			numeroItensPorPagina = (numeroItensPorPagina <= 0) ? 10 : numeroItensPorPagina;

			if (numeroItensPorPagina > numeroTotalItens)
			{
				pagina = 1;
				numeroItensPorPagina = (numeroTotalItens > 0) ? numeroTotalItens : numeroItensPorPagina;
			}

			var numeroPaginas = CalcularNumeroPaginas(numeroTotalItens, numeroItensPorPagina);

			pagina = (pagina > numeroPaginas) ? numeroPaginas : pagina;

			var numeroItensParaPular = numeroItensPorPagina * (pagina - 1);

			consulta = consulta
				.Skip(numeroItensParaPular)
				.Take(numeroItensPorPagina);

			return consulta;
		}

		public static Paginacao<T> PegarPaginacao(int numeroTotalItens, int numeroPaginaAtual, IEnumerable<T> itensPaginaAtual)
		{
			return new Paginacao<T>
			{
				NumeroPaginas = (numeroTotalItens <= 0) ? 1 : CalcularNumeroPaginas(numeroTotalItens, itensPaginaAtual.Count()),
				Pagina = numeroPaginaAtual,
				ItensPagina = itensPaginaAtual
			};
		}

		public static int CalcularNumeroPaginas(int numeroTotalItens, int numeroItensPagina)
		{
			var qtdPaginas = decimal.Divide(numeroTotalItens, numeroItensPagina);
			var numeroPaginas = (numeroItensPagina > numeroTotalItens) ? 1 : (int)Math.Ceiling(qtdPaginas);

			return numeroPaginas;
		}
	}
}