using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.UsuarioViewModels;

namespace JobCoinAPI.Mappers
{
	public class UsuarioMapper
	{
		public static ConsultaUsuarioViewModel ConverterParaConsultaUsuarioViewModel(Usuario usuario)
		{
			return new ConsultaUsuarioViewModel
			{
				Id = usuario.IdUsuario,
				Perfil = PerfilMapper.ConverterParaViewModel(usuario.Perfil),
				Nome = usuario.Nome,
				Email = usuario.Email,
				VagasCriadas = VagaMapper.ConverterParaViewModel(usuario.VagasCriadas),
				VagasFavoritadas = VagaMapper.ConverterParaViewModel(usuario.VagasFavoritadas),
			};
		}

		public static RetornoUsuarioViewModel ConverterParaRetornoUsuarioViewModel(Usuario usuario)
		{
			return new RetornoUsuarioViewModel
			{
				Id = usuario.IdUsuario,
				IdPerfil = usuario.IdPerfil,
				Nome = usuario.Nome,
				Email = usuario.Email,
			};
		}

		public static IEnumerable<ConsultaUsuarioViewModel> ConverterParaConsultaUsuarioViewModel(IEnumerable<Usuario> usuarios)
		{
			return usuarios?.Select(usuario => ConverterParaConsultaUsuarioViewModel(usuario)).ToList();
		}

		public static IEnumerable<RetornoUsuarioViewModel> ConverterParaRetornoUsuarioViewModel(IEnumerable<Usuario> usuarios)
		{
			return usuarios?.Select(usuario => ConverterParaRetornoUsuarioViewModel(usuario)).ToList();
		}
	}
}