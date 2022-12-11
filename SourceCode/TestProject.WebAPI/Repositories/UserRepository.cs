using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Data;
using TestProject.WebAPI.DataModels;

namespace TestProject.WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccountAsync(User user)
        {
            if (await Exist(user.Email))
            {
                Account account = new Account
                {
                    Email = user.Email,
                    MonthlyExpense = user.MonthlyExpense,
                    Name = user.Name,
                    Salary = user.Salary,
                    AccountCreated = DateTime.Now
                };
                var AccountAdded = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                return AccountAdded.Entity;
            }
            return null;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var UserAdded = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return UserAdded.Entity;
        }

        public async Task<bool> Exist(string Email)
        {
            return await _context.Users.AnyAsync(x => x.Email == Email);
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByEmail(string Email)
        {
            var userDetails = await _context.Users.Where(x => x.Email == Email).SingleOrDefaultAsync();
            return userDetails;
           
        }
    }
}
