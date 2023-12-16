using Application.Features.Users.Command.ChangePassword;
using Application.Features.Users.Command.Create;
using Carter;
using Mapster;
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

        users.MapPost(string.Empty, async (
            CreateUserRequest request,
            ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();
            
            var result = await sender.Send(command);
            
            return result.IsSuccess 
                ? Results.Created()
                : Results.BadRequest(result.Error);
        });

        users.MapPut(ApiRoutes.Users.ChangePassword, async (
            Guid userId,
            ChangePasswordRequest request,
            ISender sender) =>
        {
            var command = new ChangePasswordCommand(userId, request.Password);

            var result = await sender.Send(command);
            
            return result.IsSuccess 
                ? Results.NoContent() 
                : Results.BadRequest(result.Error);
        });
    }
}