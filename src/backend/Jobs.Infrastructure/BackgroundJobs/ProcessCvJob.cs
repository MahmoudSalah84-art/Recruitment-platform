using Jobs.Application.Abstractions.Interfaces;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;

namespace Jobs.Infrastructure.BackgroundJobs
{
	//public class ProcessCvJob
	//{
	//	private readonly IAiServiceATS _aiService;
	//	private readonly IUnitOfWork _unitOfWork;

	//	public ProcessCvJob(IAiServiceATS aiService, IUnitOfWork unitOfWork)
	//	{
	//		_aiService = aiService;
	//		_unitOfWork = unitOfWork;
	//	}

	//	public async Task Process(string filePath)
	//	{
	//		using var stream = File.OpenRead(filePath);

	//		var result = await _aiService.ParseCvAsync(stream, Path.GetFileName(filePath));

	//		// خزّن البيانات
	//		var cv = new CV.

	//		await _unitOfWork.CVs.AddAsync(user);

	//		foreach (var skill in result.Skills)
	//		{
	//			_context.Skills.Add(new Skill
	//			{
	//				Name = skill,
	//				User = user
	//			});
	//		}

	//		await _context.SaveChangesAsync();
	//	}
	//}
}
