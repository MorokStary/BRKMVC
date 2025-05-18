using FinTrack.Api.Infrastructure.Interfaces;
using FinTrack.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrack.Api.Infrastructure.Implementations
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public string Generate(User user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );


            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(_options.ExpiresHours)
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
