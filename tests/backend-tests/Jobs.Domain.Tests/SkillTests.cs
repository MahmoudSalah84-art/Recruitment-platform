using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Common;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class SkillTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidNameProvided()
        {
            // Arrange
            string name = "C#";

            // Act
            var skill = new Skill(name);

            // Assert
            Assert.That(skill.Name, Is.EqualTo(name));
            Assert.That(skill.IsDeleted, Is.False);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => new Skill(""));
        }

        [Test]
        public void Rename_ShouldUpdateName_WhenValid()
        {
            // Arrange
            var skill = new Skill("OldName");
            string newName = "NewName";

            // Act
            skill.Rename(newName);

            // Assert
            Assert.That(skill.Name, Is.EqualTo(newName));
        }

        [Test]
        public void SoftDelete_ShouldSetIsDeletedTrue()
        {
            // Arrange
            var skill = new Skill("Skill");
            ISoftDelete softDeleteSkill = skill;

            // Act
            softDeleteSkill.SoftDelete();

            // Assert
            Assert.That(skill.IsDeleted, Is.True);
            Assert.That(skill.DeletedAt, Is.Not.Null);
        }
    }
}
