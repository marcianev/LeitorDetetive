using ApiBackend.Configurations;
using ApiBackend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiBackend.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IConfiguration configuration)
        {
            _jwtSettings = configuration
                .GetSection("Jwt")
                .Get<JwtSettings>()!;
        }

        public string GerarToken(Usuario usuario)
        {
            var tokeHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var claims = new List<Claim>
            {
                new("UsuarioId", usuario.Id.ToString()),
                new("User", usuario.User),
                new("Tipo", usuario.Tipo)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(
                    _jwtSettings.ExpirationMinutes),

                Issuer = _jwtSettings.Issuer,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokeHandler.CreateToken(tokenDescriptor);

            return tokeHandler.WriteToken(token);
        }
    }
}
