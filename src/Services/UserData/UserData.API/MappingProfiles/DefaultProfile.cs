using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using AutoMapper;
using UserData.API.DTOs;
using UserData.API.Infrastructure.Entities;

namespace UserData.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		CreateMap<Enrollment, EnrollmentResult>();
		CreateMap<Enrollment, EnrollmentDetailResult>()
			.ForMember(
				dst => dst.CompletedUnits, opt => opt.MapFrom(
				src => src.CompletedUnits.Select(x => x.UnitId)));

		// Submission
		CreateMap<ExamResult, Submission>()
			.ForMember(
				dst => dst.Created, opt => opt.MapFrom(
				_ => DateTime.Now))
			.ForMember(
				dst => dst.Elapsed, opt => opt.MapFrom(
				src => src.TimeLimit));
		CreateMap<ExamResult.Question, Question>();
		CreateMap<ExamResult.Choice, Choice>()
			.ForMember(
				dst => dst.IsChosen, opt => opt.MapFrom(
				_ => false));

		CreateMap<GradingSubmission, Submission>();
		CreateMap<GradingSubmission.Criterion, Criterion>();
		CreateMap<GradingSubmission.Checkpoint, Checkpoint>();

		CreateMap<Submission, SubmissionGradeResult>();

		CreateMap<Submission, SubmissionOngoingResult>();
		CreateMap<Question, SubmissionOngoingResult.Question>();
		CreateMap<Choice, SubmissionOngoingResult.Choice>();

		CreateMap<Submission, SubmissionResult>();
		CreateMap<Question, SubmissionResult.Question>();
		CreateMap<Choice, SubmissionResult.Choice>();
		CreateMap<Criterion, SubmissionResult.Criterion>();
		CreateMap<Comment, SubmissionResult.Comment>();
	}
}
