using Moq;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;

namespace CommonTestUtilities.Providers;

public class DateProviderBuilder
{
    private readonly Mock<IDateProvider> _date = new();

    public DateProviderBuilder UseCaseToday(DateTime date)
    {
        _date.Setup(provider => provider.UseCaseDate).Returns(date.ToDateOnly());
        return this;
    }  
    public IDateProvider Build() => _date.Object;
}