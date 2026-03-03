using System;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Jobs.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Jobs.Infrastructure.Tests.Services
{
    [TestFixture]
    public class JwtTokenGeneratorTests
    {
        private JwtOptions _options;
        private JwtTokenGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _options = new JwtOptions
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                SecretKey = "super_secret_key_that_is_long_enough_for_hmac_sha256",
                ExpiryMinutes = 30
            };

            var optionsMonitor = Options.Create(_options);
            _generator = new JwtTokenGenerator(optionsMonitor);
        }

        [Test]
        public void GenerateToken_ValidInputs_ReturnsValidJwtString()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "test@example.com";

            // Act
            var token = _generator.GenerateToken(userId, email);

            // Assert
            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
            
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            Assert.That(jwtToken.Issuer, Is.EqualTo(_options.Issuer));
            Assert.That(jwtToken.Audiences.First(), Is.EqualTo(_options.Audience));
            
            var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            Assert.That(subClaim, Is.Not.Null);
            Assert.That(subClaim.Value, Is.EqualTo(userId.ToString()));
            
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            Assert.That(emailClaim, Is.Not.Null);
            Assert.That(emailClaim.Value, Is.EqualTo(email));
        }

        [Test]
        public void GenerateToken_WithAdditionalClaims_IncludesClaimsInToken()
        {
            var userId = Guid.NewGuid();
            var email = "test@example.com";
            var additionalClaims = new[] { new Claim("Role", "Admin") };

            var token = _generator.GenerateToken(userId, email, additionalClaims);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Role");
            Assert.That(roleClaim, Is.Not.Null);
            Assert.That(roleClaim.Value, Is.EqualTo("Admin"));
        }
    }
}
