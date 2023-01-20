using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Entities;
using AutoMapper;
using CommonLibrary.API.MessageBus.Commands;

namespace Authentication.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		CreateMap<SetRole, Role>();
		CreateMap<Role, RoleResult>();

		CreateMap<Register, CreateUser>();
		CreateMap<Register, Credential>();
	}
}
