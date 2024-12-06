using Application.Features.Authentication.Commands.Login;
using MediatR;
using SharedKernel;
using SharedKernel.Result;
using Web.API.Contracts;
using Web.API.Extensions;
using Web.API.Infrastructure;

namespace Web.API.Endpoints;

public sealed class Authentication : IEndpointGroup
{
    public void MapGroup(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder authentication = app
            .MapGroup(ApiRoutes.Authentication.BaseUri)
            .WithTags(ApiRoutes.Authentication.Tag);

        authentication.MapPost(ApiRoutes.Authentication.Login, async (
            LoginRequest request,
            ISender sender) =>
        {
            var command = new LoginCommand(request.Email, request.Password);

            Result<LoginResponse> result = await sender.Send(command);

            return result.Match(Results.Ok, ApiResults.Problem);
        });
    }
}
