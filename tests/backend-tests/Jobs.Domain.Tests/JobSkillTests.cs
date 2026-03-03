using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Common;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class JobSkillTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Arrange
            Guid jobId = Guid.NewGuid();
            Guid skillId = Guid.NewGuid();

            // Act
            var jobSkill = new JobSkill(jobId, skillId);

            // Assert
            Assert.That(jobSkill.JobId, Is.EqualTo(jobId));
            Assert.That(jobSkill.SkillId, Is.EqualTo(skillId));
            Assert.That(jobSkill.IsDeleted, Is.False);
        }

        [Test]
        public void SoftDelete_ShouldSetIsDeletedTrue()
        {
            // Arrange
            var jobSkill = new JobSkill(Guid.NewGuid(), Guid.NewGuid());
            ISoftDelete softDeleteSkill = jobSkill;

            // Act
            softDeleteSkill.SoftDelete();

            // Assert
            Assert.That(jobSkill.IsDeleted, Is.True);
            Assert.That(jobSkill.DeletedAt, Is.Not.Null);
        }
    }
}
