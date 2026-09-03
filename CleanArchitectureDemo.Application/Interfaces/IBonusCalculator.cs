using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Interfaces
{
    public interface IBonusCalculator
    {
        bool CanHandle(string employeeType);

        decimal Calculate(decimal salary);
    }
}
