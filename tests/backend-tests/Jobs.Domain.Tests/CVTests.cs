using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Common;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class CVTests
    {
        private Guid _userId;
        private FilePath _filePath;

        [SetUp]
        public void Setup()
        {
            _userId = Guid.NewGuid();
            _filePath = FilePath.Create("path/to/cv.pdf");
        }

        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Arrange
            string title = "Software Engineer CV";

            // Act
            var cv = new CV(_userId, title, _filePath);

            // Assert
            Assert.That(cv.UserId, Is.EqualTo(_userId));
            Assert.That(cv.Title, Is.EqualTo(title));
            Assert.That(cv.FilePath, Is.EqualTo(_filePath));
            Assert.That(cv.IsDeleted, Is.False);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() =>
                new CV(_userId, "", _filePath));
        }

        [Test]
        public void SetParsedData_ShouldUpdateParsedData()
        {
            // Arrange
            var cv = new CV(_userId, "Title", _filePath);
            var parsedData = ParsedData.Create("{\"skills\": \"C#\"}");

            // Act
            cv.SetParsedData(parsedData);

            // Assert
            Assert.That(cv.ParsedData, Is.EqualTo(parsedData));
        }

        [Test]
        public void SetSummary_ShouldUpdateSummaryText()
        {
            // Arrange
            var cv = new CV(_userId, "Title", _filePath);
            string summary = "Experienced developer...";

            // Act
            cv.SetSummary(summary);

            // Assert
            Assert.That(cv.SummaryText, Is.EqualTo(summary));
        }

        [Test]
        public void SoftDelete_ShouldSetIsDeletedTrue()
        {
            // Arrange
            var cv = new CV(_userId, "Title", _filePath);
            ISoftDelete softDeleteCv = cv;

            // Act
            softDeleteCv.SoftDelete();

            // Assert
            Assert.That(cv.IsDeleted, Is.True);
            Assert.That(cv.DeletedAt, Is.Not.Null);
        }
    }
}
