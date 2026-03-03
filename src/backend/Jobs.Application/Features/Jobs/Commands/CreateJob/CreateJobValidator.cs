
//using FluentValidation;

//namespace Jobs.Application.Features.Jobs.Commands.CreateJob
//{
//	public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
//	{
//		public CreateJobCommandValidator()
//		{
//			// التحقق من عنوان الوظيفة
//			RuleFor(x => x.title)
//				.NotEmpty().WithMessage("عنوان الوظيفة مطلوب.")
//				.MinimumLength(5).WithMessage("عنوان الوظيفة يجب ألا يقل عن 5 أحرف.")
//				.MaximumLength(100).WithMessage("عنوان الوظيفة يجب ألا يتجاوز 100 حرف.");

//			// التحقق من وصف الوظيفة
//			RuleFor(x => x.description)
//				.NotEmpty().WithMessage("وصف الوظيفة مطلوب.")
//				.MinimumLength(20).WithMessage("وصف الوظيفة يجب أن يكون مفصلاً (20 حرف على الأقل).");

//			// التحقق من الراتب
//			RuleFor(x => x.salary)
//				.GreaterThan(0).WithMessage("يجب أن يكون الراتب أكبر من صفر.")
//				.LessThan(1000000).WithMessage("الراتب المدخل غير منطقي.");

//			// التحقق من معرف الشركة
//			RuleFor(x => x.companyId)
//				.NotEmpty().WithMessage("يجب تحديد الشركة المعلنة عن الوظيفة.");
//		}
//	}
//}
 