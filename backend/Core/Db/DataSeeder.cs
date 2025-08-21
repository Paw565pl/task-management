using Bogus;
using TaskManagement.Backend.Features.Projects.Entities;
using TaskManagement.Backend.Features.Tasks.Entities;
using TaskStatus = TaskManagement.Backend.Features.Tasks.Entities.TaskStatus;

namespace TaskManagement.Backend.Core.Db;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (dbContext.Projects.Any())
            return;

        var projectFaker = new Faker<ProjectEntity>()
            .RuleFor(x => x.Name, f => $"{f.Company.CatchPhrase()} {f.UniqueIndex}")
            .RuleFor(x => x.Description, f => f.Commerce.ProductDescription());
        var projects = projectFaker.Generate(1000);

        await dbContext.Projects.AddRangeAsync(projects);
        await dbContext.SaveChangesAsync();

        var taskFaker = new Faker<TaskEntity>()
            .RuleFor(x => x.Title, f => f.Hacker.Phrase())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph(5))
            .RuleFor(x => x.Status, f => f.PickRandom<TaskStatus>())
            .RuleFor(x => x.Priority, f => f.PickRandom<TaskPriority>())
            .RuleFor(x => x.DueDate, f => f.Date.FutureDateOnly());

        foreach (var project in projects)
        {
            taskFaker.RuleFor(x => x.ProjectId, project.Id);

            var tasks = taskFaker.GenerateBetween(5, 20);
            await dbContext.Tasks.AddRangeAsync(tasks);
        }

        await dbContext.SaveChangesAsync();
    }
}
