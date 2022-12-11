using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using TestProject.WebAPI.Controllers;
using TestProject.WebAPI.Data;
using TestProject.WebAPI.DataModels;
using TestProject.WebAPI.Repositories;
using Xunit;

namespace TestProject.Tests
{
    public class UsersTests
    {
        private Mock<IUserRepository> _userMockRepository = new Mock<IUserRepository>();
        private UsersController _usersController;

   
        [Fact]
        public void GivenCreateUserAsyncPassedWithValidUserCredentialShouldCreateAnAccount()
        {
            User user = new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            _userMockRepository.Setup(x => x.CreateUserAsync(It.IsAny<User>())).Returns(Task.FromResult(user));
            _userMockRepository.Setup(x => x.Exist(It.IsAny<string>())).Returns(Task.FromResult(false));
            _usersController = new UsersController(_userMockRepository.Object);
            var result = _usersController.CreateUserAsync(user).Result;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var users = (result as OkObjectResult).Value as User;
            Assert.Equal(users?.Name, user.Name);
            Assert.Equal(users?.Email, user.Email);
            Assert.Equal(users?.Salary, user.Salary);
            Assert.Equal(users?.MonthlyExpense, user.MonthlyExpense);
        }

        [Fact]
        public void GivenCreateUserAsyncPassedWithNonUniqueUserShouldNotCreateAnAccount()
        {
            User user = new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            var userTaken = "UserName already taken";
            _userMockRepository.Setup(x => x.CreateUserAsync(It.IsAny<User>())).Returns(Task.FromResult(user));
            _userMockRepository.Setup(x => x.Exist(It.IsAny<string>())).Returns(Task.FromResult(true));
            _usersController = new UsersController(_userMockRepository.Object);
            var result = _usersController.CreateUserAsync(user).Result;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var users = (result as BadRequestObjectResult);
            Assert.Equal(users.Value, userTaken);
        }

        [Fact]
        public void GivenCreateUserAsyncPassedWithLessThan1000SalaryShouldNotCreateAnAccount()
        {
            User user = new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 900 };
            var inSufficientSalary = "User Salary/Monthly Expenses can't be less than 1000";
            _userMockRepository.Setup(x => x.CreateUserAsync(It.IsAny<User>())).Returns(Task.FromResult(user));
            _userMockRepository.Setup(x => x.Exist(It.IsAny<string>())).Returns(Task.FromResult(false));
            _usersController = new UsersController(_userMockRepository.Object);
            var result = _usersController.CreateUserAsync(user).Result;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var users = (result as BadRequestObjectResult);
            Assert.Equal(users.Value, inSufficientSalary);
        }
        [Fact]
        public void GivenGetUsersWhenCalledShouldReturnListofAllUsers()
        {
            List<User> user = new List<User>
            { 
                new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 },
                new User { Email = "chow@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 2000, Name = "chowdhary", Salary = 2500 }
            };
            _userMockRepository.Setup(x => x.GetAllUsersAsync()).Returns(Task.FromResult(user));
            _usersController = new UsersController(_userMockRepository.Object);
            var output = _usersController.GetUsers().Result;
            Assert.NotNull(output);
            Assert.IsType<OkObjectResult>(output);
            var result = (output as OkObjectResult).Value as List<User>;
            Assert.Equal(result?.Count, user.Count);
            Assert.Equal(result[0].Email, user[0].Email);
            Assert.Equal(result[0].Salary, user[0].Salary);
            Assert.Equal(result[1].Salary, user[1].Salary);
            Assert.Equal(result[1].MonthlyExpense, user[1].MonthlyExpense);
        }

