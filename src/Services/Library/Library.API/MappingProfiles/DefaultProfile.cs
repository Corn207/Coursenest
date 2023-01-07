using AutoMapper;
using Library.API.DTOs;
using Library.API.DTOs.Categories;
using Library.API.DTOs.Courses;
using Library.API.DTOs.Lessons;
using Library.API.DTOs.Ratings;
using Library.API.DTOs.Units;
using Library.API.Infrastructure.Entities;

namespace Library.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		// Category
		CreateMap<Category, CategoryResult>();

		CreateMap<Subcategory, SubcategoryResult>();

		CreateMap<Topic, TopicDetailedResult>()
			.ForMember(des => des.SubcategoryContent, opt => opt.MapFrom(x => x.Subcategory.Content))
			.ForMember(des => des.CategoryContent, opt => opt.MapFrom(x => x.Subcategory.Category.Content));

		CreateMap<Category, IdContentResult>();
		CreateMap<Subcategory, IdContentResult>();
		CreateMap<Topic, IdContentResult>();

		// Course
		CreateMap<Course, CourseResult>();
		CreateMap<Course, CourseDetailedResult>();

		CreateMap<CourseCover, ImageResult>()
			.ForMember(
				nameof(ImageResult.URI),
				options => options.MapFrom(src => $"data:{src.MediaType};base64,{Convert.ToBase64String(src.Data)}"));

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

		// Rating
		CreateMap<CreateRating, Rating>()
			.ForMember(des => des.Created, opt => opt.MapFrom(_ => DateTime.Now));

		CreateMap<Rating, RatingResult>();

		// Lesson
		CreateMap<CreateLesson, Lesson>();
		CreateMap<UpdateLesson, Lesson>()
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<Lesson, LessonResult>();

		// Unit
		CreateMap<Unit, UnitResult>();


		CreateMap<CreateMaterial, Material>();
		CreateMap<UpdateMaterial, Material>()
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<Material, MaterialResult>();


		CreateMap<CreateExam, Exam>();
		CreateMap<UpdateExam, Exam>()
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<Exam, ExamResult>();

		CreateMap<CreateQuestion, Question>();
		CreateMap<UpdateQuestion, Question>()
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});
		CreateMap<QuestionResult, Question>();

		CreateMap<Question, QuestionResult>();

		CreateMap<ChoiceResult, Choice>();
		CreateMap<Choice, ChoiceResult>();
	}
}
