using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web.API.Endpoints;
using Permission = SharedKernel.Permission;

namespace Web.API.Extensions;

public static class EndpointExtensions
{
    public static TBuilder RequirePermission<TBuilder>(this TBuilder builder, Permission permission) 
        where TBuilder : IEndpointConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(permission);

        return builder.RequireAuthorization(permission.ToString());
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpointDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpointGroup)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpointGroup), type))
            .ToArray();
    
        services.TryAddEnumerable(endpointDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpointGroup> endpointGroups = app.Services.GetRequiredService<IEnumerable<IEndpointGroup>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpointGroup endpointGroup in endpointGroups)
        {
            endpointGroup.MapGroup(builder);
        }

        return app;
    }
}
