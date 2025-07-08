using RoverMissionPlanner.Domain.Entities;

namespace RoverMissionPlanner.Application.Interfaces
{
	public interface IRoverTaskService
	{

		Task AddTaskAsync(RoverTask task);
		Task<List<RoverTask>> GetTasksByDateAsync(string roverName, DateOnly date);
		Task<double> GetUtilizationPercentageAsync(string roverName, DateOnly date);
		Task<List<string>> GetDistinctRoverNamesAsync();

	}

}
