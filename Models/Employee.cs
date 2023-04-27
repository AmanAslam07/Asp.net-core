using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Api.Models
{
    public class Employee:IdentityUser
    {
     
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int IdEmployee { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Password { get; set; }


    }
}
