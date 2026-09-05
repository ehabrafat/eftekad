using Eftekad.Data;
using Eftekad.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.Members;

public static class DeleteMemberFeature
{
    public class DeleteMemberEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("members/{id}", Handler)
                .DisableAntiforgery();
        }
    }
    
    public static async Task<IResult> Handler(
        int id,
        EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        // Find the member
        var member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        // Return 404 if member doesn't exist
        if (member is null)
        {
            return Results.NotFound($"Member with ID {id} not found");
        }

        // Remove the member
        dbContext.Remove(member);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new { Message = "Member deleted successfully", Id = id });
    }
}