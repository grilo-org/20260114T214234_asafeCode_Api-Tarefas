using TarefasCrud.Domain.Security.Criptography;
using TarefasCrud.Infrastructure.Security.Criptography;

namespace CommonTestUtilities.Cryptography;

public static class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new BcryptEncripter();
}