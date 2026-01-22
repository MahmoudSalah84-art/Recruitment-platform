//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Jobs.Application.Features.Users.Commands.RegisterUser
//{
//	public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
//	{
//		public RegisterUserValidator()
//		{
//			RuleFor(x => x.Email).EmailAddress();
//			RuleFor(x => x.Password).MinimumLength(8);
//			RuleFor(x => x.FullName).NotEmpty();
//		}
//	}
//}
