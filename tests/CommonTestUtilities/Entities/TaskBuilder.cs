using Bogus;
using CommonTestUtilities.Extensions;
using CommonTestUtilities.ValueObjects;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Extensions;

namespace CommonTestUtilities.Entities;

public static class TaskBuilder
{
    public static IList<TaskEntity> Collection(User user, uint count = 2, DayOfWeek targetDay = DayOfWeek.Monday)
    {
        if (count == 0)
            count = 1;

        var list = new List<TaskEntity>();
        var taskId = 1;

        for (var i = 0; i < count; i++)
        {
            var fakeTask = Build(user, targetDay);
            fakeTask.Id = taskId++;

            list.Add(fakeTask);
        }

        return list;
    }
    public static TaskEntity Build(User user, DayOfWeek targetDay = DayOfWeek.Monday)
    {
        var startDate = TarefasCrudTestsConstants.DateForTests.ToDateOnly().NextWeekday(targetDay);
        return new Faker<TaskEntity>()
            .RuleFor(task => task.Title, f => f.Lorem.Word())
            .RuleFor(task => task.Description, f => f.Lorem.Sentence())
            .RuleFor(task => task.WeeklyGoal, 1)
            .RuleFor(task => task.Progress, 0)
            .RuleFor(task => task.Category, f => f.Lorem.Word())
            .RuleFor(task => task.StartDate, _ => startDate)
            .RuleFor(task => task.WeekOfMonth, _ => startDate.GetMonthWeek())
            .RuleFor(task => task.IsCompleted, false)
            .RuleFor(task => task.UserId, _ => user.Id);
    }

}