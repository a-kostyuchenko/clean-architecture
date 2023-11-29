using Domain.Enumerations;
using Microsoft.AspNetCore.Builder;

namespace Presentation.Extensions;

public static class EndpointExtensions
{
    public static TBuilder RequirePermission<TBuilder>(this TBuilder builder, Permission permission) 
        where TBuilder : IEndpointConventionBuilder
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        ArgumentNullException.ThrowIfNull(permission);

        return builder.RequireAuthorization(permission.ToString());
    }
}