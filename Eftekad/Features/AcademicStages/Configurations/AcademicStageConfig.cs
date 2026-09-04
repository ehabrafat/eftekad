using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eftekad.Features.AcademicStages;

public class AcademicStageConfig : IEntityTypeConfiguration<AcademicStage>
{
    public void Configure(EntityTypeBuilder<AcademicStage> builder)
    {
        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}