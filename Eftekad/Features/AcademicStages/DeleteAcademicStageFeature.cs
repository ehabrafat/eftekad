using Eftekad.Data;
using Eftekad.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.AcademicStages;

public static class DeleteAcademicStageFeature
{
    
    public class DeleteAcademicStagesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("academic-stages/{id}", Handler);
        }
    }

    public static async Task<IResult> Handler(
        int id,
        EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var academicStage = await dbContext.AcademicStages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (academicStage is null)
        {
            return Results.NotFound($"Academic stage with ID {id} not found");
        }
        // handle later if there are members in this academic stage
        dbContext.Remove(academicStage);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }
}