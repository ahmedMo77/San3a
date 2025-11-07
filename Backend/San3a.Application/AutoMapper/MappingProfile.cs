using AutoMapper;
using Microsoft.AspNetCore.Identity;
using San3a.Core.DTOs;
using San3a.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();

            //CreateMap<IdentityRole, RoleDto>().ReverseMap();
            //CreateMap<CreateRoleDto, IdentityRole>();
        }
    }
}
