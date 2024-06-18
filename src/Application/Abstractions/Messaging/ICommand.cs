using MediatR;
using SharedKernel;
using SharedKernel.Result;

namespace Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;