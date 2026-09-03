using CleanArchitectureDemo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Services
{
    public class BonusService
    {
        private readonly IEnumerable<IBonusCalculator> _calculators;

        public BonusService(IEnumerable<IBonusCalculator> calculators)
        {
            _calculators = calculators;
        }

        public decimal CalculateBonus(
            string employeeType,
            decimal salary)
        {
            var calculator = _calculators
                .FirstOrDefault(x => x.CanHandle(employeeType));

            if (calculator == null)
                throw new Exception(
                    $"No bonus calculator found for {employeeType}");

            return calculator.Calculate(salary);
        }
    }
}
