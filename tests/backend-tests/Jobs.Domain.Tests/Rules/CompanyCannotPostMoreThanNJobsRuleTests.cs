using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Rules.CompanyRoles;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.Enums;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Tests.Rules
{
    [TestFixture]
    public class CompanyCannotPostMoreThanNJobsRuleTests
    {
        private Company _company;
        private User _hrUser;

        [SetUp]
        public void Setup()
        {
            var address = Address.Create("Country", "City", "Street", "123", "12345");
            _company = new Company("Tech Corp", address, "Desc", "IT", "logo.url");
            _hrUser = new User("HR", Email.Create("hr@example.com"), UserRole.HR, e => false);
        }

        [Test]
        public void IsBroken_JobsUnderLimit_ReturnsFalse()
        {
            // The limit is defaults to 10
            for (int i = 0; i < 9; i++)
            {
                var job = new Job(_company.Id, _hrUser.Id, $"Job {i}", null, "Desc", "Req", 1);
                job.Publish();
                _company.AddJob(job);
            }

            var rule = new CompanyCannotPostMoreThanNJobsRule(_company, 10);
            Assert.That(rule.IsBroken(), Is.False);
        }

        [Test]
        public void IsBroken_JobsAtLimit_ReturnsTrue()
        {
            for (int i = 0; i < 10; i++)
            {
                var job = new Job(_company.Id, _hrUser.Id, $"Job {i}", null, "Desc", "Req", 1);
                job.Publish(); // Makes job active to be counted
                _company.AddJob(job);
            }

            var rule = new CompanyCannotPostMoreThanNJobsRule(_company, 10);
            Assert.That(rule.IsBroken(), Is.True);
        }

        [Test]
        public void IsBroken_CountOnlyActiveJobs_ReturnsFalseIfExpiredOrUnpublished()
        {
            for (int i = 0; i < 10; i++)
            {
                var job = new Job(_company.Id, _hrUser.Id, $"Job {i}", null, "Desc", "Req", 1);
                // UnPublished jobs shouldn't count
                _company.AddJob(job);
            }

            var expiredJob = new Job(_company.Id, _hrUser.Id, "Expired", null, "Desc", "Req", 1, DateTime.UtcNow.AddDays(-1));
            expiredJob.Publish(); 
            _company.AddJob(expiredJob); // Published but expired

            var rule = new CompanyCannotPostMoreThanNJobsRule(_company, 10);
            Assert.That(rule.IsBroken(), Is.False);
        }
    }
}
