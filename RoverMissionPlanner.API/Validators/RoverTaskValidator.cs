using FluentValidation;
using RoverMissionPlanner.Domain.Entities;
using RoverMissionPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RoverMissionPlanner.API.Validators;
public class RoverTaskValidator : AbstractValidator<RoverTask>
{
	private readonly AppDbContext _context;

	public RoverTaskValidator(AppDbContext context)
	{
		_context = context;

		RuleFor(x => x.Latitude)
			.InclusiveBetween(-90, 90)
			.WithMessage("La latitud debe estar entre -90 y 90.");

		RuleFor(x => x.Longitude)
			.InclusiveBetween(-180, 180)
			.WithMessage("La longitud debe estar entre -180 y 180.");

		RuleFor(x => x.StartsAt.Date)
	.GreaterThanOrEqualTo(DateTime.Today)
	.WithMessage("La tarea no puede estar en una fecha anterior a hoy.");

		RuleFor(x => x.DurationMinutes)
			.GreaterThan(0)
			.WithMessage("La duración debe ser mayor a 0 minutos.");

		RuleFor(x => x.DurationMinutes)
			.LessThanOrEqualTo(240)
			.WithMessage("La duración no puede exceder las 4 horas (240 minutos).");

		RuleFor(x => x)
			.MustAsync(NoOverlapWithOtherTasks)
			.WithMessage("El rover ya tiene una tarea en el mismo rango horario.");
	}

	private async Task<bool> NoOverlapWithOtherTasks(RoverTask newTask, CancellationToken ct)
	{
		var newStart = newTask.StartsAt;
		var newEnd = newStart.AddMinutes(newTask.DurationMinutes);

		return !await _context.RoverTasks
			.Where(t => t.RoverName == newTask.RoverName && t.Id != newTask.Id)
			.AnyAsync(t =>
				newStart < t.StartsAt.AddMinutes(t.DurationMinutes) &&
				newEnd > t.StartsAt,
				cancellationToken: ct);
	}
}
