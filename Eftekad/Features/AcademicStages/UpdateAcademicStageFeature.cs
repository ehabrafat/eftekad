using Eftekad.Data;
using Eftekad.Endpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.AcademicStages;

public static class UpdateAcademicStageFeature
{
    public class UpdateAcademicStageReq
    {
        public string Name { get; set; } = string.Empty;
        public int? Code { get; set; }
    }
    
    public class UpdateAcademicStageEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("academic-stages/{id}", Handler)
                .DisableAntiforgery();
        }
    }

    public static async Task<IResult> Handler(
        int id,
        [FromBody] UpdateAcademicStageReq req,
        EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var academicStage = await dbContext.AcademicStages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (academicStage is null)
        {
            return Results.NotFound($"Academic stage with ID {id} not found");
        }
        
        academicStage.Code = req.Code ?? academicStage.Code;
        academicStage.Name = !string.IsNullOrEmpty(req.Name)  ? req.Name : academicStage.Name;
        
        var existingWithSameName = await dbContext.AcademicStages
            .AnyAsync(x => x.Code == academicStage.Code && x.Id != id, cancellationToken);

        if (existingWithSameName)
        {
            return Results.Conflict($"An academic stage with the code '{req.Code}' already exists");
        }
        
        dbContext.Update(academicStage);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new
        {
            id = academicStage.Id,
            code = academicStage.Code,
            name = academicStage.Name
        });
    }
}