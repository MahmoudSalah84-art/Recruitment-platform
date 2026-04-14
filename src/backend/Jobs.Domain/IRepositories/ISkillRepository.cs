using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepositories
{
	public interface ISkillRepository : IRepository<Skill>
	{

		Task<Skill?> GetByIdWithRelationsAsync(string id, CancellationToken cancellationToken);
		
	}
}
