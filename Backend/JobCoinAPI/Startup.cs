using System;
using JobCoinAPI.Data;
using JobCoinAPI.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JobCoinAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{																		
			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.EnableAnnotations();

				c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobCoinAPI", Version = "v1" });

				AddSwaggerConfigurationJwtBearer(c);

				AddSwaggerResponsesDocumentations(c);
			});

			AddDatabaseConfiguration(services);

			AddJwtBearerAuthentication(services);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobCoinAPI v1"));
			}

			app.UseCors(builder => builder
			 .AllowAnyOrigin()
			 .AllowAnyMethod()
			 .AllowAnyHeader());

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			context.Database.Migrate();
		}

		private void AddDatabaseConfiguration(IServiceCollection services)
		{
			services.AddDbContext<DataContext>(options =>
			{
				options.UseNpgsql(Configuration.GetConnectionString("PostgreDB"));
			});
		}

		private void AddJwtBearerAuthentication(IServiceCollection services)
		{
			Autenticacao authentication = new Autenticacao();

			services.AddSingleton(authentication);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = authentication.Key,
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}

		private void AddSwaggerConfigurationJwtBearer(SwaggerGenOptions swaggerGenOptions)
		{
			var openApiSecurityScheme = new OpenApiSecurityScheme
			{
				Name = "Authorization",
				BearerFormat = "JWT",
				Scheme = "bearer",
				Description = "Cole o seu 'token' aqui abaixo",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http
			};

			swaggerGenOptions.AddSecurityDefinition("Bearer", openApiSecurityScheme);

			var security = new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[]
					{

					}
				}
			};

			swaggerGenOptions.AddSecurityRequirement(security);
		}

		private void AddSwaggerResponsesDocumentations(SwaggerGenOptions swaggerGenOptions)
		{
			var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
			swaggerGenOptions.IncludeXmlComments(xmlPath);
		}
	}
}