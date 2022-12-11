using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.WebAPI.DataModels
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }

        public double MonthlyExpense { get; set; }

        public DateTime AccountCreated { get; set; }
    }
}
