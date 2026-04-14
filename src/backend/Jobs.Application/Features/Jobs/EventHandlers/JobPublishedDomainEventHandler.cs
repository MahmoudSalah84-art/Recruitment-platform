using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.Emails;
using Jobs.Domain.Enums;
using Jobs.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

//namespace Jobs.Application.Features.Jobs.EventHandlers
//{
//	// بعد نشر Job جديد
//	public class JobPublishedDomainEventHandler
//		: INotificationHandler<JobPublishedDomainEvent>
//	{
//		private readonly IEmailSender _emailSender;
//		private readonly IUserRepository _userRepository;
//		private readonly IJobRepository _jobRepository;

//		public JobPublishedDomainEventHandler(
//			IEmailSender emailSender,
//			IUserRepository userRepository,
//			IJobRepository jobRepository)
//		{
//			_emailSender = emailSender;
//			_userRepository = userRepository;
//			_jobRepository = jobRepository;
//		}

//		public async Task Handle(
//			JobPublishedDomainEvent notification,
//			CancellationToken cancellationToken)
//		{
//			var job = await _jobRepository.GetByIdAsync(notification.JobId, cancellationToken);
//			if (job is null) return;

//			// جيب كل الـ JobSeekers عشان تبعتلهم notification
//			var jobSeekers = await _userRepository
//				.GetAllByRoleAsync(UserRole.JobSeeker, cancellationToken);

//			var emails = jobSeekers.Select(u => _emailSender.SendAsync(
//				new NewJobPostedMessage(
//					To: u.Email,
//					UserName: $"{u.FirstName} {u.LastName}",
//					JobTitle: job.Title,
//					CompanyName: job.Company.Name,
//					JobLink: $"https://yourapp.com/jobs/{job.Id}"),
//				cancellationToken));

//			await Task.WhenAll(emails);
//		}
//	}
//}
