using AutoMapper;
using Identity.API.DTOs;
using Identity.API.Models;

namespace Identity.API.MappingProfiles;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<Image, ImageResponse>()
            .ForCtorParam(
                nameof(ImageResponse.URI),
                options => options.MapFrom(src => $"data:{src.MediaType};base64," + Convert.ToBase64String(src.Data)));

        CreateMap<Experience, ExperienceDTO>();

        CreateMap<User, ProfileResponse>()
            .ForCtorParam(
                nameof(ProfileResponse.Avatar),
                option => option.MapFrom(src => src.AvatarImage))
            .ForCtorParam(
                nameof(ProfileResponse.InterestedTopicIds),
                option => option.MapFrom(src => src.InterestedTopics.Select(x => x.TopicId)));

        CreateMap<User, InfoResponse>()
            .ForCtorParam(
                nameof(ProfileResponse.Avatar),
                option => option.MapFrom(src => src.AvatarImage));

        CreateMap<User, InstructorResponse>()
            .ForCtorParam(
                nameof(ProfileResponse.Avatar),
                option => option.MapFrom(src => src.AvatarImage));

        CreateMap<User, UserResponse>();

        CreateMap<EditUserRequest, User>()
            .ForAllMembers(options =>
            {
                options.Condition((source, destination, member) => member != null);
            });

        CreateMap<AddUserRequest, User>();
    }
}
