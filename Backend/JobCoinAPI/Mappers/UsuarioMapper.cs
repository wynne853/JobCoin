using System.Collections.Generic;
using System.Linq;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.UsuarioViewModels;

namespace JobCoinAPI.Mappers
{
	public class UsuarioMapper
	{
		public static ConsultaUsuarioViewModel ConverterParaViewModel(Usuario usuario)
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

		public static ICollection<ConsultaUsuarioViewModel> ConverterParaViewModel(ICollection<Usuario> usuarios)
		{
			return usuarios?.Select(usuario => ConverterParaViewModel(usuario)).ToList();
		}
	}
}