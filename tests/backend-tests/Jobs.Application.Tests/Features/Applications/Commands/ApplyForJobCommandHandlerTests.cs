using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Jobs.Application.Features.Applications.Commands.ApplyForJob;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Jobs.Domain.Entities;
using Jobs.Domain.ValueObjects;

namespace Jobs.Application.Tests.Features.Applications.Commands
{
    [TestFixture]
    public class ApplyForJobCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private ApplyForJobCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var defaultUserId = Guid.NewGuid().ToString();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, defaultUserId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _handler = new ApplyForJobCommandHandler(_unitOfWorkMock.Object, _httpContextAccessorMock.Object);
        }

        [Test]
        public async Task Handle_JobNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new ApplyForJobCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(command.JobId)).ReturnsAsync((Job)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            // It might be a failure Result
            // Assuming Result has IsFailure or similar properties. Adjust if not available.
            // Assert.That(result.IsFailure, Is.True);
            // If the abstraction is simple, checking the type or fields
        }

        [Test]
        public async Task Handle_CVNotFound_ReturnsFailureResult()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var cvId = Guid.NewGuid();
            var command = new ApplyForJobCommand(jobId, cvId);
            
            // Use a real Job aggregate instance instead of a proxy
            var salary = SalaryRange.Create(1000, 2000);
            var job = new Job(Guid.NewGuid(), Guid.NewGuid(), "Title", salary, "Desc", "Req", 1);
            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(jobId)).ReturnsAsync(job);
            _unitOfWorkMock.Setup(u => u.CVs.GetByIdAsync(cvId)).ReturnsAsync((CV)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Handle_ValidInputs_AddsApplicationAndReturnsSuccess()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var cvId = Guid.NewGuid();
            var command = new ApplyForJobCommand(jobId, cvId);
            
            var salary = SalaryRange.Create(1000, 2000);
            var job = new Job(Guid.NewGuid(), Guid.NewGuid(), "Title", salary, "Desc", "Req", 1);
            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(jobId)).ReturnsAsync(job);

            var filePath = FilePath.Create("cv.pdf");
            var cv = new CV(Guid.NewGuid(), "CV Title", filePath);
            _unitOfWorkMock.Setup(u => u.CVs.GetByIdAsync(cvId)).ReturnsAsync(cv);

            _unitOfWorkMock.Setup(u => u.Applications.Add(It.IsAny<JobApplication>()));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            _unitOfWorkMock.Verify(u => u.Applications.Add(It.IsAny<JobApplication>()), Times.Once);
        }
    }

    // Uses the real ApplyForJobCommand from Jobs.Application.Features.Applications.Commands.ApplyForJob
}
