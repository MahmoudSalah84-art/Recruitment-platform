using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Rules.JobRoles;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Tests.Rules
{
    [TestFixture]
    public class JobExpirationDateMustBeFutureRuleTests
    {
        [Test]
        public void IsBroken_FutureDate_ReturnsFalse()
        {
            var futureDate = DateTime.UtcNow.AddDays(10);
            var rule = new JobExpirationDateMustBeFutureRule(futureDate);
            
            Assert.That(rule.IsBroken(), Is.False);
        }

        [Test]
        public void IsBroken_PastDate_ReturnsTrue()
        {
            var pastDate = DateTime.UtcNow.AddDays(-1);
            var rule = new JobExpirationDateMustBeFutureRule(pastDate);
            
            Assert.That(rule.IsBroken(), Is.True);
        }

        [Test]
        public void IsBroken_CurrentDate_ReturnsTrue()
        {
            var currentDate = DateTime.UtcNow;
            var rule = new JobExpirationDateMustBeFutureRule(currentDate);
            
            Assert.That(rule.IsBroken(), Is.True);
        }
    }
}
