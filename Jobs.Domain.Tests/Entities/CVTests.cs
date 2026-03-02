using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Exceptions;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Tests.Entities
{
    [TestFixture]
    public class CVTests
    {
        private Guid _userId;
        private FilePath _validFilePath;

        [SetUp]
        public void Setup()
        {
            _userId = Guid.NewGuid();
            _validFilePath = FilePath.Create("/path/to/cv.pdf");
        }

        [Test]
        public void Constructor_ValidInputs_CreatesCV()
        {
            var cv = new CV(_userId, "My Resume", _validFilePath);

            Assert.Multiple(() =>
            {
                Assert.That(cv.UserId, Is.EqualTo(_userId));
                Assert.That(cv.Title, Is.EqualTo("My Resume"));
                Assert.That(cv.FilePath, Is.EqualTo(_validFilePath));
                Assert.That(cv.ParsedData, Is.Null);
            });
        }

        [Test]
        public void Constructor_EmptyUserId_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new CV(Guid.Empty, "My Resume", _validFilePath));
        }

        [Test]
        public void Constructor_EmptyTitle_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new CV(_userId, "", _validFilePath));
        }

        [Test]
        public void Constructor_NullFilePath_ThrowsBusinessRuleViolationException()
        {
            Assert.Throws<BusinessRuleViolationException>(() =>
                new CV(_userId, "My Resume", null));
        }

        [Test]
        public void SetParsedData_ValidData_SetsProperty()
        {
            var cv = new CV(_userId, "My Resume", _validFilePath);
            var data = ParsedData.Create("{\"skills\":[\"C#\"]}");

            cv.SetParsedData(data);

            Assert.That(cv.ParsedData, Is.EqualTo(data));
        }

        [Test]
        public void SetParsedData_NullData_ThrowsBusinessRuleViolationException()
        {
            var cv = new CV(_userId, "My Resume", _validFilePath);

            Assert.Throws<BusinessRuleViolationException>(() => cv.SetParsedData(null));
        }

        [Test]
        public void SetSummary_UpdatesSummaryProperty()
        {
            var cv = new CV(_userId, "My Resume", _validFilePath);

            cv.SetSummary("Experienced .NET Developer.");

            Assert.That(cv.SummaryText, Is.EqualTo("Experienced .NET Developer."));
        }
    }
}
