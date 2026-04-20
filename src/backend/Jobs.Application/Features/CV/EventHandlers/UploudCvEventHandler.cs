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
			await _sender.Send(
				new CreateCVJobRecommendationCommand_cv(notification.Id),
				cancellationToken);
		}
	}
}