using System;
using System.Collections.Generic;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.Enums;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Common;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class JobTests
    {
        private SalaryRange _validSalary;
        private Guid _companyId;
        private Guid _hrId;

        [SetUp]
        public void Setup()
        {
            _validSalary = SalaryRange.Create(40000, 60000);
            _companyId = Guid.NewGuid();
            _hrId = Guid.NewGuid();
        }

        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Arrange
            string title = "Software Engineer";
            string description = "Develop awesome software";
            string requirements = ".NET, C#";
            int expLevel = 2; // Years

            // Act
            var job = new Job(_companyId, _hrId, title, _validSalary, description, requirements, expLevel);

            // Assert
            Assert.That(job.CompanyId, Is.EqualTo(_companyId));
            Assert.That(job.HrId, Is.EqualTo(_hrId));
            Assert.That(job.Title, Is.EqualTo(title));
            Assert.That(job.Salary, Is.EqualTo(_validSalary));
            Assert.That(job.Description, Is.EqualTo(description));
            Assert.That(job.Requirements, Is.EqualTo(requirements));
            Assert.That(job.ExperienceLevel, Is.EqualTo(expLevel));
            Assert.That(job.IsPublished, Is.False);
            Assert.That(job.IsExpired, Is.False);
            Assert.That(job.IsDeleted, Is.False);
        }

        [Test]
        public void Publish_ShouldSetIsPublishedTrue()
        {
            // Arrange
            var job = new Job(_companyId, _hrId, "Title", _validSalary, "Desc", "Req", 1);

            // Act
            job.Publish();

            // Assert
            Assert.That(job.IsPublished, Is.True);
        }

        [Test]
        public void UpdateDetails_ShouldUpdateProperties_WhenValid()
        {
            // Arrange
            var job = new Job(_companyId, _hrId, "Old Title", _validSalary, "Old Desc", "Old Req", 1);
            string newTitle = "New Title";
            string newDesc = "New Desc";
            string newReq = "New Req";
            int newExp = 5;
            SalaryRange newSalary = SalaryRange.Create(80000, 100000);

            // Act
            job.UpdateDetails(newTitle, newDesc, newReq, newExp, newSalary);

            // Assert
            Assert.That(job.Title, Is.EqualTo(newTitle));
            Assert.That(job.Description, Is.EqualTo(newDesc));
            Assert.That(job.Requirements, Is.EqualTo(newReq));
            Assert.That(job.ExperienceLevel, Is.EqualTo(newExp));
            Assert.That(job.Salary, Is.EqualTo(newSalary));
        }

        [Test]
        public void AddRequiredSkill_ShouldAddSkill_WhenValid()
        {
            // Arrange
            var job = new Job(_companyId, _hrId, "Title", _validSalary, "Desc", "Req", 1);
            
            // JobSkill constructor: JobSkill(Guid jobId , Guid skillId)
            var skill = new JobSkill(job.Id, Guid.NewGuid());

            // Act
            job.AddRequiredSkill(skill);
            
            // Assert
            Assert.That(job.RequiredSkills, Does.Contain(skill));
        }
    }
}
