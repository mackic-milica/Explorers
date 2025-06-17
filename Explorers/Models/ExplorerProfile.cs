using AutoMapper;
using SredaZadatak.Models.DTO;

namespace SredaZadatak.Models
{
    public class ExplorerProfile : Profile
    {
        public ExplorerProfile()
        {
           CreateMap<Explorer, ExplorerDTO>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name));
           
        }
    }
}
