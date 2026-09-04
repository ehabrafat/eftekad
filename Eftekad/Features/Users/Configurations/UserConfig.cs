using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eftekad.Features.Users;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(u => u.Member).WithOne(m => m.User)
            .HasForeignKey<User>(u => u.MemberId);
    }
}