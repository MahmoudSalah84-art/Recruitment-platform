using System;
using System.Collections.Generic;
using NUnit.Framework;
using Jobs.Domain.Entities;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.Enums;
using Jobs.Domain.Common;
using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Tests
{
    [TestFixture]
    public class CompanyTests
    {
        private Address _validAddress;

        [SetUp]
        public void Setup()
        {
            _validAddress = Address.Create("USA", "New York", "5th Avenue", "101", "10001");
        }

        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsProvided()
        {
            // Arrange
            string name = "TechCorp";
            string description = "A leading tech company";
            string industry = "Technology";
            string logoUrl = "https://example.com/logo.png";

            // Act
            var company = new Company(name, _validAddress, description, industry, logoUrl);

            // Assert
            Assert.That(company.Name, Is.EqualTo(name));
            Assert.That(company.CompanyAddress, Is.EqualTo(_validAddress));
            Assert.That(company.Description, Is.EqualTo(description));
            Assert.That(company.Industry, Is.EqualTo(industry));
            Assert.That(company.LogoUrl, Is.EqualTo(logoUrl));
            Assert.That(company.Employees, Is.Not.Null);
            Assert.That(company.Employees, Is.Empty);
            Assert.That(company.Jobs, Is.Not.Null);
            Assert.That(company.Jobs, Is.Empty);
            Assert.That(company.IsDeleted, Is.False);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            string invalidName = "";

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => 
                new Company(invalidName, _validAddress, "Desc", "Tech", "Url"));
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenAddressIsNull()
        {
            // Arrange
            Address invalidAddress = null;

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => 
                new Company("Name", invalidAddress, "Desc", "Tech", "Url"));
        }

        [Test]
        public void UpdateInfo_ShouldUpdateDescriptionAndLogo_WhenValid()
        {
            // Arrange
            var company = new Company("TechCorp", _validAddress, "Old Desc", "Tech", "OldUrl");
            string newDesc = "New Description";
            string newIndustry = "New Tech";
            string newLogo = "NewUrl";

            // Act
            company.UpdateInfo(newDesc, newIndustry, newLogo);

            // Assert
            Assert.That(company.Description, Is.EqualTo(newDesc));
            Assert.That(company.Industry, Is.EqualTo(newIndustry));
            Assert.That(company.LogoUrl, Is.EqualTo(newLogo));
        }

        [Test]
        public void UpdateInfo_ShouldNotUpdate_WhenValuesAreNull()
        {
            // Arrange
            var company = new Company("TechCorp", _validAddress, "Old Desc", "Tech", "OldUrl");

            // Act
            company.UpdateInfo(null, null, null);

            // Assert - Logic: Description = description ?? Description;
            Assert.That(company.Description, Is.EqualTo("Old Desc"));
            Assert.That(company.Industry, Is.EqualTo("Tech"));
            Assert.That(company.LogoUrl, Is.EqualTo("OldUrl"));
        }

        [Test]
        public void AddEmployee_ShouldAddUser_WhenUserIsValid()
        {
            // Arrange
            var company = new Company("TechCorp", _validAddress, "Desc", "Tech", "Url");
            
            var email = Email.Create("john@example.com");
            var user = new User("John Doe", email, UserRole.Applicant, (e) => false); 

            // Act
            company.AddEmployee(user);

            // Assert
            Assert.That(company.Employees, Does.Contain(user));
        }

        [Test]
        public void AddJob_ShouldAddJob_WhenJobIsValid()
        {
            // Arrange
            var company = new Company("TechCorp", _validAddress, "Desc", "Tech", "Url");
            
            var email = Email.Create("hr@example.com");
            var hrUser = new User("HR Manager", email, UserRole.HR, (e) => false);
            
            var salary = SalaryRange.Create(50000, 80000);
            var job = new Job(company.Id, hrUser.Id, "Dev", salary, "Code stuff", "C#", 3);

            // Act
            company.AddJob(job);

            // Assert
            Assert.That(company.Jobs, Does.Contain(job));
        }

        [Test]
        public void SoftDelete_ShouldSetIsDeletedTrue()
        {
            // Arrange
            var company = new Company("TechCorp", _validAddress, "Desc", "Tech", "Url");
            ISoftDelete softDeleteCompany = company;

            // Act
            softDeleteCompany.SoftDelete();

            // Assert
            Assert.That(company.IsDeleted, Is.True);
            Assert.That(company.DeletedAt, Is.Not.Null);
        }
    }
}
