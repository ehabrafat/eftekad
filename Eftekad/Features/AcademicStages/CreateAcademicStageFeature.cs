using AutoMapper;
using Eftekad.Data;
using Eftekad.Endpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.AcademicStages;

public static class CreateAcademicStageFeature
{

    public class CreateAcademicStageReq
    {
        public string Name { get; set; } = string.Empty;
    }
    
    public class CreateAcademicStageEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("academic-stages", Handler)
                .DisableAntiforgery();
        }
    }

    public static async Task<IResult> Handler(
        [FromBody] CreateAcademicStageReq req,
        IMapper mapper,
        EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var academicStage = mapper.Map<CreateAcademicStageReq, AcademicStage>(req);
        // transaction here
        var lastOne = await dbContext.AcademicStages.OrderByDescending(x => x.Code).FirstOrDefaultAsync(cancellationToken);
        academicStage.Code = lastOne is not null ? lastOne.Code + 1 : 1;
        dbContext.AcademicStages.Add(academicStage);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.Created($"/academic-stages/{academicStage.Id}", new { 
            id = academicStage.Id,
            code = academicStage.Code,
            name = academicStage.Name
        });    
    }
}