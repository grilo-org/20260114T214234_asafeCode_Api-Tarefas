using TarefasCrud.Domain.Enums;

namespace TarefasCrud.Domain.Extensions;

public static class ProgressOperationExtension
{
    public static int ToInt(this ProgressOperation enumValue) => (int)enumValue;
}