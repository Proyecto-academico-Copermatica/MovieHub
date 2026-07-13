using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MovieHubAPI.Filters;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType!
            .GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>()
            .Any();

        if (hasAuthorize)
        {
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
                }
            ];
        }
    }
}
