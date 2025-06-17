using AutoMapper;
using SredaZadatak.Models.DTO;

namespace SredaZadatak.Models
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>();
        }
    }
}
