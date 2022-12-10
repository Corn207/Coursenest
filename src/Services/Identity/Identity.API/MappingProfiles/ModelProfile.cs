using AutoMapper;
using Identity.API.DTOs;
using Identity.API.Models;

namespace Identity.API.MappingProfiles;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<Avatar, ImageResponse>()
            .ForCtorParam(
                nameof(ImageResponse.URI),
                options => options.MapFrom(src => $"data:{src.MediaType};base64," + Convert.ToBase64String(src.Data)));

        CreateMap<Experience, ExperienceResponse>();

        CreateMap<User, UserProfileResponse>()
            .ForCtorParam(
                nameof(UserProfileResponse.Avatar),
                option => option.MapFrom(src => src.Avatar))
            .ForCtorParam(
                nameof(UserProfileResponse.InterestedTopicIds),
                option => option.MapFrom(src => src.InterestedTopics.Select(x => x.TopicId)))
            .ForCtorParam(
                nameof(UserProfileResponse.FollowedTopicIds),
                option => option.MapFrom(src => src.FollowedTopics.Select(x => x.TopicId)));

        CreateMap<User, UserInfoResponse>()
            .ForCtorParam(
                nameof(UserInfoResponse.Avatar),
                option => option.MapFrom(src => src.Avatar));

        CreateMap<User, UserInstructorResponse>()
            .ForCtorParam(
                nameof(UserInstructorResponse.Avatar),
                option => option.MapFrom(src => src.Avatar));

        CreateMap<User, UserResponse>();


        CreateMap<UserPutRequest, User>()
            .ForAllMembers(options =>
            {
                options.Condition((source, destination, member) => member != null);
            });

        CreateMap<UserPostRequest, User>();

        CreateMap<ExperiencePostRequest, Experience>();
    }
}
