using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.Emails;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

//namespace Jobs.Application.Features.Applications.EventHandler
//{
//	// بعد تغيير حالة الـ Application
//	public class JobApplicationStatusChangedDomainEventHandler
//		: INotificationHandler<JobApplicationAcceptedDomainEvent>,
//		  INotificationHandler<JobApplicationRejectedDomainEvent>
//	{
//		private readonly IEmailSender _emailSender;
//		private readonly IJobApplicationRepository _applicationRepository;

//		public JobApplicationStatusChangedDomainEventHandler(
//			IEmailSender emailSender,
//			IJobApplicationRepository applicationRepository)
//		{
//			_emailSender = emailSender;
//			_applicationRepository = applicationRepository;
//		}

//		public async Task Handle(
//			JobApplicationAcceptedDomainEvent notification,
//			CancellationToken cancellationToken) =>
//			await SendStatusEmail(notification.ApplicationId, "Accepted", null, cancellationToken);

//		public async Task Handle(
//			JobApplicationRejectedDomainEvent notification,
//			CancellationToken cancellationToken) =>
//			await SendStatusEmail(notification.ApplicationId, "Rejected", null, cancellationToken);

//		private async Task SendStatusEmail(
//			string applicationId,
//			string status,
//			string? note,
//			CancellationToken cancellationToken)
//		{
//			var application = await _applicationRepository
//				.GetByIdWithDetailsAsync(applicationId, cancellationToken);

//			if (application is null) return;

//			await _emailSender.SendAsync(new ApplicationStatusChangedMessage(
//				To: application.Applicant.Email,
//				UserName: $"{application.Applicant.FirstName} {application.Applicant.LastName}",
//				JobTitle: application.Job.Title,
//				CompanyName: application.Job.Company.Name,
//				Status: status,
//				Note: note),
//				cancellationToken);
//		}
//	}
//}
