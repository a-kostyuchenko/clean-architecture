using Application.Features.Users.Command.ChangePassword;
using Application.Features.Users.Command.Create;
using Application.Features.Users.Queries.GetById;
using Domain.Users;
using Domain.Users.Permissions;
using Mapster;
using MediatR;
using SharedKernel;
using SharedKernel.Result;
using Web.API.Contracts;
using Web.API.Extensions;
using Web.API.Infrastructure;

namespace Web.API.Endpoints;

public sealed class Users : IEndpointGroup
{
    public void MapGroup(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder users = app
            .MapGroup(ApiRoutes.Users.BaseUri)
            .WithTags(ApiRoutes.Users.Tag);

        users.MapGet(ApiRoutes.Users.GetById, async (
            Guid userId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserByIdQuery(userId);
            
            Result<UserResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequirePermission(UserPermission.Read);

        users.MapPost(string.Empty, async (
            CreateUserRequest request,
            ISender sender) =>
        {
            CreateUserCommand command = request.Adapt<CreateUserCommand>();
            
            Result<Guid> result = await sender.Send(command);
            
            return result.Match(Results.Created, ApiResults.Problem);
        });

        users.MapPut(ApiRoutes.Users.ChangePassword, async (
            Guid userId,
            ChangePasswordRequest request,
            ISender sender) =>
        {
            var command = new ChangePasswordCommand(userId, request.Password);

            Result result = await sender.Send(command);
            
            return result.Match(Results.NoContent, ApiResults.Problem);
        });
    }
}
