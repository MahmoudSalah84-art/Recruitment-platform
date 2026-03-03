using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.JobRoles
{
    public class JobSalaryMustBePositiveRule : IBusinessRule
    {
        private readonly decimal _salary;
        public JobSalaryMustBePositiveRule(decimal salary)
        {
            _salary = salary;
        }

        public bool IsBroken() => _salary <= 0;
        public string Message => "Job salary must be positive.";
    }
}
