using APICommonLibrary.Contracts;
using Authentication.API.DTOs;
using Authentication.API.Models;
using AutoMapper;

namespace Authentication.API.MappingProfiles;

public class ModelProfile : Profile
{
	public ModelProfile()
	{
		CreateMap<Role, RoleDTO>();


		CreateMap<RegisterRequest, AddUserRequest>();
	}
}
