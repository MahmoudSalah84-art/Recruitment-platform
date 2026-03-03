using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Common;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class UserSkillTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid skillId = Guid.NewGuid();

            // Act
            var userSkill = new UserSkill(userId, skillId);

            // Assert
            Assert.That(userSkill.UserId, Is.EqualTo(userId));
            Assert.That(userSkill.SkillId, Is.EqualTo(skillId));
            Assert.That(userSkill.IsDeleted, Is.False);
        }

        [Test]
        public void SoftDelete_ShouldSetIsDeletedTrue()
        {
            // Arrange
            var userSkill = new UserSkill(Guid.NewGuid(), Guid.NewGuid());
            ISoftDelete softDeleteSkill = userSkill;

            // Act
            softDeleteSkill.SoftDelete();

            // Assert
            Assert.That(userSkill.IsDeleted, Is.True);
            Assert.That(userSkill.DeletedAt, Is.Not.Null);
        }
    }
}
