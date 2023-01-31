using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserData.API.DTOs;
using UserData.API.Infrastructure.Contexts;
using UserData.API.Infrastructure.Entities;

namespace UserData.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class EnrollmentsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;
	private readonly IRequestClient<GetCourseTier> _getCourseTierClient;
	private readonly IRequestClient<CheckUnit> _checkUnitClient;

	public EnrollmentsController(
		IMapper mapper,
		DataContext context,
		IRequestClient<GetCourseTier> getCourseTierClient,
		IRequestClient<CheckUnit> checkUnitClient)
	{
		_mapper = mapper;
		_context = context;
		_getCourseTierClient = getCourseTierClient;
		_checkUnitClient = checkUnitClient;
	}


	// GET: /enrollments
	[HttpGet]
	public async Task<ActionResult<IEnumerable<EnrollmentResult>>> GetAll()
	{
		var userId = GetUserId();

		var results = await _context.Enrollments
			.ProjectTo<EnrollmentResult>(_mapper.ConfigurationProvider)
			.Where(x => x.StudentUserId == userId)
			.ToListAsync();

		return results;
	}

	// GET: /enrollments/5
	[HttpGet("{enrollmentId}")]
	public async Task<ActionResult<EnrollmentDetailResult>> Get(
		int enrollmentId)
	{
		var userId = GetUserId();

		var result = await _context.Enrollments
			.ProjectTo<EnrollmentDetailResult>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(x =>
				x.EnrollmentId == enrollmentId &&
				x.StudentUserId == userId);
		if (result == null) return NotFound($"EnrollmentId: {enrollmentId} is not existed.");

		return result;
	}


	// POST: /enrollments
	[HttpPost]
	public async Task<ActionResult<EnrollmentResult>> Create(
		[FromBody] int courseId)
	{
		var userId = GetUserId();

		var enrollments = await _context.Enrollments
			.Where(x => x.StudentUserId == userId)
			.Select(x => new { x.CourseId, x.Completed })
			.ToListAsync();
		if (enrollments.Any(x => x.CourseId == courseId))
			return Conflict($"Already enrolled this course ({courseId}).");

		if (enrollments.Count(x => x.Completed != null) >= 3)
			return Forbid("Cannot enroll more than 3 courses.");

		var request = new GetCourseTier()
		{
			Id = courseId,
			IsApproved = true
		};
		var response = await _getCourseTierClient
			.GetResponse<CourseTierResult, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}
		if (!response.Is(out Response<CourseTierResult>? courseTierResult) ||
			courseTierResult == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		if (courseTierResult.Message.Tier == CourseTier.Premium &&
			!User.IsInRole(nameof(RoleTypes.Student)))
		{
			return Forbid("User is not Student.");
		}

		var enrollment = new Enrollment()
		{
			CourseId = courseId,
			StudentUserId = userId
		};
		_context.Enrollments.Add(enrollment);

		await _context.SaveChangesAsync();

		var result = _mapper.Map<EnrollmentResult>(enrollment);

		return CreatedAtAction(nameof(Get), new { enrollment.EnrollmentId }, result);
	}


	// DELETE: /enrollments/5
	[HttpDelete("{enrollmentId}")]
	[Authorize(Roles = nameof(RoleTypes.Student))]
	public async Task<ActionResult> Delete(
		int enrollmentId)
	{
		var userId = GetUserId();

		var affected = await _context.Enrollments
			.Where(x =>
				x.EnrollmentId == enrollmentId &&
				x.StudentUserId == userId)
			.ExecuteDeleteAsync();
		if (affected == 0) return NotFound();

		return NoContent();
	}


	// POST: /enrollments/material
	[HttpPost("material")]
	public async Task<ActionResult> CompleteMaterial(
		[FromBody] CompleteMaterial body)
	{
		var userId = GetUserId();

		var enrollment = await _context.Enrollments
			.AsNoTracking()
			.Include(x => x.CompletedUnits)
			.FirstOrDefaultAsync(x =>
				x.EnrollmentId == body.EnrollmentId &&
				x.StudentUserId == userId);
		if (enrollment == null)
			return NotFound("Enrollment is not existed.");
		if (enrollment.CompletedUnits.Any(x => x.UnitId == body.UnitId))
			return Conflict($"UnitId: {body.UnitId} existed.");

		var request = new CheckUnit()
		{
			UnitId = body.UnitId,
			IsExam = false
		};
		var response = await _checkUnitClient
			.GetResponse<Existed, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		var completedUnit = new CompletedUnit()
		{
			UnitId = body.UnitId,
			EnrollmentId = body.EnrollmentId
		};
		_context.CompletedUnits.Add(completedUnit);

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Get), new { body.EnrollmentId });
	}


	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}
}