        [Fact]
        public void GivenGetAccountsAsyncWhenCalledShouldReturnListofAllAccounts()
        {
            List<Account> accounts = new List<Account>
            {
                new Account { Email = "imti@altimetrik.com", AccountId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000, AccountCreated = DateTime.Now },
                new Account { Email = "chow@altimetrik.com", AccountId = Guid.NewGuid(), MonthlyExpense = 2000, Name = "chowdhary", Salary = 2500,AccountCreated = DateTime.Now }
            };
            _userMockRepository.Setup(x => x.GetAllAccountsAsync()).Returns(Task.FromResult(accounts));
            _usersController = new UsersController(_userMockRepository.Object);
            var output = _usersController.GetAccountsAsync().Result;
            Assert.NotNull(output);
            Assert.IsType<OkObjectResult>(output);
            var result = (output as OkObjectResult).Value as List<Account>;
            Assert.Equal(result?.Count, accounts.Count);
            Assert.Equal(result[0].Email, accounts[0].Email);
            Assert.Equal(result[0].Salary, accounts[0].Salary);
            Assert.Equal(result[1].Salary, accounts[1].Salary);
            Assert.Equal(result[1].MonthlyExpense, accounts[1].MonthlyExpense);
        }

        [Fact]
        public void GivenCreateAccountAsyncPassedWithValidUserCredentialShouldCreateAnAccount()
        {
            Account account = new Account { Email = "imti@altimetrik.com", AccountId = Guid.NewGuid(), AccountCreated = DateTime.Now, MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            User user = new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            _userMockRepository.Setup(x => x.CreateAccountAsync(It.IsAny<User>())).Returns(Task.FromResult(account));
            _userMockRepository.Setup(x => x.Exist(It.IsAny<string>())).Returns(Task.FromResult(true));          
            _usersController = new UsersController(_userMockRepository.Object);
            var result = _usersController.CreateAccountAsync(user).Result;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var users = (result as OkObjectResult).Value as Account;
            Assert.Equal(users?.Name, account.Name);
            Assert.Equal(users?.Email, account.Email);
            Assert.Equal(users?.Salary, account.Salary);
            Assert.Equal(users?.MonthlyExpense, account.MonthlyExpense);

        }

        [Fact]
        public void GivenCreateAccountAsyncPassedWithInValidUserCredentialThenShouldNotCreateAnAccount()
        {
            Account account = new Account { Email = "imti@altimetrik.com", AccountId = Guid.NewGuid(), AccountCreated = DateTime.Now, MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            var NonExistUser = "User does not exist";
            User user = new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            _userMockRepository.Setup(x => x.CreateAccountAsync(It.IsAny<User>())).Returns(Task.FromResult(account));
            _userMockRepository.Setup(x => x.Exist(It.IsAny<string>())).Returns(Task.FromResult(false));
            _usersController = new UsersController(_userMockRepository.Object);
            var result = _usersController.CreateAccountAsync(user).Result;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var users = (result as BadRequestObjectResult);
            Assert.Equal(users?.Value, NonExistUser);

        }

        [Fact]
        public void GivenGetUserByEmailPassedWithInValidEmailThenThrowBadrequest()
        {
          //  Account account = new Account { Email = "imti@altimetrik.com", AccountId = Guid.NewGuid(), AccountCreated = DateTime.Now, MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            var NonExistUser = "User does not exist";
            User user = new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            _userMockRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(user));
            _userMockRepository.Setup(x => x.Exist(It.IsAny<string>())).Returns(Task.FromResult(false));
            _usersController = new UsersController(_userMockRepository.Object);
            var result = _usersController.GetUserByEmail(user.Email).Result;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var users = (result as BadRequestObjectResult);
            Assert.Equal(users?.Value, NonExistUser);

        }

        [Fact]
        public void GivenGetUserByEmailPassedWithValidEmailThenShouldGetValidUserDetails()
        {
            User user = new User { Email = "imti@altimetrik.com", UserId = Guid.NewGuid(), MonthlyExpense = 1500, Name = "imtiyaz", Salary = 2000 };
            _userMockRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(user));
            _userMockRepository.Setup(x => x.Exist(It.IsAny<string>())).Returns(Task.FromResult(true));
            _usersController = new UsersController(_userMockRepository.Object);
            var output = _usersController.GetUserByEmail(user.Email).Result;
            Assert.NotNull(output);
            Assert.IsType<OkObjectResult>(output);
            var result = (output as OkObjectResult).Value as User;
            Assert.Equal(result?.Name, user.Name);
            Assert.Equal(result?.Email, user.Email);
            Assert.Equal(result?.Salary, user.Salary);
            Assert.Equal(result?.MonthlyExpense, user.MonthlyExpense);

        }
    }
}