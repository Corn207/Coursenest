using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Entities;
using AutoMapper;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;

namespace Authentication.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		CreateMap<SetRole, Role>();
		CreateMap<Role, RoleResult>();

		CreateMap<Register, CreateUser>();
		CreateMap<Register, Credential>();

		CreateMap<Credential, CredentialsResult.Credential>()
			.ForMember(
				dst => dst.Roles, opt => opt.MapFrom(
				src => src.Roles.Select(x => x.Type)));
	}
}
