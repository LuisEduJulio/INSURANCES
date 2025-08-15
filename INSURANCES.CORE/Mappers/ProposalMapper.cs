using AutoMapper;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.ModelView;

namespace INSURANCES.CORE.Mappers
{
    public class ProposalMapper : Profile
    {
        public ProposalMapper()
        {
            CreateMap<ProposalEntity, ProposalModelView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.HiringId, opt => opt.MapFrom(src => src.Hiring != null ? src.Hiring.Id : (Guid?)null))
                .ForMember(dest => dest.HiringDate, opt => opt.MapFrom(src => src.Hiring != null ? src.Hiring.CreatedDate : (DateTime?)null))
                .ForMember(dest => dest.Proposal, opt => opt.MapFrom(src => src.Proposal))
                .ForMember(dest => dest.ProposalStatus, opt => opt.MapFrom(src => src.ProposalStatus))
                .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => src.IsDisabled));
        }
    }
}