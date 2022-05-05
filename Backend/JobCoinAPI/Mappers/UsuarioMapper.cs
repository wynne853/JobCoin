using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.UsuarioViewModels;

namespace JobCoinAPI.Mappers
{
	public class UsuarioMapper
	{
		public static ConsultaGeralUsuarioViewModel ConverterParaConsultaGeralUsuarioViewModel(Usuario usuario)
		{
			return usuario == null ? null : new ConsultaGeralUsuarioViewModel
			{
				IdUsuario = usuario.IdUsuario,
				Nome = usuario.Nome,
				Email = usuario.Email,
				Perfil = PerfilMapper.ConverterParaConsultaViewModel(usuario.Perfil)
			};
		}

		public static ConsultaUnicaUsuarioViewModel ConverterParaConsultaUnicaUsuarioViewModel(Usuario usuario)
		{
			return usuario == null ? null : new ConsultaUnicaUsuarioViewModel
			{
				IdUsuario = usuario.IdUsuario,
				Nome = usuario.Nome,
				Email = usuario.Email,
				Perfil = PerfilMapper.ConverterParaConsultaViewModel(usuario.Perfil),
				VagasCriadas = VagaMapper.ConverterParaConsultaGeralVagaViewModel(usuario.VagasCriadas),
				VagasFavoritadas = VagaMapper.ConverterParaConsultaGeralVagaViewModel(usuario.VagasFavoritadas),
			};
		}

		public static IEnumerable<ConsultaGeralUsuarioViewModel> ConverterParaConsultaGeralUsuarioViewModel(IEnumerable<Usuario> usuarios)
		{
			if (usuarios == null || usuarios.Count() == 0)
				return null;

			return usuarios.Select(usuario => ConverterParaConsultaGeralUsuarioViewModel(usuario)).ToList();
		}

		public static IEnumerable<ConsultaUnicaUsuarioViewModel> ConverterParaConsultaUnicaUsuarioViewModel(IEnumerable<Usuario> usuarios)
		{
			if (usuarios == null || usuarios.Count() == 0)
				return null;

			return usuarios.Select(usuario => ConverterParaConsultaUnicaUsuarioViewModel(usuario)).ToList();
		}
	}
}