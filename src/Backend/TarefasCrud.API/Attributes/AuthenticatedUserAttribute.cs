using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Filters;

namespace TarefasCrud.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter)){}
}