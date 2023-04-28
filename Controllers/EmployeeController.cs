using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Api.Models;
using Web_Api.Service;

namespace Web_Api.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        [Authorize(Roles = Role.Admin)]
        [Authorize]
        [HttpGet]
        [Route("api/Employee/GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees=await employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [Route("api/Employee/GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute]int id)
        {
            var employee = await employeeService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }
        
        [HttpPost]
        [Route("api/Employee/RegisterEmployee")]
        public async Task<string> RegisterEmployee([FromBody]Employee employee)
        {
            return await employeeService.RegisterEmployeeAsync(employee);
        }
       
        [HttpPost]
        [Route("api/Employee/RegisterAdmin")]
        public async Task<string> RegisterAdmin([FromBody]Employee employee)
        {
            return await employeeService.RegisterAdminAsync(employee);
        }
        [Authorize(Roles = Role.Admin)]
        [HttpPut]
        [Route("api/Employee/Update Employee/{id}")]
        public async Task<string> UpdateEmployee([FromBody]Employee employee, [FromRoute]int id)
        {
            return await employeeService.UpdateEmployeeAsync(employee, id);
        }
        [Authorize(Roles = Role.Admin)]
        [HttpDelete]
        [Route("api/Employee/DeleteEmployee/{id}")]
        public async Task<string> DeleteEmployee([FromRoute]int id)
        {
            return await employeeService.DeleteEmployeeAsync(id);
        }
        [HttpPost]
        [Route("api/Employee/LoginEmployee")]
        public async Task<string> LoginEmployee([FromBody]AuthenticateEmployee authenticateEmployee)
        {
            return await employeeService.EmployeeLoginAsync(authenticateEmployee);
        }
        [Authorize(Roles =Role.Employee)]
        [HttpGet]
        [Route("api/Employee/GetEmployeeDetails")]
        public async Task<Employee> GetEmployeeeDetails()
        {
            return await employeeService.GetEmployeeDetailsAsync();
        }
    }
}
