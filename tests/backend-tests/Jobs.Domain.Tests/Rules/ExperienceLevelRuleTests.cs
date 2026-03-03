using NUnit.Framework;
using Jobs.Domain.Rules;

namespace Jobs.Domain.Tests.Rules
{
    [TestFixture]
    public class ExperienceLevelRuleTests
    {
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(40)]
        public void IsBroken_ValidExperienceLevel_ReturnsFalse(int level)
        {
            var rule = new ExperienceLevelRule(level);
            Assert.That(rule.IsBroken(), Is.False);
        }

        [TestCase(-1)]
        [TestCase(41)]
        [TestCase(50)]
        public void IsBroken_InvalidExperienceLevel_ReturnsTrue(int level)
        {
            var rule = new ExperienceLevelRule(level);
            Assert.That(rule.IsBroken(), Is.True);
        }
    }
}
