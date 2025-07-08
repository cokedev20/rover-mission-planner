using Microsoft.EntityFrameworkCore;
using RoverMissionPlanner.Domain.Entities;
using System.Collections.Generic;

namespace RoverMissionPlanner.Infrastructure.Persistence
{
	public class AppDbContext : DbContext
	{
		public DbSet<RoverTask> RoverTasks => Set<RoverTask>();

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
	}
}
