using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TestProject.WebAPI.DataModels;

namespace TestProject.WebAPI.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
