using Jobs.Domain.Entities;
using Jobs.Domain.Repository.Repo;
using Jobs.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public SkillRepository(JobDbContext context) : base(context) { }
    }
}
