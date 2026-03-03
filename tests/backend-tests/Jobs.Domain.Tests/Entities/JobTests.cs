using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Exceptions;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Tests.Entities
{
    [TestFixture]
    public class JobTests
    {
        private Guid _companyId;
        private Guid _hrId;

        [SetUp]
        public void Setup()
        {
            _companyId = Guid.NewGuid();
            _hrId = Guid.NewGuid();
        }

        [Test]
        public void Constructor_ValidInputs_CreatesJob()
        {
            var job = new Job(_companyId, _hrId, "Software Engineer", null, "Desc", "Reqs", 3, DateTime.UtcNow.AddDays(10));

            Assert.Multiple(() =>
            {
                Assert.That(job.CompanyId, Is.EqualTo(_companyId));
                Assert.That(job.HrId, Is.EqualTo(_hrId));
                Assert.That(job.Title, Is.EqualTo("Software Engineer"));
                Assert.That(job.ExperienceLevel, Is.EqualTo(3));
                Assert.That(job.IsPublished, Is.False);
            });
        }

        [Test]
        public void Constructor_EmptyCompanyId_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new Job(Guid.Empty, _hrId, "Title", null, "D", "R", 3));
        }

        [Test]
        public void Constructor_EmptyHrId_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new Job(_companyId, Guid.Empty, "Title", null, "D", "R", 3));
        }

        [Test]
        public void Constructor_EmptyTitle_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new Job(_companyId, _hrId, "", null, "D", "R", 3));
        }

        [Test]
        public void Publish_WhenNotPublished_SetsIsPublishedToTrue()
        {
            var job = new Job(_companyId, _hrId, "Title", null, "D", "R", 3);

            job.Publish();

            Assert.That(job.IsPublished, Is.True);
        }

        [Test]
        public void UpdateDetails_ValidInputs_UpdatesProperties()
        {
            var job = new Job(_companyId, _hrId, "Title", null, "D", "R", 3);

            job.UpdateDetails("New Title", "New Desc", "New Reqs", 5, null);

            Assert.Multiple(() =>
            {
                Assert.That(job.Title, Is.EqualTo("New Title"));
                Assert.That(job.Description, Is.EqualTo("New Desc"));
                Assert.That(job.Requirements, Is.EqualTo("New Reqs"));
                Assert.That(job.ExperienceLevel, Is.EqualTo(5));
            });
        }

        [Test]
        public void UpdateDetails_EmptyTitle_ThrowsBusinessRuleViolationException()
        {
            var job = new Job(_companyId, _hrId, "Title", null, "D", "R", 3);

            Assert.Throws<BusinessRuleViolationException>(() =>
                job.UpdateDetails("", "D", "R", 5, null));
        }

        [Test]
        public void AddRequiredSkill_ValidSkill_AddsToCollection()
        {
            var job = new Job(_companyId, _hrId, "Title", null, "D", "R", 3);
            var skill = new JobSkill(job.Id, Guid.NewGuid());

            job.AddRequiredSkill(skill);

            Assert.That(job.RequiredSkills, Contains.Item(skill));
        }

        [Test]
        public void AddRequiredSkill_NullSkill_ThrowsBusinessRuleViolationException()
        {
            var job = new Job(_companyId, _hrId, "Title", null, "D", "R", 3);

            Assert.Throws<BusinessRuleViolationException>(() => job.AddRequiredSkill(null!));
        }
    }
}
