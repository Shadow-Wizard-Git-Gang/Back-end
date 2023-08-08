using AutoMapper;
using VC.Models;
using VC.Models.DTOs.UserDTOs;
using VC.Models.Identity;

namespace VC.Helpers.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig() { 
            CreateMap<ApplicationUser, User>().ReverseMap();
            CreateMap<ApplicationUser, UserCreateRequestDTO>().ReverseMap();
        }
    }
}
