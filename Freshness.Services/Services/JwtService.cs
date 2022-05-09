using Common.Settings;
using Freshness.Common.Constants;
using Freshness.Domain.Entities;
using Freshness.Models.ResponseModels;
using Freshness.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Freshness.Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtBearerSettings _jwtBearerSettings;

        public JwtService(IOptions<JwtBearerSettings> jwtBearerSettings)
        {
            _jwtBearerSettings = jwtBearerSettings.Value;
        }

        public TokenResponseModel GenerateToken(Worker user)
        {
            var token = CreateJwtToken(user);

            return token;
        }

        private TokenResponseModel CreateJwtToken(Worker user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email ?? "email"),
                    new Claim(ClaimTypes.Name, user.Name ?? "username"),
                    new Claim(ClaimTypes.Role, user.Role ?? Role.Worker)
                };

            var now = DateTime.UtcNow;

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtBearerSettings.Issuer,
                audience: _jwtBearerSettings.Audience,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromDays(_jwtBearerSettings.ExpireDate)),
                signingCredentials: new SigningCredentials(_jwtBearerSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var jwtTokenInStringFormat = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new TokenResponseModel
            {
                AccessToken = jwtTokenInStringFormat
            };
        }
    }
}
