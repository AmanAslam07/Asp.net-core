using Microsoft.AspNetCore.Mvc;
using Web_Api.Models;

namespace Web_Api.Service
{
    public interface IEmployeeService
    {
         Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<string> RegisterEmployeeAsync(Employee employee);
        Task<string> UpdateEmployeeAsync(Employee employee, int id);
        Task<string> DeleteEmployeeAsync(int id);
        Task<string> EmployeeLoginAsync(AuthenticateEmployee authenticateEmployee);
        Task<string> RegisterAdminAsync(Employee employee);
        Task<Employee> GetEmployeeDetailsAsync();
    }
}
