using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.UploadResume
{
	

	public class UploadResumeCommandHandler : IRequestHandler<UploadResumeCommand, Result<string>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;
		// يمكن استخدام خدمة خاصة لرفع الملفات هنا

		public UploadResumeCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result<string>> Handle(UploadResumeCommand request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			// منطق حفظ الملف (مثلاً في مجلد wwwroot أو Cloud Storage)
			var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
			var filePath = Path.Combine("wwwroot/resumes", fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await request.File.CopyToAsync(stream);
			}

			var resume = new UserResume
			{
				UserId = userId,
				FileName = request.File.FileName,
				Url = $"/resumes/{fileName}"
			};

			await _unitOfWork.Resumes.AddAsync(resume);
			await _unitOfWork.CompleteAsync();

			return Result.Success(resume.Url);
		}
	}
}
