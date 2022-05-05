using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace JobCoinAPI.Shared
{
	public class Autenticacao
	{
        public SecurityKey Key { get; }

        public SigningCredentials SigningCredentials { get; }

        public Autenticacao()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}