using AutoMapper;
using ApiDevBP.Entities;
using ApiDevBP.Models;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<UserEntity, UserModel>().ReverseMap();
    }
}