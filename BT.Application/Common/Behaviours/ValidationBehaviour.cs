using FluentValidation;
using Mediator;
using ValidationException = BT.Application.Common.Exceptions.ValidationException;

namespace BT.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger _logger;
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger logger)
    {
        _logger = logger;
        _validators = validators;
    }

    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next(request, cancellationToken);
        
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            _logger.Warning("Validation errors occurred for request {RequestName}: {@Errors}", typeof(TRequest).Name, failures);
            throw new ValidationException(failures);
        }

        return await next(request, cancellationToken);
    }
}