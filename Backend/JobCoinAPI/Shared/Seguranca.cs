using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobCoinAPI.Models;
using JobCoinAPI.ViewModels.LoginViewModels;
using Microsoft.IdentityModel.Tokens;

namespace JobCoinAPI.Shared
{
	public class Seguranca
	{
        public static string GeradorSenhaHash(string senha)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding encode = Encoding.UTF8;
                byte[] resultado = hash.ComputeHash(encode.GetBytes(senha));

                foreach (byte b in resultado)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }

        public static TokenViewModel GeradorToken(Autenticacao autenticacao, Usuario usuario)
        {
            DateTime creationDate = DateTime.Now;
            DateTime expirationDate = creationDate + TimeSpan.FromHours(2);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                }),
                NotBefore = creationDate.ToUniversalTime(),
                Expires = expirationDate.ToUniversalTime(),
                SigningCredentials = autenticacao.SigningCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenViewModel = new TokenViewModel
            {
                Autenticado = true,
                DataCriacao = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                DataExpiracao = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Token = tokenHandler.WriteToken(token)
            };

            return tokenViewModel;
        }
    }
}