using Application.Features.Authentication.Commands.Login;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Contracts;

namespace Presentation.Endpoints;

public sealed class Authentication : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var authentication = app.MapGroup(ApiRoutes.Authentication.Base);

        authentication.MapPost(ApiRoutes.Authentication.Login, async (ISender sender, LoginRequest request) =>
        {
            var command = new LoginCommand(request.Email, request.Password);

            var result = await sender.Send(command);

            return result.IsSuccess 
                ? Results.Ok(result.Value) 
                : Results.BadRequest(result.Error);
        });
    }
}