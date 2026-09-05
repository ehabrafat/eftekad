using AutoMapper;
using Eftekad.Data;
using Eftekad.Endpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eftekad.Features.Members;

public static class UpdateMemberFeature
{
    public class UpdateMemberReq
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
    public class UpdateMemberEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("members/{id}", Handler)
                .DisableAntiforgery();
        }
    }
    public static async Task<IResult> Handler(
        int id,
        UpdateMemberReq req,
        IMapper mapper,
        EfDbContext dbContext,
        CancellationToken cancellationToken)
    {
        // Find existing member
        var existingMember = await dbContext.Members
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (existingMember is null)
        {
            return Results.NotFound($"Member with ID {id} not found");
        }

        // Map the updated values to the existing member
        mapper.Map(req, existingMember);
        
        // Update the member in the database
        dbContext.Update(existingMember);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new { Message = "Member updated successfully", Id = existingMember.Id });
    }
}