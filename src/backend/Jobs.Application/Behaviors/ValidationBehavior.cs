//using FluentValidation;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Jobs.Application.Behaviors
//{
//	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//		where TRequest : IRequest<TResponse>
//	{
//		private readonly IEnumerable<IValidator<TRequest>> _validators;

//		public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

//		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//		{
//			if (!_validators.Any()) return await next();

//			var context = new ValidationContext<TRequest>(request);
//			var validationFailures = _validators
//				.Select(validator => validator.Validate(context))
//				.SelectMany(validationResult => validationResult.Errors)
//				.Where(validationFailure => validationFailure != null)
//				.Select(failure => failure.ErrorMessage)
//				.Distinct()
//				.ToList();

//			if (validationFailures.Any())
//			{
//				// هنا يمكنك تحويلها لـ Result فاشل أو رمي Exception مخصص
//				throw new ValidationException(string.Join(", ", validationFailures));
//			}

//			return await next();
//		}
//	}
//}
