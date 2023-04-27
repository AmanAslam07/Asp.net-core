using Microsoft.AspNetCore.Identity;

namespace Web_Api.Models
{
    public class Role:IdentityRole
    {
        public const string Admin = "Admin";
        public const string Employee = "Employee";
    }
}
