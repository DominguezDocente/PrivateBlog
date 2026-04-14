using FluentValidation;
using FluentValidation.Results;
using PrivateBlog.Application.Exceptions;
using System.Reflection;

namespace PrivateBlog.Application.Utilities.Mediator
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            await ValidateRequestAsync(request).ConfigureAwait(false);

            Type useCaseType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            var useCase = _serviceProvider.GetService(useCaseType);

            if (useCase is null)
            {
                throw new MediatorException($"No se encontró un handler para {request.GetType().Name}");
            }

            MethodInfo method = useCaseType.GetMethod("Handle")!;

            return await (Task<TResponse>)method.Invoke(useCase, new object[] { request })!;
        }

        public async Task Send(IRequest request)
        {
            await ValidateRequestAsync(request).ConfigureAwait(false);

            Type useCaseType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());

            var useCase = _serviceProvider.GetService(useCaseType);

            if (useCase is null)
            {
                throw new MediatorException($"No se encontró un handler para {request.GetType().Name}");
            }

            MethodInfo method = useCaseType.GetMethod("Handle")!;

            await (Task)method.Invoke(useCase, new object[] { request })!;
        }

        private async Task ValidateRequestAsync(object request)
        {
            Type requestType = request.GetType();
            Type validatorInterface = typeof(IValidator<>).MakeGenericType(requestType);
            object? validator = _serviceProvider.GetService(validatorInterface);

            if (validator is null)
            {
                return;
            }

            MethodInfo? validateMethod = validatorInterface.GetMethod("ValidateAsync", new[] { requestType, typeof(CancellationToken) });

            if (validateMethod is null)
            {
                return;
            }

            object? validationResult = validateMethod.Invoke(validator, new object[] { request, CancellationToken.None });

            if (validationResult is not Task task)
            {
                return;
            }

            await task.ConfigureAwait(false);

            PropertyInfo? resultProperty = task.GetType().GetProperty("Result");
            if (resultProperty?.GetValue(task) is not ValidationResult result)
            {
                return;
            }

            if (!result.IsValid)
            {
                throw new CustomValidationException(result);
            }
        }
    }
}
