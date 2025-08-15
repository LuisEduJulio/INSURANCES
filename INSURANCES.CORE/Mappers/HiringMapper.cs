using AutoMapper;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.ModelView;

namespace INSURANCES.CORE.Mappers
{
    public class HiringMapper : Profile
    {
        public HiringMapper()
        {
            CreateMap<HiringEntity, HiringModelView>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                   .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.ProposalId, opt => opt.MapFrom(src => src.ProposalId))
                   .ForMember(dest => dest.HiringDate, opt => opt.MapFrom(src => src.HiringDate))
                   .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved));
        }
    }
}