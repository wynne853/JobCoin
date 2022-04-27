using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.FuncionalidadeViewModel;
using JobCoinAPI.ViewModels.PerfilViewModel;
using JobCoinAPI.ViewModels.UsuarioViewModel;

namespace JobCoinAPI.Shared
{
	public class Mapper
	{
		public AlteracaoUsuarioViewModel ConverterParaAlteracaoUsuarioViewModel(Usuario usuario)
		{
			return new AlteracaoUsuarioViewModel
			{
				IdPerfil = usuario.IdPerfil,
				Nome = usuario.Nome,
				Senha = usuario.Email
			};
		}

		public ConsultaUsuarioViewModel ConverterParaConsultaUsuarioViewModel(Usuario usuario)
		{
			return new ConsultaUsuarioViewModel
			{
				Id = usuario.IdUsuario,
				Perfil = usuario.Perfil.NomePerfil,
				Nome = usuario.Nome,
				Email = usuario.Email
			};
		}

		public CriacaoUsuarioViewModel ConverterParaCriacaoUsuarioViewModel(Usuario usuario)
		{
			return new CriacaoUsuarioViewModel
			{
				Nome = usuario.Nome,
				Email = usuario.Email,
				Senha = usuario.Senha,
				IdPerfil = usuario.IdPerfil
			};
		}
	}
}