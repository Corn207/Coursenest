using AutoMapper;
using Library.API.DTOs;
using Library.API.DTOs.Courses;
using Library.API.Infrastructure.Entities;

namespace Library.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		// Course
		CreateMap<CreateCourse, Course>()
			.ForMember(des => des.IsApproved, opt => opt.MapFrom(_ => false))
			.ForMember(des => des.Created, opt => opt.MapFrom(_ => DateTime.Now))
			.ForMember(des => des.LastModified, opt => opt.MapFrom(_ => DateTime.Now));
		CreateMap<UpdateCourse, Course>()
			.ForMember(des => des.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<Course, CourseResult>()
			.ForMember(
				des => des.RatingStat,
				opt =>
				{
					opt.MapFrom(x => new RatingStatResult()
					{
						AverageStars = x.Ratings.Average(x => x.Stars),
						Total = x.Ratings.Count
					});
				});
		CreateMap<Course, CourseDetailedResult>();

		// CourseCover
		CreateMap<CourseCover, ImageResult>()
			.ForMember(
				nameof(ImageResult.URI),
				options => options.MapFrom(src => $"data:{src.MediaType};base64,{Convert.ToBase64String(src.Data)}"));

		// Rating
		CreateMap<CreateRating, Rating>()
			.ForMember(des => des.Created, opt => opt.MapFrom(_ => DateTime.Now));

		CreateMap<Rating, RatingResult>()
			.ForMember(des => des.OwnerUserId, opt => opt.MapFrom(x => x.UserId));

		// Category
		CreateMap<Category, DTOs.Categories.CategoryResult>();

		CreateMap<Subcategory, DTOs.Categories.SubcategoryResult>();
		CreateMap<Subcategory, DTOs.Categories.SubcategoryDetailedResult>()
			.ForMember(des => des.CategoryContent, opt => opt.MapFrom(x => x.Category.Content));

		CreateMap<Topic, DTOs.Categories.TopicResult>();
		CreateMap<Topic, DTOs.Categories.TopicDetailedResult>()
			.ForMember(des => des.SubcategoryContent, opt => opt.MapFrom(x => x.Subcategory.Content))
			.ForMember(des => des.CategoryContent, opt => opt.MapFrom(x => x.Subcategory.Category.Content));
	}
}
