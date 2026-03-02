using System;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Common;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class CVJobRecommendationTests
    {
        private Guid _cvId;
        private Guid _jobId;

        [SetUp]
        public void Setup()
        {
            _cvId = Guid.NewGuid();
            _jobId = Guid.NewGuid();
        }

        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Arrange
            int score = 85;

            // Act
            var recommendation = new CVJobRecommendation(_cvId, _jobId, score);

            // Assert
            Assert.That(recommendation.CvId, Is.EqualTo(_cvId));
            Assert.That(recommendation.JobId, Is.EqualTo(_jobId));
            Assert.That(recommendation.Score, Is.EqualTo(score));
            Assert.That(recommendation.IsActive, Is.True);
            Assert.That(recommendation.IsDeleted, Is.False);
        }

        [Test]
        public void Deactivate_ShouldSetIsActiveFalse()
        {
            // Arrange
            var recommendation = new CVJobRecommendation(_cvId, _jobId, 85);

            // Act
            recommendation.Deactivate();

            // Assert
            Assert.That(recommendation.IsActive, Is.False);
            Assert.That(recommendation.DeactivatedAt, Is.Not.Null);
        }

        [Test]
        public void SoftDelete_ShouldSetIsDeletedTrue()
        {
            // Arrange
            var recommendation = new CVJobRecommendation(_cvId, _jobId, 85);
            ISoftDelete softDeleteRec = recommendation;

            // Act
            softDeleteRec.SoftDelete();

            // Assert
            Assert.That(recommendation.IsDeleted, Is.True);
            Assert.That(recommendation.DeletedAt, Is.Not.Null);
        }
    }
}
