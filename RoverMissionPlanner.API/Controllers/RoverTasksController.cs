using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using RoverMissionPlanner.Application.Interfaces;
using RoverMissionPlanner.Domain.Entities;

namespace RoverMissionPlanner.API.Controllers;

[ApiController]
[Route("api/rovers/{roverName}/tasks")]
public class RoverTasksController : ControllerBase
{
	private readonly IRoverTaskService _service;

	public RoverTasksController(IRoverTaskService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<IActionResult> CreateTask(
	string roverName,
	[FromBody] RoverTask task,
	[FromServices] IValidator<RoverTask> validator)
	{
		var validation = await validator.ValidateAsync(task);
		if (!validation.IsValid)
			return BadRequest(validation.Errors);

		try
		{
			task.RoverName = roverName;
			await _service.AddTaskAsync(task);
			return CreatedAtAction(nameof(GetTasks), new { roverName, date = task.StartsAt.Date }, task);
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetTasks(string roverName, [FromQuery] DateOnly date)
	{
		var result = await _service.GetTasksByDateAsync(roverName, date);
		return Ok(result);
	}

	[HttpGet("/api/rovers/{roverName}/utilization")]
	public async Task<IActionResult> GetUtilization(string roverName, [FromQuery] DateOnly date)
	{
		var percent = await _service.GetUtilizationPercentageAsync(roverName, date);
		return Ok(new { utilization = Math.Round(percent, 2) });
	}

	[HttpGet("/api/rovers")]
	public async Task<IActionResult> GetAllRovers()
	{
		var rovers = await _service.GetDistinctRoverNamesAsync();
		return Ok(rovers);
	}
}
