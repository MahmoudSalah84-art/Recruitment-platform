using NUnit.Framework;
using Jobs.Domain.Rules.JobRoles;

namespace Jobs.Domain.Tests.Rules
{
    [TestFixture]
    public class JobSalaryMustBePositiveRuleTests
    {
        [TestCase(1)]
        [TestCase(100.5)]
        [TestCase(50000)]
        public void IsBroken_PositiveSalary_ReturnsFalse(decimal salary)
        {
            var rule = new JobSalaryMustBePositiveRule(salary);
            Assert.That(rule.IsBroken(), Is.False);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-500.25)]
        public void IsBroken_ZeroOrNegativeSalary_ReturnsTrue(decimal salary)
        {
            var rule = new JobSalaryMustBePositiveRule(salary);
            Assert.That(rule.IsBroken(), Is.True);
        }
    }
}
