using APICommonLibrary.MessageBus.Commands;
using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Entities;
using AutoMapper;

namespace Authentication.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		CreateMap<Role, RoleResult>();
		CreateMap<SetRole, Role>();

		CreateMap<Register, CreateUser>();
		CreateMap<Register, Credential>();
	}
}
