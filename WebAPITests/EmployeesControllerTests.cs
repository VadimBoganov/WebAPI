using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebAPI.DAL;
using WebAPI.Models;
using Xunit;
using FluentAssertions;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Moq;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITests
{
    public class EmployeesControllerTests : IClassFixture<WebApplicationFactory<WebAPI.Startup>>
    {
        private readonly List<Employee> _employees;
        private readonly HttpClient _httpClient;
        private readonly EmployeeContext _employeeContext;
        private readonly DbContextOptions<EmployeeContext> _options;

        public EmployeesControllerTests(WebApplicationFactory<WebAPI.Startup> fixture)
        {
            _httpClient = fixture.CreateClient();

            _options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseSqlServer("Data Source=(local);Initial Catalog=WebAPI;Integrated Security=True")
                .Options;

            _employeeContext = new EmployeeContext(_options);

            _employees = new List<Employee>();
            _employees.AddRange(new List<Employee>
            {
                new FixedEmployee
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Rate = 70000,
                    Discriminator = nameof(FixedEmployee)
                },
                new HourlyEmployee
                {
                    FirstName = "Petr",
                    LastName = "Petrov",
                    Rate = 1000,
                    Discriminator = nameof(HourlyEmployee)
                }
            });
        }

        #region Unit tests

        [Fact]
        public void Get_ShouldReturnNotFound_WhenFindNotExistName()
        {
            var mockRepository = new Mock<IRepository<Employee>>();
            mockRepository.Setup(repo => repo.Data).Returns(_employees);

            var controller = new EmployeesController(mockRepository.Object);

            var result = controller.GetByName("123").Result as NotFoundObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Value.Should().Be("Employee with name 123 doesn't exist...");
        }

        [Fact]
        public void Get_SholudReturnEmploeeByName_WhenIsExistInDatabase()
        {
            var mockRepository = new Mock<IRepository<Employee>>();
            mockRepository.Setup(repo => repo.GetEmployeeByName("Ivan")).Returns(_employees);

            var controller = new EmployeesController(mockRepository.Object);

            var result = controller.GetByName("Ivan");

            result.Value.Should().NotBeNull();
        }

        [Fact]
        public void Get_SholudReturnNotFound_WhenDatabaseIsEmpty()
        {
            var mockRepository = new Mock<IRepository<Employee>>();
            mockRepository.Setup(repo => repo.Data).Returns(new List<Employee>());

            var controller = new EmployeesController(mockRepository.Object);

            var result = controller.GetMaxSalary().Result as NotFoundObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Value.Should().Be("Employees list is empty...");
        }

        [Fact]
        public void Get_ShouldReturnSumSalary_WhenDatabaseNotEmpty()
        {
            var mockRepository = new Mock<IRepository<Employee>>();
            mockRepository.Setup(repo => repo.Data).Returns(_employees);

            var controller = new EmployeesController(mockRepository.Object);

            var result = controller.GetSumSalary();

            result.Value.Should().BeGreaterThan(0);
        }

        #endregion

        #region Integration tests

        [Fact]
        public async void Get_ShouldReturnEmployeeByName()
        {
            var response = await _httpClient.GetAsync("https://localhost:5000/api/employees/Ivan");
            var content = await response.Content.ReadAsStringAsync();

            var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            employees.Should().NotBeNull();
        }
        
        [Fact]
        public async void Get_ShouldReturnSumSalary()
        {
            var response = await _httpClient.GetAsync("https://localhost:5000/api/employees/sumsalary");
            var content = await response.Content.ReadAsStringAsync();

            var fromDatabase = _employeeContext.Employees.ToList().Select(e => e.Salary).Sum();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            fromDatabase.ToString().Should().Be(content.Split('.').ToArray()[0]);
        }

        [Fact]
        public async void Get_ShouldReturmEmployeeWithMaxSalary()
        {
            var response = await _httpClient.GetAsync("https://localhost:5000/api/employees/maxsalary");
            var content = await response.Content.ReadAsStringAsync();

            var employee = JsonConvert.DeserializeObject<Employee>(content);

            var fromDatabase = _employeeContext.Employees.ToList().OrderByDescending(e => e.Salary).First();

            fromDatabase.Salary.Should().Be(employee.Salary);
        }

        [Fact]
        public async void Post_ShouldReturnOk_WhenAddNewEmployee()
        {
            var fromDatabase = await _employeeContext.Employees.ToListAsync();

            foreach (var employee in fromDatabase.Where(e => e.FirstName == _employees.First().FirstName))
                _employeeContext.Remove(employee);

            await _employeeContext.SaveChangesAsync();

            var response = await _httpClient.PostAsJsonAsync("https://localhost:5000/api/employees", _employees.First());

            fromDatabase = await _employeeContext.Employees.ToListAsync();

            fromDatabase.Select(data => data.LastName).Should().Contain(_employees.First().LastName);
        }

        #endregion
    }
}
