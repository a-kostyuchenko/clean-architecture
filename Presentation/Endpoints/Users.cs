using Application.Features.Users.Command.ChangePassword;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Contracts;

namespace Presentation.Endpoints;

public sealed class Users : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var users = app.MapGroup(ApiRoutes.Users.Base);

        users.MapPut(ApiRoutes.Users.ChangePassword, async (
            Guid userId,
            ChangePasswordRequest request,
            ISender sender) =>
        {
            var command = new ChangePasswordCommand(userId, request.Password);

            var result = await sender.Send(command);
            
            return result.IsSuccess 
                ? Results.Ok() 
                : Results.BadRequest(result.Error);
        });
    }
}