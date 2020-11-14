using AutoMapper;
using HandwritingRecognition.BL.Models;
using HandwritingRecognition.Web.Models;

namespace HandwritingRecognition.Web.Mapper
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<PredictionResultDto, PredictionResultViewModel>();
        }
    }
}
