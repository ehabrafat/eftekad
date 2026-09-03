using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Eftekad.Config;
using Eftekad.Data;
using Eftekad.Endpoints;
using Eftekad.Features.Users;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Eftekad.Features.Auth;

public static class LoginFeature
{

    public class LoginReq
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRes
    {
        public string AccessToken { get; set; } = string.Empty;
    }
    
    public class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/login", Handler)
                .DisableAntiforgery();
        }
    }

    public static async Task<IResult> Handler([FromBody] LoginReq req,
        EfDbContext dbContext,
        JwtConfig jwtConfig,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == req.Username, cancellationToken);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        // Verify password using BCrypt
        try
        {
            bool passwordValid = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);
            if (!passwordValid)
            {
                return Results.Unauthorized();
            }
        }
        catch (Exception)
        {
            return Results.Unauthorized();
        }
        
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtConfig.Secret));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires:  DateTime.Now.AddMinutes(jwtConfig.ExpiresIn),
            signingCredentials: credentials
        );
        
        return Results.Ok(new LoginRes{AccessToken = new JwtSecurityTokenHandler().WriteToken(token)});
    }

}