using CleanArchitectureDemo.Application.Queries;
using CleanArchitectureDemo.Application.Services;
using CleanArchitectureDemo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;
        private readonly BonusService _bonusService;
        private readonly GetAllEmployeesQueryHandler _getAllEmployeesHandler;
        private readonly GetEmployeeByIdQueryHandler _getEmployeeByIdHandler;

        public EmployeeController(EmployeeService service, BonusService bonusService, GetAllEmployeesQueryHandler getAllEmployeesHandler, GetEmployeeByIdQueryHandler getEmployeeByIdHandler)
        {
            _service = service;
            _bonusService = bonusService;
            _getAllEmployeesHandler = getAllEmployeesHandler;
            _getEmployeeByIdHandler = getEmployeeByIdHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllEmployeesQuery();

            var employees = await _getAllEmployeesHandler.Handle(query);

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetEmployeeByIdQuery(id);

            var employee = await _getEmployeeByIdHandler.Handle(query);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [Authorize(Roles = "Admin1")]
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            await _service.AddAsync(employee);

            return Ok(employee);
        }


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("bonus")]
        public IActionResult CalculateBonus(string type, decimal salary)
        {
            var bonus = _bonusService.CalculateBonus(type, salary);

            return Ok(new
            {
                EmployeeType = type,
                Salary = salary,
                Bonus = bonus
            });
        }
    }
}
