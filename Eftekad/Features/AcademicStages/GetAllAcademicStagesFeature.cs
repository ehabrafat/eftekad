using AutoMapper;
using Eftekad.Data;
using Eftekad.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.AcademicStages;

public static class GetAllAcademicStagesFeature
{
    public class AcademicStageRes
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class GetAllAcademicStagesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("academic-stages", Handler);
        }
    }
    public static async Task<IResult> Handler(
       EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var academicStages = await dbContext.AcademicStages
            .OrderBy(x => x.Code)
            .Select(x => new AcademicStageRes
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);
        return Results.Ok(academicStages);
    }
}