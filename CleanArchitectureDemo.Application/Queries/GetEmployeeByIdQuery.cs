using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Queries
{
    //CQRS → GetEmployeeById Query
    public class GetEmployeeByIdQuery
    {
        public int Id { get; }

        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }
    }
}
