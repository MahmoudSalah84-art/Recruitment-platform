

//namespace Jobs.Application.Behaviors
//{
//	public class TransactionBehavior<TRequest, TResponse>
//	{
//		private readonly IUnitOfWork _unitOfWork;

//		public TransactionBehavior(IUnitOfWork unitOfWork)
//		{
//			_unitOfWork = unitOfWork;
//		}

//		public async Task<TResponse> Handle(
//			TRequest request,
//			CancellationToken cancellationToken,
//			Func<Task<TResponse>> next)
//		{
//			await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
//			var response = await next();
//			await _unitOfWork.SaveChangesAsync(cancellationToken);
//			await transaction.CommitAsync(cancellationToken);
//			return response;
//		}
//	}
//}
