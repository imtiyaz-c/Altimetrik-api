using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System;
using TestProject.WebAPI.DataModels;

namespace TestProject.WebAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByEmail(string Email);
        Task<bool> Exist(string Email);

        Task<Account> CreateAccountAsync(User user);
        Task<List<Account>> GetAllAccountsAsync();
    }
}
