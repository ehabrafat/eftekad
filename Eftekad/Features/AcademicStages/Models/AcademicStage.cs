using System.ComponentModel.DataAnnotations.Schema;
using Eftekad.Features.Members;

namespace Eftekad.Features.AcademicStages;

public class AcademicStage
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int Code { get; set; }
    
    public List<Member> Members { get; set; } = new();
}