using APICommonLibrary.MessageBus.Commands;
using AutoMapper;
using Identity.API.DTOs;
using Identity.API.Infrastructure.Entities;

namespace Identity.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		// User
		CreateMap<CreateUser, User>()
			.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.Now))
			.ForMember(dest => dest.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
			.ForMember(
				dest => dest.InterestedTopics,
				opt => opt.MapFrom(x => x.InterestedTopicIds
					.Select(i => new InterestedTopic() { TopicId = i })));
		CreateMap<UpdateUser, User>()
			.ForMember(dest => dest.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<User, UserResult>();
		CreateMap<User, UserProfileResult>();
		CreateMap<User, UserInstructorResult>();

		// Avatar
		CreateMap<Avatar, ImageResult>()
			.ForMember(
				nameof(ImageResult.URI),
				options => options.MapFrom(src => $"data:{src.MediaType};base64,{Convert.ToBase64String(src.Data)}"));

		// InterestedTopic
		CreateMap<InterestedTopic, int>()
			.ConvertUsing(x => x.TopicId);

		// FollowedTopic
		CreateMap<FollowedTopic, int>()
			.ConvertUsing(x => x.TopicId);

		// Experience
		CreateMap<CreateExperience, Experience>();

		CreateMap<Experience, ExperienceResult>();
	}
}
