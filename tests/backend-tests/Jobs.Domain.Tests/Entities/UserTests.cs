using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Tests.Entities
{
    [TestFixture]
    public class UserTests
    {
        private Email _validEmail;
        private Func<string, bool> _emailExistsMock;

        [SetUp]
        public void Setup()
        {
            _validEmail = Email.Create("test@example.com");
            _emailExistsMock = email => false; // By default, email doesn't exist
        }

        [Test]
        public void Constructor_ValidInputs_CreatesUser()
        {
            // Act
            var user = new User("John Doe", _validEmail, UserRole.Applicant, _emailExistsMock);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(user.FullName, Is.EqualTo("John Doe"));
                Assert.That(user.Email.Value, Is.EqualTo("test@example.com"));
                Assert.That(user.Role, Is.EqualTo(UserRole.Applicant));
                Assert.That(user.IsVerified, Is.False);
            });
        }

        [Test]
        public void Constructor_InvalidEmail_ThrowsBusinessRuleViolationException()
        {
            // Arrange
            Func<string, bool> emailExists = email => true;

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => 
                new User("John Doe", _validEmail, UserRole.Applicant, emailExists));
        }

        [Test]
        public void Constructor_EmptyName_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() => 
                new User("", _validEmail, UserRole.Applicant, _emailExistsMock));
        }

        [Test]
        public void Verify_WhenNotVerified_SetsIsVerifiedToTrue()
        {
            // Arrange
            var user = new User("John Doe", _validEmail, UserRole.Applicant, _emailExistsMock);

            // Act
            user.Verify();

            // Assert
            Assert.That(user.IsVerified, Is.True);
        }

        [Test]
        public void AddSkill_ValidSkill_AddsToCollection()
        {
            // Arrange
            var user = new User("John Doe", _validEmail, UserRole.Applicant, _emailExistsMock);
            var skill = new UserSkill(user.Id, Guid.NewGuid());

            // Act
            user.AddSkill(skill);

            // Assert
            Assert.That(user.Skills, Contains.Item(skill));
        }

        [Test]
        public void AddSkill_DuplicateSkill_DoesNotAddTwice()
        {
            // Arrange
            var user = new User("John Doe", _validEmail, UserRole.Applicant, _emailExistsMock);
            var skillId = Guid.NewGuid();
            var skill = new UserSkill(user.Id, skillId);
            
            user.AddSkill(skill);

            // Act
            user.AddSkill(new UserSkill(user.Id, skillId));

            // Assert
            Assert.That(user.Skills.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddSkill_NullSkill_ThrowsBusinessRuleViolationException()
        {
            var user = new User("John Doe", _validEmail, UserRole.Applicant, _emailExistsMock);
            Assert.Throws<BusinessRuleViolationException>(() => user.AddSkill(null!));
        }

        [Test]
        public void UpdateProfile_ValidInputs_UpdatesProperties()
        {
            // Arrange
            var user = new User("John Doe", _validEmail, UserRole.Applicant, _emailExistsMock);

            // Act
            user.UpdateProfile("Jane Doe", "jane@example.com", "1234567890", "Software Engineer", "http://image.url");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(user.FullName, Is.EqualTo("Jane Doe"));
                Assert.That(user.Email.Value, Is.EqualTo("jane@example.com"));
                Assert.That(user.Bio, Is.EqualTo("Software Engineer"));
                Assert.That(user.ProfilePictureUrl, Is.EqualTo("http://image.url"));
            });
        }
    }
}
