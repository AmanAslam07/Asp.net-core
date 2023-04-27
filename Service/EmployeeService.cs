using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web_Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeDbContext employeeDbContext;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<Employee> userManager;
        private readonly SignInManager<Employee> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public EmployeeService(EmployeeDbContext employeeDbContext,
           IConfiguration configuration,
           IHttpContextAccessor httpContextAccessor,
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            RoleManager<IdentityRole> roleManager)
       {
            this.employeeDbContext = employeeDbContext;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var records = await employeeDbContext.Employees.ToListAsync();
            return records;
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var record = await employeeDbContext.Employees.FirstOrDefaultAsync(x => x.IdEmployee == id);
            return record;
        }
        public async Task<string> RegisterEmployeeAsync(Employee employee)
        {
            var Employee = new Employee
            {
                Name = employee.Name,
                Address = employee.Address,
                EmailAddress = employee.EmailAddress,
                Password = employee.Password
            };
            if (!await roleManager.RoleExistsAsync(Role.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Admin));
            }
            if (!await roleManager.RoleExistsAsync(Role.Employee))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Employee));
            }
            if (await roleManager.RoleExistsAsync(Role.Employee))
            {
                await userManager.AddToRoleAsync(Employee, Role.Employee);
            }
            employeeDbContext.Employees.Add(Employee);
            employeeDbContext.SaveChanges();
            return "Employee Added";
        }
        public async Task<string> RegisterAdminAsync(Employee employee)
        {
            var EmployeeExists=await userManager.FindByNameAsync(employee.Name);
            if (EmployeeExists != null)
            {
               return "Employee Already exists";
           }
            else
           {
               var Emp = new Employee
                {
                    Name = employee.Name,
                    Address = employee.Address,
                    EmailAddress = employee.EmailAddress,
                    Password = employee.Password
               };
                
               
                if (!await roleManager.RoleExistsAsync(Role.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(Role.Admin));
                }
                if(!await roleManager.RoleExistsAsync(Role.Employee)) 
                {
                    await roleManager.CreateAsync(new IdentityRole(Role.Employee));
                }
                if(await roleManager.RoleExistsAsync(Role.Admin))
                {
                    await userManager.AddToRoleAsync(Emp, Role.Admin);                    
                }
                employeeDbContext.Employees.Add(Emp);
                employeeDbContext.SaveChanges();
                return "Admin Added Successfully";
            }
        }
        public async Task<string> UpdateEmployeeAsync(Employee employee,int id)
        {

            var emp = await employeeDbContext.Employees.FirstOrDefaultAsync(e=>e.IdEmployee==id);
            if (emp != null)
            {
                
                emp.Name = employee.Name;
                emp.Address = employee.Address;
                emp.EmailAddress = employee.EmailAddress;
                emp.Password = employee.Password;
                
               
               // await userManager.UpdateAsync(emp);
                
                await employeeDbContext.SaveChangesAsync(true);
                return ("Employee Updated");
            }
            else
            {
                return "Not updated";
            }
        }
        
        public async Task<string> DeleteEmployeeAsync(int id)
        {
            var record=await employeeDbContext.Employees.Where(x=>x.IdEmployee == id).FirstOrDefaultAsync();
            employeeDbContext.Employees.Remove(record);
            employeeDbContext.SaveChanges();
            return "Employee Deleted";

        }
        public async Task<string> EmployeeLoginAsync(AuthenticateEmployee authenticateEmployee)
        {

            //var result = await signInManager.PasswordSignInAsync(authenticateEmployee.EmailAddress, authenticateEmployee.Password, false, false);
            var emp=await employeeDbContext.Employees.FirstOrDefaultAsync(x=>x.EmailAddress==authenticateEmployee.EmailAddress);                                      
            
            //var emp = await userManager.FindByEmaiAsync(authenticateEmployee.EmailAddress);

            if (emp != null )//&& await userManager.CheckPasswordAsync(emp, authenticateEmployee.Password))
            {
                var EmployeeRole = await userManager.GetRolesAsync(emp);
                var authClaims = new List<Claim>
                     {
                            new Claim(ClaimTypes.Email,authenticateEmployee.EmailAddress),
                            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                     };
                foreach (var employeeRole in EmployeeRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, employeeRole));
                }
                var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:ValidIssuer"],
                    audience: configuration["Jwt:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return "unauthorized";
            }
            
        }
        public async Task<Employee> GetEmployeeDetailsAsync()
        {
            var result = string.Empty;
             if( httpContextAccessor!=null)
            {
                result= httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            }
           
            var emp=await employeeDbContext.Employees.SingleOrDefaultAsync(e=>e.EmailAddress==result);

                return new Employee
                {
                    IdEmployee=emp.IdEmployee,
                    Name = emp.Name,
                    Address = emp.Address,
                    EmailAddress = emp.EmailAddress,
                    Password = emp.Password,
                };
            
            
        }
    }
}
