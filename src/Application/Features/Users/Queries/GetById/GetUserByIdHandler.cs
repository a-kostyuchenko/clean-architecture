using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users.Errors;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Result;

namespace Application.Features.Users.Queries.GetById;

internal sealed class GetUserByIdHandler(IApplicationDbContext context)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        UserResponse? user = await context.Users
            .Where(u => u.Id == query.UserId)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        }

        return user;
    }
}