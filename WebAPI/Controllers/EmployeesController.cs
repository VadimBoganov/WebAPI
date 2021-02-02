using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly IRepository<Employee> _repository;
        public EmployeesController(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        [HttpGet("{pageNo?}/{pageSize?}")]
        public IEnumerable<object> GetEmployees(int pageNo = 1, int pageSize = 5) 
        {
            int skip = (pageNo - 1) * pageSize;

            return _repository.Data.OrderByDescending(e => e.Salary)
                            .ThenBy(e => e.FullName)
                            .Skip(skip)
                            .Take(pageSize)
                            .Select(e => new { e.ID, e.FullName, e.Salary })
                            .ToList();
        }

        [HttpGet("{name}")]
        public ActionResult<Employee> GetByName(string name)
        {
            var employee = _repository.Data.FirstOrDefault(e => e.FirstName == name);

            if (employee == null)
                return NotFound($"Employee with name {name} doesn't exist...");

            return employee;
        }

        [HttpGet("sumSalary")]
        public ActionResult<double> GetSumSalary() 
        {
            var employees = _repository.Data;

            if (employees.Count() == 0)
                return NotFound("Employees list is empty...");

            return employees.Select(e => e.Salary).Sum();
        }
        
        [HttpGet("maxSalary")]
        public ActionResult<Employee> GetMaxSalary()
        {
            var employees = _repository.Data;

            if (employees.Count() == 0)
                return NotFound("Employees list is empty...");

            return employees.OrderByDescending(e => e.Salary).First();
        }

        [HttpPost]
        public ActionResult<Employee> Add(Employee employee)
        {
            if (employee == null)
                return BadRequest(employee);

            _repository.Add(employee);

            return CreatedAtAction(nameof(GetEmployees), new { employee.ID }, employee);
        }
    }
}
