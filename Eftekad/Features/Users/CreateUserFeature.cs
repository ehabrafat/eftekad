using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using Eftekad.Data;
using Eftekad.Endpoints;
using Eftekad.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.Users;

public static class CreateUserFeature
{
    public class CreateUserReq
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = Auth.Role.User;

        [Column("church")]
        public string Church { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("profilePic")]
        public IFormFile? ProfilePic { get; set; }
    }
    
    public class CreateUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users", Handler)
                .DisableAntiforgery();
        }
    }

    public class CreateUserValidator : AbstractValidator<CreateUserReq>
    {
        public CreateUserValidator(EfDbContext dbContext)
        {
            RuleFor(x => x.Username)
                .MustAsync(async (username, cancellationToken) =>
                {
                    return !(await dbContext.Users.AnyAsync(x => x.Username == username,  cancellationToken));
                }).WithMessage(ErrorMessage.UsernameAlreadyExists)
                .When(x => !string.IsNullOrEmpty(x.Username));
            
            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellationToken) =>
                {
                    return !(await dbContext.Users.AnyAsync(x => x.Email == email,  cancellationToken));
                })
                .WithMessage(ErrorMessage.EmailAlreadyExists)
                .When(x => !string.IsNullOrEmpty(x.Email));

        }
    }

    public static async Task<IResult> Handler([FromForm] CreateUserReq req, IMapper mapper,
        EfDbContext dbContext,
        IValidator<CreateUserReq> validator,
        CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await validator.ValidateAsync(req, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(new {Errors = validationResult.Errors.Select(x => x.ErrorMessage)});
        }
        var user =  mapper.Map<CreateUserReq, User>(req);
        user.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.Created($"/users/{user.Id}", null);
    }
}