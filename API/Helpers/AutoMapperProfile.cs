using System;
using API.DTOS;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Mapper;

public class AutoMapperProfile :Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser,MemberDto>()
        .ForMember(a =>a.Age, o=>o.MapFrom(s =>s.DateOfBirth.CalculateAge()))
        .ForMember(d =>d.PhotoUrl ,o =>o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Photo,PhotoDto>();
        CreateMap<MemberUpdateDto,AppUser>();
        CreateMap<RegisterDto,AppUser>();
        CreateMap<string,DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
    }
}
