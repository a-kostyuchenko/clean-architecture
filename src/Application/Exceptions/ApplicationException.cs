using SharedKernel;
using SharedKernel.Errors;

namespace Application.Exceptions;

public class ApplicationException(
    string requestName,
    Error? error = default,
    Exception? innerException = default) : Exception("Application exception", innerException)
{
    public string RequestName { get; } = requestName;

    public Error? Error { get; } = error;
}
