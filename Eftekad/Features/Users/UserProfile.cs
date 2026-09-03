using AutoMapper;

namespace Eftekad.Features.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserFeature.CreateUserReq, User>();
    }
}