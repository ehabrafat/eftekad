using AutoMapper;

namespace Eftekad.Features.AcademicStages;

public class AcademicStageProfile : Profile
{
    public AcademicStageProfile()
    {
        CreateMap<CreateAcademicStageFeature.CreateAcademicStageReq, AcademicStage>();
    }
}