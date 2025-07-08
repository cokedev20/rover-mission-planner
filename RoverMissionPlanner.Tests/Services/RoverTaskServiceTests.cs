using Xunit;
using RoverMissionPlanner.Infrastructure.Services;
using RoverMissionPlanner.Infrastructure.Persistence;
using RoverMissionPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskStatus = RoverMissionPlanner.Domain.Entities.TaskStatus;

namespace RoverMissionPlanner.Tests.Services;

public class RoverTaskServiceTests
{
	private AppDbContext GetDbContext()
	{
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
			.Options;

		return new AppDbContext(options);
	}

	[Fact]
	public async Task AddTaskAsync_Should_Throw_When_Overlapping_Task_Exists()
	{
		// Arrange
		var context = GetDbContext();
		var service = new RoverTaskService(context);

		var existingTask = new RoverTask
		{
			Id = Guid.NewGuid(),
			RoverName = "rover1",
			StartsAt = DateTime.UtcNow.AddHours(1),
			DurationMinutes = 60,
			TaskType = TaskType.Drill,
			Status = TaskStatus.Planned,
			Latitude = 0,
			Longitude = 0
		};

		await context.RoverTasks.AddAsync(existingTask);
		await context.SaveChangesAsync();

		var newTask = new RoverTask
		{
			RoverName = "rover1",
			StartsAt = existingTask.StartsAt.AddMinutes(30), // solapado
			DurationMinutes = 60,
			TaskType = TaskType.Photo,
			Status = TaskStatus.Planned,
			Latitude = 0,
			Longitude = 0
		};

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddTaskAsync(newTask));
	}

	[Fact]
	public async Task AddTaskAsync_Should_Succeed_When_No_Overlap()
	{
		// Arrange
		var context = GetDbContext();
		var service = new RoverTaskService(context);

		var existingTask = new RoverTask
		{
			Id = Guid.NewGuid(),
			RoverName = "rover1",
			StartsAt = DateTime.UtcNow.AddHours(1),
			DurationMinutes = 60,
			TaskType = TaskType.Drill,
			Status = TaskStatus.Planned,
			Latitude = 0,
			Longitude = 0
		};

		await context.RoverTasks.AddAsync(existingTask);
		await context.SaveChangesAsync();

		var newTask = new RoverTask
		{
			RoverName = "rover1",
			StartsAt = existingTask.StartsAt.AddHours(2), 
			DurationMinutes = 60,
			TaskType = TaskType.Photo,
			Status = TaskStatus.Planned,
			Latitude = 0,
			Longitude = 0
		};

		// Act
		await service.AddTaskAsync(newTask);

		// Assert
		Assert.Equal(2, await context.RoverTasks.CountAsync());
	}

	[Fact]
	public async Task AddTaskAsync_Should_Allow_Same_Time_For_Different_Rovers()
	{
		var context = GetDbContext();
		var service = new RoverTaskService(context);

		var time = DateTime.UtcNow.AddHours(1);

		await context.RoverTasks.AddAsync(new RoverTask
		{
			Id = Guid.NewGuid(),
			RoverName = "rover1",
			StartsAt = time,
			DurationMinutes = 60,
			TaskType = TaskType.Drill,
			Status = TaskStatus.Planned,
			Latitude = 0,
			Longitude = 0
		});

		await context.SaveChangesAsync();

		var newTask = new RoverTask
		{
			RoverName = "rover2", // otro rover
			StartsAt = time,
			DurationMinutes = 60,
			TaskType = TaskType.Photo,
			Status = TaskStatus.Planned,
			Latitude = 0,
			Longitude = 0
		};

		await service.AddTaskAsync(newTask);

		Assert.Equal(2, await context.RoverTasks.CountAsync());
	}
}
