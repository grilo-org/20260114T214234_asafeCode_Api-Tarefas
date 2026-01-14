using FluentValidation;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Exceptions;

namespace TarefasCrud.Application.UseCases.RoutineTask;

public class TaskValidator : AbstractValidator<RequestTaskJson>
{
    public TaskValidator(DateOnly dateNow, TaskEntity? task = null)
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.TASK_TITLE_EMPTY);

        RuleFor(t => t.Description)
            .MaximumLength(150)
            .WithMessage(ResourceMessagesException.DESCRIPTION_EXCEEDS_LIMIT_CHARACTERS);

        RuleFor(t => t.WeeklyGoal)
            .GreaterThan(0)
            .LessThanOrEqualTo(t => DaysToSunday(t.StartDate))
            .WithMessage(ResourceMessagesException.WEEKLY_GOAL_INVALID);

        RuleFor(t => t.WeeklyGoal)
            .GreaterThanOrEqualTo(_ => task!.Progress)
            .WithMessage(ResourceMessagesException.WEEKLY_GOAL_LOWER_THAN_PROGRESS)
            .When(_ => task is not null);
        
        RuleFor(t => t.StartDate)
            .GreaterThanOrEqualTo(dateNow)
            .WithMessage(ResourceMessagesException.START_DATE_IN_PAST);

        RuleFor(t => t.StartDate)
            .Must(date => date.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
            .WithMessage(ResourceMessagesException.START_DATE_INVALID);
    }

    private static int DaysToSunday(DateOnly startDate)
    {
        var dayOfWeek = startDate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)startDate.DayOfWeek;
        return 7 - dayOfWeek + 1;
    }
}