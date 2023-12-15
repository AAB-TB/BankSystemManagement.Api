using AutoMapper;
using BankSystemManagement.Core.Services;
using BankSystemManagement.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankSystemManagement.Tests
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task AccountTransferAsync_WhenAuthorizedAndSufficientBalance_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var fromAccountId = 123;
            var toAccountId = 456;
            var amount = 50.0m;

            var mockAccountRepo = new Mock<IAccountRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<AccountService>>();

            // Set up mock behavior for CheckAccountOwnershipAsync
            mockAccountRepo.Setup(repo => repo.CheckAccountOwnershipAsync(userId, fromAccountId))
                           .ReturnsAsync(true);

            // Set up mock behavior for GetBalanceAsync
            mockAccountRepo.Setup(repo => repo.GetBalanceAsync(fromAccountId))
                           .ReturnsAsync(100.0m); // Assuming sufficient balance

            // Set up mock behavior for AccountTransferAsync
            mockAccountRepo.Setup(repo => repo.AccountTransferAsync(fromAccountId, toAccountId, amount))
                           .ReturnsAsync(true); // Assuming successful transfer

            var accountService = new AccountService(mockAccountRepo.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await accountService.AccountTransferAsync(userId, fromAccountId, toAccountId, amount);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task CreateBankAccountAsync_WhenAuthorizedAndCustomerExists_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var customerId = 1;
            var accountTypeId = 2;

            var mockAccountRepo = new Mock<IAccountRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<AccountService>>();

            // Set up mock behavior for CheckCustomerExistsAsync
            mockAccountRepo.Setup(repo => repo.CheckCustomerExistsAsync(customerId))
                           .ReturnsAsync(true);

            // Set up mock behavior for CreateBankAccountAsync
            mockAccountRepo.Setup(repo => repo.CreateBankAccountAsync(customerId, accountTypeId))
                           .ReturnsAsync(true); // Assuming successful bank account creation

            var accountService = new AccountService(mockAccountRepo.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await accountService.CreateBankAccountAsync(userId, customerId, accountTypeId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AccountTransferAsync_WhenNotAuthorized_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var fromAccountId = 123;
            var toAccountId = 456;
            var amount = 50.0m;

            var mockAccountRepo = new Mock<IAccountRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<AccountService>>();

            // Set up mock behavior for CheckAccountOwnershipAsync
            mockAccountRepo.Setup(repo => repo.CheckAccountOwnershipAsync(userId, fromAccountId))
                           .ReturnsAsync(false); // Simulate not authorized

            var accountService = new AccountService(mockAccountRepo.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await accountService.AccountTransferAsync(userId, fromAccountId, toAccountId, amount);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateBankAccountAsync_WhenCustomerDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var customerId = 1;
            var accountTypeId = 2;

            var mockAccountRepo = new Mock<IAccountRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<AccountService>>();

            // Set up mock behavior for CheckCustomerExistsAsync
            mockAccountRepo.Setup(repo => repo.CheckCustomerExistsAsync(customerId))
                           .ReturnsAsync(false); // Simulate customer does not exist

            var accountService = new AccountService(mockAccountRepo.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await accountService.CreateBankAccountAsync(userId, customerId, accountTypeId);

            // Assert
            Assert.False(result);
        }

    }
}