using System.Transactions;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Behaviors;

internal sealed class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!IsCommand())
        {
            return await next();
        }

        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        var response = await next();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        transactionScope.Complete();

        return response;
    }

    private static bool IsCommand()
    {
        return typeof(TRequest) == typeof(ICommand) ||
               typeof(TRequest) == typeof(ICommand<TResponse>);
    }
}