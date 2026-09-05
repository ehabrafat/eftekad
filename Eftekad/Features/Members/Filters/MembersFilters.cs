namespace Eftekad.Features.Members.Filters;

public static class MembersFilters
{
    public static IQueryable<Member> ApplyFilter(
        this IQueryable<Member> query,
        GetAllMembersFeature.GetAllMembersFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(m =>
                m.Name.Contains(searchTerm) ||
                m.Phone.Contains(searchTerm));
        }
        // Apply status filter
        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            query = query.Where(m => m.Status == filter.Status);
        }

        // Apply gender filter
        if (!string.IsNullOrWhiteSpace(filter.Gender))
        {
            query = query.Where(m => m.Gender == filter.Gender);
        }
        
        if (filter.AcademicStageId.HasValue)
        {
            query = query.Where(m => m.AcademicStageId == filter.AcademicStageId.Value);
        }
        return query;
    }
}