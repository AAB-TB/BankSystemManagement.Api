using AutoMapper;
using BankSystemManagement.Core.Services;
using BankSystemManagement.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Tests
{
    public class LoanServiceTests
    {
        [Fact]
        public async Task CreateLoanAsync_WhenAdminUserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var adminUserId = 1;
            var customerUserId = 2;
            var amount = 1000.0m;
            var accountTypeId = 3;

            var mockLoanRepo = new Mock<ILoanRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<LoanService>>();

            // Set up mock behavior for CheckAdminExistsAsync
            mockLoanRepo.Setup(repo => repo.CheckAdminExistsAsync(adminUserId))
                         .ReturnsAsync(false); // Simulate admin user does not exist

            var loanService = new LoanService(mockLoanRepo.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await loanService.CreateLoanAsync(adminUserId, customerUserId, amount, accountTypeId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateLoanAsync_WhenCustomerDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var adminUserId = 1;
            var customerUserId = 2;
            var amount = 1000.0m;
            var accountTypeId = 3;

            var mockLoanRepo = new Mock<ILoanRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<LoanService>>();

            // Set up mock behavior for CheckAdminExistsAsync (return true) and CheckCustomerExistsAsync (return false)
            mockLoanRepo.Setup(repo => repo.CheckAdminExistsAsync(adminUserId))
                         .ReturnsAsync(true);
            mockLoanRepo.Setup(repo => repo.CheckCustomerExistsAsync(customerUserId))
                         .ReturnsAsync(false); // Simulate customer does not exist

            var loanService = new LoanService(mockLoanRepo.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await loanService.CreateLoanAsync(adminUserId, customerUserId, amount, accountTypeId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateLoanAsync_WhenAccountTypeDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var adminUserId = 1;
            var customerUserId = 2;
            var amount = 1000.0m;
            var accountTypeId = 3;

            var mockLoanRepo = new Mock<ILoanRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<LoanService>>();

            // Set up mock behavior for CheckAdminExistsAsync (return true), CheckCustomerExistsAsync (return true),
            // and CheckAccountTypeExistsAsync (return false)
            mockLoanRepo.Setup(repo => repo.CheckAdminExistsAsync(adminUserId))
                         .ReturnsAsync(true);
            mockLoanRepo.Setup(repo => repo.CheckCustomerExistsAsync(customerUserId))
                         .ReturnsAsync(true);
            mockLoanRepo.Setup(repo => repo.CheckAccountTypeExistsAsync(customerUserId, accountTypeId))
                         .ReturnsAsync(false); // Simulate account type does not exist

            var loanService = new LoanService(mockLoanRepo.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await loanService.CreateLoanAsync(adminUserId, customerUserId, amount, accountTypeId);

            // Assert
            Assert.False(result);
        }

    }
}
