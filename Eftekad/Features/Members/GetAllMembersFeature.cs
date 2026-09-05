using AutoMapper;
using Eftekad.Data;
using Eftekad.Endpoints;
using Eftekad.Features.Members.Filters;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.Members;

public static class GetAllMembersFeature
{
    public class MemberRes
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int? AcademicStageId { get; set; }
        public string AcademicStageName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Apartment { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
    
    public class GetAllMembersFilter
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public int? AcademicStageId { get; set; }
    }
    
    public class GetAllMembersEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("members", Handler)
                .DisableAntiforgery();
        }
    }
    
    public static async Task<IResult> Handler(
        [AsParameters] GetAllMembersFilter filter,
        IMapper mapper,
        EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Members.ApplyFilter(filter);
        var members = await query
            .Select(m => new MemberRes
            {
                Id = m.Id,
                Name = m.Name,
                Phone = m.Phone,
                DateOfBirth = m.DateOfBirth.ToString() ?? string.Empty,
                Status = m.Status,
                Gender = m.Gender,
                AcademicStageId = m.AcademicStageId,
                AcademicStageName = m.AcademicStage != null ? m.AcademicStage.Name : string.Empty,
                Address = m.Address,
                Area = m.Area,
                Street = m.Street,
                Building = m.Building,
                Floor = m.Floor,
                Apartment = m.Apartment,
                Notes = m.Notes,
            })
            .ToListAsync(cancellationToken);
        return Results.Ok(members);
    }
}