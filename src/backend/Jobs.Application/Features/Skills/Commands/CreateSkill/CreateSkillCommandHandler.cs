using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Skills.Commands.CreateSkill
{
	public class CreateSkillCommandHandler : ICommandHandler<CreateSkillCommand, string>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateSkillCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<string>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
		{
			var exists = await _unitOfWork.Skills.ExistsAsync(x => x.Name == request.Name);

			if (exists)
				return Result<string>.Failure("A skill with this name already exists.");

			var skill =new  Skill(request.Name);

			_unitOfWork.Skills.Add(skill);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<string>.Success(skill.Id);
		}
	}
}
