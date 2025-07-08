using Microsoft.EntityFrameworkCore;
using RoverMissionPlanner.Application.Interfaces;
using RoverMissionPlanner.Domain.Entities;
using RoverMissionPlanner.Infrastructure.Persistence;

namespace RoverMissionPlanner.Infrastructure.Services
{
	public class RoverTaskService : IRoverTaskService
	{
		private readonly AppDbContext _context;

		public RoverTaskService(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddTaskAsync(RoverTask task)
		{
			var overlapping = await _context.RoverTasks
				.Where(t => t.RoverName == task.RoverName &&
							t.StartsAt < task.StartsAt.AddMinutes(task.DurationMinutes) &&
							t.StartsAt.AddMinutes(t.DurationMinutes) > task.StartsAt)
				.AnyAsync();

			if (overlapping)
				throw new InvalidOperationException("La tarea se solapa con otra existente.");

			task.Id = Guid.NewGuid();
			_context.RoverTasks.Add(task);
			await _context.SaveChangesAsync();
		}


		public async Task<List<RoverTask>> GetTasksByDateAsync(string roverName, DateOnly date)
		{
			var start = date.ToDateTime(TimeOnly.MinValue);
			var end = date.ToDateTime(TimeOnly.MaxValue);

			return await _context.RoverTasks
				.Where(t => t.RoverName == roverName && t.StartsAt >= start && t.StartsAt <= end)
				.OrderBy(t => t.StartsAt)
				.ToListAsync();
		}

		public async Task<double> GetUtilizationPercentageAsync(string roverName, DateOnly date)
		{
			var tasks = await GetTasksByDateAsync(roverName, date);
			var totalMinutes = tasks.Sum(t => t.DurationMinutes);
			return (double)totalMinutes / (24 * 60) * 100;
		}

		public async Task<List<string>> GetDistinctRoverNamesAsync()
		{
			return await _context.RoverTasks
				.Select(t => t.RoverName)
				.Distinct()
				.OrderBy(name => name)
				.ToListAsync();
		}
	}

}