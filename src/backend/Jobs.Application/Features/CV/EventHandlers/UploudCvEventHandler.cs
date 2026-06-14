using Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation;
using Jobs.Domain.Events.CV_Recommendation_Events;
using MediatR;

namespace Jobs.Application.Features.CV.EventHandlers
{
	public class UploudCvEventHandler : INotificationHandler<CvUploadedEvent>
	{
		private readonly ISender _sender;

		public UploudCvEventHandler(ISender sender)
		{
			_sender = sender;
		}

		public async Task Handle(CvUploadedEvent notification, CancellationToken cancellationToken)
		{
			var result = await _sender.Send(new CreateCVJobRecommendationCommand_cv(notification.Id), cancellationToken);

			if (result.IsFailure)
			{
				// إجبر الـ Outbox Processor إنه يعرف إن فيه مصيبة حصلت عشان ميعلمش عليها كـ True
				throw new Exception($"Failed to process CV recommendations: {result.Error}");
			}
		}
	}
}	