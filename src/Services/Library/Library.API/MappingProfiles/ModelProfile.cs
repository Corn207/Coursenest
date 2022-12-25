using AutoMapper;
using Library.API.DTOs;
using Library.API.DTOs.Courses;
using Library.API.DTOs.Ratings;
using Library.API.DTOs.Units;
using Library.API.Models;

namespace Library.API.MappingProfiles;

public class ModelProfile : Profile
{
	public ModelProfile()
	{
		CreateMap<Category, CategoryResponse>();

		CreateMap<Subcategory, SubcategoryResponse>();

		CreateMap<Topic, TopicResponse>();

		CreateMap<Image, ImageResponse>()
			.ForCtorParam(
				nameof(ImageResponse.URI),
				options => options.MapFrom(src => $"data:{src.MediaType};base64," + Convert.ToBase64String(src.Data)));

		CreateMap<Rating, RatingResponse>();

        CreateMap<List<Rating>, RatingStatResponse>()
			.ForCtorParam(
				nameof(RatingStatResponse.AverageStars),
				options => options.MapFrom(src => Math.Round(src.Average(x => x.Stars), 1)))
			.ForCtorParam(
				nameof(RatingStatResponse.Count),
				options => options.MapFrom(src => src.Count));

		CreateMap<Course, CourseCardResponse>()
			.ForCtorParam(
				nameof(CourseCardResponse.Image),
				option => option.MapFrom(src => src.Image))
			.ForCtorParam(
				nameof(CourseCardResponse.RatingStat),
				option => option.MapFrom(src => src.Ratings));

		CreateMap<Course, CourseResponse>()
			.ForCtorParam(
				nameof(CourseResponse.Materials),
				option => option.MapFrom(src => src.Lessons
					.OfType<Material>()))
			.ForCtorParam(
				nameof(CourseResponse.Exams),
				option => option.MapFrom(src => src.Lessons
					.OfType<Exam>()));

		CreateMap<Unit, UnitResponse>()
			.IncludeAllDerived();

		CreateMap<Material, UnitResponse>();

		CreateMap<Exam, UnitResponse>();


		CreateMap<CategoryRequest, Category>();

		CreateMap<SubcategoryRequest, Subcategory>();

		CreateMap<TopicRequest, Topic>();

		CreateMap<RatingPostRequest, Rating>();

        CreateMap<RatingPutRequest, Rating>()
            .ForAllMembers(options =>
            {
                options.Condition((source, destination, member) => member != null);
            });
    }
}
