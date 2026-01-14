using CommonTestUtilities.ValueObjects;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;

namespace CommonTestUtilities.Providers;

public class FixedFakeDateForTests : IDateProvider
{ 
    public DateOnly UseCaseDate => TarefasCrudTestsConstants.DateForTests.ToDateOnly();
}