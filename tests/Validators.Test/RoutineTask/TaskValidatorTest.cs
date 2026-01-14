using CommonTestUtilities.Entities;
using CommonTestUtilities.Extensions;
using CommonTestUtilities.Requests;
using CommonTestUtilities.ValueObjects;
using Shouldly;
using TarefasCrud.Application.UseCases.RoutineTask;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Exceptions;

namespace Validators.Test.RoutineTask;

public class TaskValidatorTest
{
    private readonly DateOnly _date = TarefasCrudTestsConstants.DateForTests.ToDateOnly();

    [Fact]
    public void Success()
    {
        var validator = new TaskValidator(_date);
        var request = RequestTaskJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(true);
    }  
    
    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    public void Success_StartDate_Day(DayOfWeek targetDay)
    {
        var validator = new TaskValidator(_date);
        var request = RequestTaskJsonBuilder.Build(targetDay: targetDay);
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(true);
    } 
    
    [Theory]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void Error_StartDate_Day(DayOfWeek targetDay)
    {
        var validator = new TaskValidator(_date);
        var request = RequestTaskJsonBuilder.Build(targetDay: targetDay);
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.START_DATE_INVALID));
    }  
    
    [Fact]
    public void Error_Title_Empty()
    {
        var validator = new TaskValidator(_date);
        var request = RequestTaskJsonBuilder.Build();
        request.Title = string.Empty;
        
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.TASK_TITLE_EMPTY));
    }    
    
    [Fact]
    public void Error_Description_Exceeds_Character_Limit()
    {
        var validator = new TaskValidator(_date);
        var request = RequestTaskJsonBuilder.Build(descriptionChar: 200);
        
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.DESCRIPTION_EXCEEDS_LIMIT_CHARACTERS));
    }    
    
    [Fact]
    public void Error_WeeklyGoal_Invalid()
    {
        var validator = new TaskValidator(_date);
        var request = RequestTaskJsonBuilder.Build(weeklyGoal: 10);
        
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.WEEKLY_GOAL_INVALID));
    }      
    [Fact]
    public void Error_WeeklyGoal_Lower_Than_Progress()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        task.Progress = 2;
        var validator = new TaskValidator(_date, task);
        var request = RequestTaskJsonBuilder.Build(weeklyGoal: 1);
        
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.WEEKLY_GOAL_LOWER_THAN_PROGRESS));
    }    
    
    [Fact]
    public void Error_StartDate_Past()
    {
        var validator = new TaskValidator(_date);
        var request = RequestTaskJsonBuilder.Build();
        request.StartDate = _date.PastWeekday(DayOfWeek.Monday);
        
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.START_DATE_IN_PAST));
    } 
}