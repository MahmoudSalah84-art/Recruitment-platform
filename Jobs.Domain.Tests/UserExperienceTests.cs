using System;
using System.Reflection;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Common;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class UserExperienceTests
    {
        // Wrapper method to invoke internal constructor using Reflection
        private UserExperience CreateUserExperience(
            Guid userId,
            string jobTitle,
            Guid companyId,
            EmploymentType employmentType,
            DateTime startDate,
            DateTime? endDate,
            bool isCurrent,
            string description)
        {
            var type = typeof(UserExperience);
            var constructor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { typeof(Guid), typeof(string), typeof(Guid), typeof(EmploymentType), typeof(DateTime), typeof(DateTime?), typeof(bool), typeof(string) },
                null);

            if (constructor == null)
            {
                throw new InvalidOperationException("Could not find the internal constructor for UserExperience.");
            }

            return (UserExperience)constructor.Invoke(new object[] { userId, jobTitle, companyId, employmentType, startDate, endDate, isCurrent, description });
        }

        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid companyId = Guid.NewGuid();
            string jobTitle = "Developer";
            EmploymentType employmentType = EmploymentType.FullTime;
            DateTime startDate = DateTime.UtcNow.AddYears(-2);
            DateTime? endDate = null;
            bool isCurrent = true;
            string description = "Worked hard.";

            // Act
            var experience = CreateUserExperience(userId, jobTitle, companyId, employmentType, startDate, endDate, isCurrent, description);

            // Assert
            Assert.That(experience.UserId, Is.EqualTo(userId));
            Assert.That(experience.CompanyId, Is.EqualTo(companyId));
            Assert.That(experience.JobTitle, Is.EqualTo(jobTitle));
            Assert.That(experience.EmploymentType, Is.EqualTo(employmentType));
            Assert.That(experience.StartDate, Is.EqualTo(startDate));
            Assert.That(experience.IsCurrent, Is.EqualTo(isCurrent));
            Assert.That(experience.Description, Is.EqualTo(description));
        }

        [Test]
        public void Update_ShouldUpdateProperties()
        {
            // Arrange
            var experience = CreateUserExperience(Guid.NewGuid(), "Title", Guid.NewGuid(), EmploymentType.PartTime, DateTime.UtcNow.AddYears(-1), null, true, "Desc");
            string newTitle = "Senior Dev";

            // Act
            experience.Update(newTitle, experience.CompanyId, experience.EmploymentType, experience.StartDate, experience.EndDate, experience.IsCurrent, experience.Description);

            // Assert
            Assert.That(experience.JobTitle, Is.EqualTo(newTitle));
        }

        [Test]
        public void MarkAsCurrent_ShouldSetIsCurrentTrueAndClearEndDate()
        {
            // Arrange
            var experience = CreateUserExperience(Guid.NewGuid(), "Title", Guid.NewGuid(), EmploymentType.PartTime, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow, false, "Desc");

            // Act
            experience.MarkAsCurrent();

            // Assert
            Assert.That(experience.IsCurrent, Is.True);
            Assert.That(experience.EndDate, Is.Null);
        }
    }
}
