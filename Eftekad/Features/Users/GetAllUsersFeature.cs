using Eftekad.Data;
using Eftekad.Endpoints;
using Eftekad.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.Users;

public static class GetAllUsersFeature
{
    public class UserRes
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; }
        public string Church { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePic { get; set; } = string.Empty;
    }

    public class GetAllUsersEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("users", Handler)
               .RequireAuthorization(x => x.RequireAuthenticatedUser());
        }
    }

    public static async Task<IResult> Handler(
        EfDbContext dbContext,
        CancellationToken cancellationToken,
        ICurrentUser currentUser)
    {
        // 1. Start with base query
        var users = await dbContext.Users
            .Where(x=> x.Id != int.Parse(currentUser.Id))
            .Select(u => new UserRes
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Role = u.Role,
                Church = u.Church,
                Email = u.Email,
                ProfilePic = u.ProfilePic
            })
            .ToListAsync(cancellationToken);
        return Results.Ok(users);
    }
}