using AutoMapper;
using Dto;
using Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<Protocol, ProtocolDto>().ReverseMap();
            CreateMap<ProtocolContent, ProtocolContentDto>().ReverseMap();
            CreateMap<ProtocolPdfFile, ProtocolPdfFileDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Template, TemplateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}