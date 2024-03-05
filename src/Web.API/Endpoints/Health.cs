using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Web.API.Contracts;

namespace Web.API.Endpoints;

public class Health : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapHealthChecks(ApiRoutes.Health.BaseUri, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}