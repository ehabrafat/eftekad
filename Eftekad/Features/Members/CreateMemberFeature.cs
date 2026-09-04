using AutoMapper;
using Eftekad.Data;
using Eftekad.Endpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Eftekad.Features.Members;

public static class CreateMemberFeature
{
    public class CreateMemberReq
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int? AcademicStageId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Apartment { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
    
    public class CreateMemberEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("members", Handler)
                .DisableAntiforgery();
        }
    }

    public static async Task<IResult> Handler(
        [FromBody] CreateMemberReq req,
        IMapper mapper,
        EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var member = mapper.Map<CreateMemberReq, Member>(req);
        dbContext.Members.Add(member);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.Created();
    }
}