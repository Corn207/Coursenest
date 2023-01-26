using APICommonLibrary.MessageBus.Responses;
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

		CreateMap<Category, IdContentResult>()
			.ForMember(des => des.Id, opt => opt.MapFrom(x => x.CategoryId));
		CreateMap<Subcategory, IdContentResult>()
			.ForMember(des => des.Id, opt => opt.MapFrom(x => x.SubcategoryId));
		CreateMap<Topic, IdContentResult>()
			.ForMember(des => des.Id, opt => opt.MapFrom(x => x.TopicId));

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

		CreateMap<Course, CourseResult>();
		CreateMap<Course, CourseDetailedResult>();
		CreateMap<CourseCover, ImageResult>()
			.ForMember(
				nameof(ImageResult.URI),
				options => options.MapFrom(src => $"data:{src.MediaType};base64,{Convert.ToBase64String(src.Data)}"));

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

		// Material
		CreateMap<CreateMaterial, Material>();
		CreateMap<UpdateMaterial, Material>()
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<Material, MaterialResult>();

		// Exam
		CreateMap<CreateExam, Exam>();
		CreateMap<UpdateExam, Exam>()
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<Exam, DTOs.Units.ExamResult>();

		CreateMap<Exam, APICommonLibrary.MessageBus.Responses.ExamResult>()
			.ForMember(
				dst => dst.LessonTitle, opt => opt.MapFrom(
				src => src.Lesson.Title))
			.ForMember(
				dst => dst.CourseTitle, opt => opt.MapFrom(
				src => src.Lesson.Course.Title))
			.ForMember(
				dst => dst.TimeLimit, opt => opt.MapFrom(
				src => src.RequiredTime))
			.ForMember(
				dst => dst.TopicId, opt => opt.MapFrom(
				src => src.Lesson.Course.TopicId));
		CreateMap<Question, APICommonLibrary.MessageBus.Responses.ExamResult.Question>();
		CreateMap<Choice, APICommonLibrary.MessageBus.Responses.ExamResult.Choice>();

		// Question
		CreateMap<CreateQuestion, Question>();
		CreateMap<UpdateQuestion, Question>()
			.ForAllMembers(options =>
			{
				options.Condition((source, destination, member) => member != null);
			});

		CreateMap<Question, QuestionResult>();

		// Choice
		CreateMap<ChoiceResult, Choice>();

		CreateMap<Choice, ChoiceResult>();
	}
}
