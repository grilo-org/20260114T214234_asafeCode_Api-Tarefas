using Bogus;
using CommonTestUtilities.Extensions;
using CommonTestUtilities.ValueObjects;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Extensions;

namespace CommonTestUtilities.Requests;

public static class RequestTaskJsonBuilder
{
    public static RequestTaskJson Build(int weeklyGoal = 1, int descriptionChar = 20, DayOfWeek targetDay = DayOfWeek.Monday) => new Faker<RequestTaskJson>()
        .RuleFor(t => t.Title, f => f.Random.Word())
        .RuleFor(t => t.Description, f => f.Random.String(descriptionChar))
        .RuleFor(t => t.Category, f => f.Random.Word())
        .RuleFor(t => t.WeeklyGoal, f => weeklyGoal)
        .RuleFor(t => t.StartDate, f => TarefasCrudTestsConstants.DateForTests.ToDateOnly().NextWeekday(targetDay));
}