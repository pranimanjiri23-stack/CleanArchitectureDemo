using CleanArchitectureDemo.Application.Services;
using CleanArchitectureDemo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _service.GetAllAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _service.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            await _service.AddAsync(employee);

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            await _service.UpdateAsync(employee);

            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
    }
}
