using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Queries
{
    //CQRS → GetEmployeeById Query
    public class GetEmployeeByIdQueryHandler
    {
        private readonly IEmployeeRepository _repository;

        public GetEmployeeByIdQueryHandler(
            IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Employee?> Handle(
            GetEmployeeByIdQuery query)
        {
            return await _repository.GetByIdAsync(query.Id);
        }
    }
}
