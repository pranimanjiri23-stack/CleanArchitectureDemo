using CleanArchitectureDemo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.BonusCalculators
{
    //Opan Close Priciple (SOLID)
    public class InternBonusCalculator : IBonusCalculator
    {
        public bool CanHandle(string employeeType)
        {
            return employeeType.Equals(
                "Intern",
                StringComparison.OrdinalIgnoreCase);
        }

        public decimal Calculate(decimal salary)
        {
            return salary * 0.02m;
        }
    }
}
