using AutoMapper;
using Library.API.DTOs;
using Library.API.DTOs.Categories;
using Library.API.DTOs.Courses;
using Library.API.DTOs.Lessons;
using Library.API.DTOs.Ratings;
using Library.API.DTOs.Units;
using Library.API.Infrastructure.Entities;
using static Library.API.DTOs.Units.CreateQuestion;

namespace Library.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		#region Category
		CreateMap<Category, CategoryResult>();
		CreateMap<Subcategory, CategoryResult.SubcategoryResult>();

		CreateMap<Topic, TopicDetailedResult>()
			.ForMember(dst => dst.SubcategoryId, opt => opt.MapFrom(x => x.SubcategoryId))
			.ForMember(dst => dst.SubcategoryContent, opt => opt.MapFrom(x => x.Subcategory.Content))
			.ForMember(dst => dst.CategoryId, opt => opt.MapFrom(x => x.Subcategory.CategoryId))
			.ForMember(dst => dst.CategoryContent, opt => opt.MapFrom(x => x.Subcategory.Category.Content));

		CreateMap<Category, IdContentResult>()
			.ForMember(dst => dst.Id, opt => opt.MapFrom(x => x.CategoryId));
		CreateMap<Subcategory, IdContentResult>()
			.ForMember(dst => dst.Id, opt => opt.MapFrom(x => x.SubcategoryId));
		CreateMap<Topic, IdContentResult>()
			.ForMember(dst => dst.Id, opt => opt.MapFrom(x => x.TopicId));
		#endregion


		#region Course
		CreateMap<CreateCourse, Course>()
			.ForMember(dst => dst.IsApproved, opt => opt.MapFrom(_ => false))
			.ForMember(dst => dst.Created, opt => opt.MapFrom(_ => DateTime.Now))
			.ForMember(dst => dst.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
			.ForMember(dst => dst.RatingAverage, opt => opt.MapFrom(_ => 0))
			.ForMember(dst => dst.RatingTotal, opt => opt.MapFrom(_ => 0));
		CreateMap<UpdateCourse, Course>()
			.ForMember(dst => dst.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
			.ForAllMembers(options =>
			{
				options.Condition((source, dsttination, member) => member != null);
			});

		CreateMap<Course, CourseResult>();
		CreateMap<Course, CourseDetailedResult>();
		CreateMap<CourseCover, ImageResult>()
			.ForMember(
				dst => dst.URI, opt => opt.MapFrom(
				src => $"data:{src.MediaType};base64,{Convert.ToBase64String(src.Data)}"));
		#endregion


		#region Rating
		CreateMap<CreateRating, Rating>()
			.ForMember(dst => dst.Created, opt => opt.MapFrom(_ => DateTime.Now));

		CreateMap<Rating, RatingResult>();
		#endregion


		#region Lesson
		CreateMap<CreateLesson, Lesson>();
		CreateMap<UpdateLesson, Lesson>()
			.ForAllMembers(options =>
			{
				options.Condition((source, dsttination, member) => member != null);
			});

		CreateMap<Lesson, LessonResult>();
		#endregion


		#region Unit
		CreateMap<Unit, UnitResult>()
			.Include<Material, UnitResult>()
			.ForMember(
				dst => dst.IsExam, opt => opt.MapFrom(
				_ => true));

		CreateMap<Material, UnitResult>()
			.ForMember(
				dst => dst.IsExam, opt => opt.MapFrom(
				_ => false));
		#endregion


		#region Material
		CreateMap<CreateMaterial, Material>()
			.ForMember(
				dst => dst.RequiredTime, opt => opt.MapFrom(
				src => TimeSpan.FromMinutes(src.RequiredTimeMinutes)));
		CreateMap<UpdateMaterial, Material>()
			.ForAllMembers(options =>
			{
				options.Condition((source, dsttination, member) => member != null);
			});

		CreateMap<Material, MaterialResult>()
			.ForMember(
				dst => dst.CourseId, opt => opt.MapFrom(
				src => src.Lesson.CourseId));
		#endregion


		#region Exam
		CreateMap<CreateExam, Exam>()
			.ForMember(
				dst => dst.RequiredTime, opt => opt.MapFrom(
				src => TimeSpan.FromMinutes(src.RequiredTimeMinutes)));
		CreateMap<UpdateExam, Exam>()
			.ForAllMembers(options =>
			{
				options.Condition((source, dsttination, member) => member != null);
			});

		CreateMap<Exam, ExamResult>()
			.ForMember(
				dst => dst.CourseId, opt => opt.MapFrom(
				src => src.Lesson.CourseId));

		CreateMap<Exam, CommonLibrary.API.MessageBus.Responses.ExamResult>()
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
		#endregion


		#region Question && Choice
		CreateMap<CreateQuestion, Question>();
		CreateMap<CreateChoice, Choice>();

		CreateMap<UpdateQuestion, Question>()
			.ForAllMembers(options =>
			{
				options.Condition((source, dsttination, member) => member != null);
			});

		CreateMap<Question, QuestionResult>();
		CreateMap<Choice, ChoiceResult>();
		#endregion
	}
}
