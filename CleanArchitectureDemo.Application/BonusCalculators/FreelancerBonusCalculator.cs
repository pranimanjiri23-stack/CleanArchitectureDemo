using CleanArchitectureDemo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.BonusCalculators
{
    public class FreelancerBonusCalculator : IBonusCalculator
    {
        public bool CanHandle(string employeeType)
        {
            return employeeType.Equals(
                "Freelancer",
                StringComparison.OrdinalIgnoreCase);
        }

        public decimal Calculate(decimal salary)
        {
            return salary * 0.03m;
        }
    }
}
