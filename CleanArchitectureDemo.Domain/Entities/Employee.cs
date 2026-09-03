using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Domain.Entities
{
    public class Employee
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public decimal Salary { get; private set; }

        public int LeaveDays { get; private set; }

        public string Address { get; private set; } = string.Empty;

        public string Type { get; private set; } = string.Empty;

        public Employee(string name,string email,decimal salary,int leaveDays,string address,string type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Employee name is required.");

            if (salary <= 0)
                throw new ArgumentException("Salary must be greater than zero.");

            if (leaveDays < 0)
                throw new ArgumentException("Leave days cannot be negative.");

            Name = name;
            Email = email;
            Salary = salary;
            LeaveDays = leaveDays;
            Address = address;
            Type = type;
        }

    }
}
