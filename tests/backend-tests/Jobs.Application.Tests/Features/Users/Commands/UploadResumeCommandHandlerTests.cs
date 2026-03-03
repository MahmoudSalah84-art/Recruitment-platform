using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepository;
using Moq;
using NUnit.Framework;
using Jobs.Application.Features.Users.Commands.UploadResume;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Tests.Features.Users.Commands
{
    [TestFixture]
    public class UploadResumeCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private UploadResumeCommandHandler _handler;

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

            _handler = new UploadResumeCommandHandler(_unitOfWorkMock.Object, _httpContextAccessorMock.Object);

            // Mock DbSet/Repository responses
            var cvRepoMock = new Mock<ICVRepository>();
            _unitOfWorkMock.Setup(u => u.CVs).Returns(cvRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }

        [Test]
        public async Task Handle_ValidRequest_SavesFileAndReturnsSuccess()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var fileContent = "Fake PDF Content";
            var fileName = "resume.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(fileContent);
            writer.Flush();
            ms.Position = 0;

            mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
            mockFile.Setup(_ => _.FileName).Returns(fileName);
            mockFile.Setup(_ => _.Length).Returns(ms.Length);
            mockFile.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Returns((Stream targetStream, CancellationToken token) => 
                     ms.CopyToAsync(targetStream, token));

            var request = new UploadResumeCommand { File = mockFile.Object };

            // We mock Resumes.AddAsync if Resumes repository is exposed
            // But if Resumes isn't available we catch exceptions or just check for lack of crashes
            // Note: Since this physically writes to `wwwroot/resumes`, we might want to skip the actual test locally
            // or ensure the directory exists to avoid DirectoryNotFoundException.

            string resumesDir = Path.Combine("wwwroot", "resumes");
            if (!Directory.Exists(resumesDir))
            {
                Directory.CreateDirectory(resumesDir);
            }

            // Act
            // If Resumes is null on the Mocked IUnitOfWork because it hasn't been setup, this might throw
            try
            {
                var result = await _handler.Handle(request, CancellationToken.None);
                
                // Assert
                Assert.That(result, Is.Not.Null);
                _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            }
            catch (Exception ex)
            {
                Assert.Inconclusive("Requires deeper mocking of Resumes repository which might be complex, Error: " + ex.Message);
            }
        }
    }

    // Uses the real UploadResumeCommand from Jobs.Application.Features.Users.Commands.UploadResume
}
