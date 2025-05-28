﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User.Domain.Models.JWT;   

namespace User.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateToken(int userId, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); -> token JWT

        var rsa = RSA.Create();
        string keyPath = Path.Combine(AppContext.BaseDirectory, "data", "private.key");
        keyPath = Path.GetFullPath(keyPath); // przekształca na absolutną ścieżkę
        rsa.ImportFromPem(File.ReadAllText(keyPath));
        var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

