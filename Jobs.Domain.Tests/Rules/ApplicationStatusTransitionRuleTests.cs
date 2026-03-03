using NUnit.Framework;
using Jobs.Domain.Enums;
using Jobs.Domain.Rules.JobApplication;

namespace Jobs.Domain.Tests.Rules
{
    [TestFixture]
    public class ApplicationStatusTransitionRuleTests
    {
        [TestCase(ApplicationStatus.Pending, ApplicationStatus.Accepted, ExpectedResult = false)]
        [TestCase(ApplicationStatus.Pending, ApplicationStatus.Rejected, ExpectedResult = false)]
        public bool IsBroken_ValidTransitions_ReturnsFalse(ApplicationStatus current, ApplicationStatus next)
        {
            var rule = new ApplicationStatusTransitionRule(current, next);
            return rule.IsBroken();
        }

        [TestCase(ApplicationStatus.Pending, ApplicationStatus.Pending, ExpectedResult = true)]
        [TestCase(ApplicationStatus.Accepted, ApplicationStatus.Pending, ExpectedResult = true)]
        [TestCase(ApplicationStatus.Accepted, ApplicationStatus.Rejected, ExpectedResult = true)]
        [TestCase(ApplicationStatus.Rejected, ApplicationStatus.Pending, ExpectedResult = true)]
        [TestCase(ApplicationStatus.Rejected, ApplicationStatus.Accepted, ExpectedResult = true)]
        public bool IsBroken_InvalidTransitions_ReturnsTrue(ApplicationStatus current, ApplicationStatus next)
        {
            var rule = new ApplicationStatusTransitionRule(current, next);
            return rule.IsBroken();
        }
    }
}
