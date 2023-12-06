using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Presentation.Contracts;

namespace Presentation.Endpoints;

public class Health : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapHealthChecks(ApiRoutes.Health.Base, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}