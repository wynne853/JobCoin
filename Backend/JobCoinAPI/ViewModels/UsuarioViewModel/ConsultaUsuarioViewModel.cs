using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobCoinAPI.ViewModels.UsuarioViewModel
{
	public class ConsultaUsuarioViewModel
	{
		public Guid Id { get; set; }
		public string Perfil { get; set; }
		public string Nome { get; set; }
		public string Email { get; set; }
	}
}