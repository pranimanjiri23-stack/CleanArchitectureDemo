using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Queries
{
    public class GetAllEmployeesQueryHandler
    {
        private readonly IEmployeeRepository _repository;

        public GetAllEmployeesQueryHandler(
            IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Employee>> Handle(
            GetAllEmployeesQuery query)
        {
            return await _repository.GetAllAsync();
        }
    }
}
