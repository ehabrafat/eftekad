using System.Globalization;
using AutoMapper;
using Eftekad.Shared.Extensions;

namespace Eftekad.Features.Members.Mappings;

public class MemberProfile : Profile
{
    public MemberProfile()
    {
        CreateMap<CreateMemberFeature.CreateMemberReq, Member>()
            .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth.ToDateOnly()));
        
        
    }
}
