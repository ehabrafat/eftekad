using Eftekad.Data;
using Eftekad.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.Users;

public static class DeleteUserFeature
{
    public class DeleteUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("users/{id}", Handler);
        }
    }
    
    public static async Task<IResult> Handler(int id, EfDbContext dbContext, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        
        // 2. If user doesn't exist, return 404 Not Found
        if (user is null)
        {
            return Results.NotFound(new { Message = $"User with ID '{id}' not found" });
        }
        
        // 3. Remove the user
        dbContext.Remove(user);
        
        // 4. Save changes to database
        await dbContext.SaveChangesAsync(cancellationToken);
        
        // 5. Return 204 No Content (successful deletion)
        return Results.NoContent();
    }
    
}