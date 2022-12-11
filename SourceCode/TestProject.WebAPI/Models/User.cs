
using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.WebAPI.DataModels
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public double Salary { get; set; }

        public double MonthlyExpense { get; set; }
    }
}
