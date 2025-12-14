using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Specifications
{
    public class JobRequiresSkillSpec : ISpecification<Job>
    {
        private readonly string _skill;
        public JobRequiresSkillSpec(string skill) => _skill = skill;

        public bool IsSatisfiedBy(Job job) => job.Requirements?.Contains(_skill, StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
