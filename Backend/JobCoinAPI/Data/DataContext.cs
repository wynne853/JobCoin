using JobCoinAPI.Maps;
using JobCoinAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobCoinAPI.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Funcionalidade> Funcionalidades { get; set; }
		
		public DbSet<Perfil> Perfis { get; set; }
		
		public DbSet<PerfilFuncionalidade> PerfisFuncionalidades { get; set; }
		
		public DbSet<Usuario> Usuarios { get; set; }

		public DbSet<VagaFavoritadaUsuario> VagasFavoritadas { get; set; }

		public DbSet<Vaga> Vagas { get; set; }

		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Funcionalidade>().Map();
			modelBuilder.Entity<Perfil>().Map();
			modelBuilder.Entity<PerfilFuncionalidade>().Map();
			modelBuilder.Entity<Usuario>().Map();
			modelBuilder.Entity<VagaFavoritadaUsuario>().Map();
			modelBuilder.Entity<Vaga>().Map();

			base.OnModelCreating(modelBuilder);
		}
	}
}