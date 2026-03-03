using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;
using Jobs.Domain.Common;
using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class JobApplicationTests
    {
        private Guid _applicantId;
        private Guid _jobId;
        private Guid _cvId;

        [SetUp]
        public void Setup()
        {
            _applicantId = Guid.NewGuid();
            _jobId = Guid.NewGuid();
            _cvId = Guid.NewGuid();
        }

        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Act
            var application = new JobApplication(_applicantId, _jobId, _cvId);

            // Assert
            Assert.That(application.ApplicantId, Is.EqualTo(_applicantId));
            Assert.That(application.JobId, Is.EqualTo(_jobId));
            Assert.That(application.CvId, Is.EqualTo(_cvId));
            Assert.That(application.Status, Is.EqualTo(ApplicationStatus.Pending));
            Assert.That(application.IsDeleted, Is.False);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenApplicantIdIsEmpty()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() =>
                new JobApplication(Guid.Empty, _jobId, _cvId));
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenJobIdIsEmpty()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() =>
                new JobApplication(_applicantId, Guid.Empty, _cvId));
        }

        [Test]
        public void ChangeStatus_ShouldUpdateStatusAndHistory_WhenValidTransition()
        {
            // Arrange
            var application = new JobApplication(_applicantId, _jobId, _cvId);
            var initialStatus = application.Status; // Pending
            var newStatus = ApplicationStatus.Accepted;

            // Act
            application.ChangeStatus(newStatus);

            // Assert
            Assert.That(application.Status, Is.EqualTo(newStatus));
            Assert.That(application.StatusHistory, Is.Not.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void SoftDelete_ShouldSetIsDeletedTrue()
        {
            // Arrange
            var application = new JobApplication(_applicantId, _jobId, _cvId);
            ISoftDelete softDeleteApp = application;

            // Act
            softDeleteApp.SoftDelete();

            // Assert
            Assert.That(application.IsDeleted, Is.True);
            Assert.That(application.DeletedAt, Is.Not.Null);
        }
    }
}
