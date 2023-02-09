using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.Utilities.APIs;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserData.API.DTOs;
using UserData.API.Infrastructure.Contexts;

namespace UserData.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class SubmissionsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;
	private readonly IRequestClient<GetExam> _getExamClient;

	public SubmissionsController(
		IMapper mapper,
		DataContext context,
		IRequestClient<GetExam> getExamClient)
	{
		_mapper = mapper;
		_context = context;
		_getExamClient = getExamClient;
	}


	// GET: /submissions/ongoing
	[HttpGet("ongoing")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionOngoingResult))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetOngoing()
	{
		var userId = GetUserId();

		var result = await _context.Submissions
			.Where(x =>
				x.StudentUserId == userId &&
				DateTime.Now < x.Created.Add(x.TimeLimit) &&
				DateTime.Now < x.Created.Add(x.Elapsed))
			.ProjectTo<SubmissionOngoingResult>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync();
		if (result == null)
			return NotFound("There is no ongoing examination.");

		return Ok(result);
	}

	//// POST: /submissions
	//[HttpPost]
	//[ProducesResponseType(StatusCodes.Status201Created)]
	//[ProducesResponseType(StatusCodes.Status409Conflict)]
	//public async Task<IActionResult> StartExam(
	//	[FromBody] StartExam body)
	//{
	//	var userId = GetUserId();

	//	var enrollmentExists = await _context.Enrollments
	//		.AnyAsync(x =>
	//			x.EnrollmentId == body.EnrollmentId &&
	//			x.StudentUserId == userId);
	//	if (!enrollmentExists)
	//		return NotFound("Enrollment does not exist.");

	//	var ongoingExists = await _context.Submissions
	//		.AnyAsync(x =>
	//			x.EnrollmentId == body.EnrollmentId &&
	//			x.Enrollment.Completed != null &&
	//			x.StudentUserId == userId &&
	//			DateTime.Now < x.Created.Add(x.TimeLimit) &&
	//			DateTime.Now < x.Created.Add(x.Elapsed));
	//	if (ongoingExists)
	//		return Conflict("EnrollmentId is completed or " +
	//			"does not exist or " +
	//			"there is an ongoing examination.");

	//	var request = new GetExam()
	//	{
	//		UnitId = body.ExamUnitId
	//	};
	//	var response = await _getExamClient
	//		.GetResponse<ExamResult, NotFound>(request);
	//	if (response.Is(out Response<NotFound>? notFoundResponse))
	//	{
	//		return NotFound(notFoundResponse!.Message.Message);
	//	}
	//	if (!response.Is(out Response<ExamResult>? examResult) ||
	//		examResult == null)
	//	{
	//		return StatusCode(StatusCodes.Status500InternalServerError);
	//	}

	//	var submission = _mapper.Map<Submission>(examResult.Message);
	//	submission.StudentUserId = userId;
	//	submission.EnrollmentId = body.EnrollmentId;
	//	_context.Submissions.Add(submission);

	//	await _context.SaveChangesAsync();

	//	var result = _mapper.Map<SubmissionOngoingResult>(submission);

	//	return CreatedAtAction(nameof(GetOngoing), null, result);
	//}


	//// GET: /submissions
	//[HttpGet]
	//[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionOngoingResult))]
	//[ProducesResponseType(StatusCodes.Status404NotFound)]
	//public async Task<ActionResult<IEnumerable<SubmissionGradeResult>>> GetAll(
	//	[FromBody] int enrollmentId)
	//{
	//	var userId = GetUserId();

	//	var results = await _context.Submissions
	//		.Where(x =>
	//			x.EnrollmentId == enrollmentId &&
	//			x.StudentUserId == userId)
	//		.ProjectTo<SubmissionGradeResult>(_mapper.ConfigurationProvider)
	//		.ToListAsync();

	//	return results;
	//}

	//// GET: /submissions/5
	//[HttpGet("{submissionId}")]
	//public async Task<ActionResult<SubmissionResult>> Get(
	//	int submissionId)
	//{
	//	var userId = GetUserId();

	//	var result = await _context.Submissions
	//		.ProjectTo<SubmissionResult>(_mapper.ConfigurationProvider)
	//		.FirstOrDefaultAsync(x =>
	//			x.SubmissionId == submissionId &&
	//			x.StudentUserId == userId);
	//	if (result == null)
	//		return NotFound("Submission does not exist.");

	//	return result;
	//}

	//// POST: /submissions/5/submit
	//[HttpPost("{submissionId}/submit")]
	//public async Task<ActionResult> Submit(
	//	int submissionId,
	//	[FromBody] SubmitSubmission body)
	//{
	//	var userId = GetUserId();

	//	var ongoing = await _context.Submissions
	//		.Include(x => x.Questions)
	//		.ThenInclude(x => x.Choices)
	//		.FirstOrDefaultAsync(x =>
	//			x.SubmissionId == submissionId &&
	//			x.StudentUserId == userId &&
	//			DateTime.Now < x.Created.Add(x.TimeLimit) &&
	//			DateTime.Now < x.Created.Add(x.Elapsed));
	//	if (ongoing == null) return NotFound();

	//	var instructorRequires = ongoing.Questions.Any(x => x.Choices.Count == 0);

	//	foreach (var item in body.Answers)
	//	{
	//		var question = ongoing.Questions
	//			.FirstOrDefault(x => x.QuestionId == item.QuestionId);
	//		if (question == null)
	//			return NotFound($"QuestionId: {item.QuestionId} is not existed.");

	//		if (item.ChoiceId != null)
	//		{
	//			var choice = question.Choices.FirstOrDefault(x => x.ChoiceId == item.ChoiceId);
	//			if (choice == null)
	//				return NotFound($"ChoiceId: {item.ChoiceId} is not existed.");

	//			choice.IsChosen = true;
	//		}
	//		else
	//		{
	//			var choice = new Choice()
	//			{
	//				Content = item.Content!
	//			};
	//			question.Choices.Add(choice);
	//		}
	//	}
	//	ongoing.Elapsed = DateTime.Now - ongoing.Created;

	//	if (!instructorRequires)
	//	{
	//		var maxGrade = ongoing.Questions.Sum(x => x.Point);
	//		var quizGrade = ongoing.Questions
	//			.Where(x => x.Choices.Any(x => x.IsCorrect == x.IsChosen))
	//			.Sum(x => x.Point);

	//		ongoing.Grade = quizGrade;
	//		ongoing.Graded = DateTime.Now;

	//		if (quizGrade / (float)maxGrade > 0.75)
	//		{
	//			var completedUnit = new CompletedUnit()
	//			{
	//				EnrollmentId = ongoing.EnrollmentId,
	//				UnitId = ongoing.UnitId
	//			};
	//			_context.CompletedUnits.Add(completedUnit);
	//		}
	//	}

	//	await _context.SaveChangesAsync();

	//	var result = _mapper.Map<SubmissionResult>(ongoing);

	//	return CreatedAtAction(nameof(Get), new { ongoing.SubmissionId }, result);
	//}

	//// POST: /submissions/5/grading
	//[HttpPost("{submissionId}/grading")]
	//[Authorize(Roles = nameof(RoleType.Instructor))]
	//public async Task<ActionResult> Grading(
	//	int submissionId,
	//	[FromBody] GradingSubmission body)
	//{
	//	var userId = GetUserId();

	//	var submission = await _context.Submissions
	//		.Include(x => x.Questions)
	//		.ThenInclude(x => x.Choices)
	//		.FirstOrDefaultAsync(x =>
	//			x.SubmissionId == submissionId &&
	//			x.Graded != null);
	//	if (submission == null) return NotFound();

	//	var availableCriteriaPoint = submission.Questions
	//		.Where(x => x.Choices.All(x => x.IsChosen == null))
	//		.Sum(x => x.Point);
	//	var maxCriteriaPoint = body.Criteria.Sum(x => x.Checkpoints.Max(x => x.Point));
	//	if (availableCriteriaPoint != maxCriteriaPoint)
	//		return BadRequest(
	//			$"Available criteria point is ({availableCriteriaPoint}).\n" +
	//			$"Your maximum criteria point is ({maxCriteriaPoint})");

	//	_mapper.Map(body, submission);

	//	var maxGrade = submission.Questions.Sum(x => x.Point);
	//	var quizGrade = submission.Questions
	//		.Where(x => x.Choices.Any(x => x.IsCorrect == x.IsChosen))
	//		.Sum(x => x.Point);
	//	var criteriaGrade = submission.Criteria
	//		.SelectMany(x => x.Checkpoints)
	//		.Where(x => x.IsChosen)
	//		.Sum(x => x.Point);

	//	submission.Grade = quizGrade + criteriaGrade;
	//	submission.InstructorUserId = userId;
	//	submission.Graded = DateTime.Now;

	//	if (quizGrade / (float)maxGrade > 0.75)
	//	{
	//		var completedUnit = new CompletedUnit()
	//		{
	//			EnrollmentId = submission.EnrollmentId,
	//			UnitId = submission.UnitId
	//		};
	//		_context.CompletedUnits.Add(completedUnit);
	//	}

	//	await _context.SaveChangesAsync();

	//	var result = _mapper.Map<SubmissionResult>(submission);

	//	return CreatedAtAction(nameof(Get), new { submissionId }, result);
	//}


	//// POST: /submissions/5/comment
	//[HttpPost("{submissionId}/comment")]
	//public async Task<ActionResult> Comment(
	//	int submissionId,
	//	[FromBody] string commentContent)
	//{
	//	var userId = GetUserId();

	//	Submission? submission;
	//	if (User.IsInRole(nameof(RoleTypes.Instructor)))
	//	{
	//		submission = await _context.Submissions
	//			.FirstOrDefaultAsync(x =>
	//				x.SubmissionId == submissionId &&
	//				(x.InstructorUserId == userId ||
	//				x.InstructorUserId == null));
	//	}
	//	else
	//	{
	//		submission = await _context.Submissions
	//			.FirstOrDefaultAsync(x =>
	//				x.SubmissionId == submissionId &&
	//				x.StudentUserId == userId);
	//	}

	//	if (submission == null) return NotFound();

	//	var comment = new Comment()
	//	{
	//		Content = commentContent,
	//		OwnerUserId = userId,
	//		Created = DateTime.Now,
	//		SubmissionId = submissionId
	//	};
	//	_context.Comments.Add(comment);

	//	if (User.IsInRole(nameof(RoleTypes.Instructor)))
	//		submission.InstructorUserId ??= userId;

	//	await _context.SaveChangesAsync();

	//	var result = _mapper.Map<SubmissionResult.Comment>(comment);

	//	return CreatedAtAction(nameof(Get), new { submissionId }, result);
	//}


	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}
}
