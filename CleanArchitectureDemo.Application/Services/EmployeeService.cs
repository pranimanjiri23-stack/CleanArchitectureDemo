using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Employee employee)
        {
            // Business logic can be added here

            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new Exception("Employee name is required.");

            await _repository.AddAsync(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            await _repository.UpdateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee != null)
            {
                await _repository.DeleteAsync(employee);
            }
        }
    }
}
