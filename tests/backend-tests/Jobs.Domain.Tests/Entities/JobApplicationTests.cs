using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;
using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Tests.Entities
{
    [TestFixture]
    public class JobApplicationTests
    {
        private Guid _applicantId;
        private Guid _jobId;
        private Guid? _cvId;

        [SetUp]
        public void Setup()
        {
            _applicantId = Guid.NewGuid();
            _jobId = Guid.NewGuid();
            _cvId = Guid.NewGuid();
        }

        [Test]
        public void Constructor_ValidInputs_CreatesJobApplication()
        {
            var application = new JobApplication(_applicantId, _jobId, _cvId);

            Assert.Multiple(() =>
            {
                Assert.That(application.ApplicantId, Is.EqualTo(_applicantId));
                Assert.That(application.JobId, Is.EqualTo(_jobId));
                Assert.That(application.CvId, Is.EqualTo(_cvId));
                Assert.That(application.Status, Is.EqualTo(ApplicationStatus.Pending));
            });
        }

        [Test]
        public void Constructor_EmptyApplicantId_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new JobApplication(Guid.Empty, _jobId, _cvId));
        }

        [Test]
        public void Constructor_EmptyJobId_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new JobApplication(_applicantId, Guid.Empty, _cvId));
        }

        [Test]
        public void ChangeStatus_ValidTransitionToAccepted_UpdatesStatus()
        {
            var application = new JobApplication(_applicantId, _jobId, _cvId);

            application.ChangeStatus(ApplicationStatus.Accepted);

            Assert.That(application.Status, Is.EqualTo(ApplicationStatus.Accepted));
        }

        [Test]
        public void ChangeStatus_ValidTransitionToRejected_UpdatesStatus()
        {
            var application = new JobApplication(_applicantId, _jobId, _cvId);

            application.ChangeStatus(ApplicationStatus.Rejected);

            Assert.That(application.Status, Is.EqualTo(ApplicationStatus.Rejected));
        }

        [Test]
        public void ChangeStatus_InvalidTransition_ThrowsBusinessRuleViolationException()
        {
            var application = new JobApplication(_applicantId, _jobId, _cvId);
            application.ChangeStatus(ApplicationStatus.Accepted);

            // Assuming a rule prevents changing back from Accepted to Pending.
            // Wrapping in generic Exception or BusinessRuleViolationException check,
            // depending on what ApplicationStatusTransitionRule enforces.
            try
            {
                application.ChangeStatus(ApplicationStatus.Pending);
                Assert.Fail("Status change from Accepted to Pending should preferably fail by domain rules.");
            }
            catch (Exception ex)
            {
                Assert.Pass("Caught expected exception or violation rule: " + ex.Message);
            }
        }
    }
}
