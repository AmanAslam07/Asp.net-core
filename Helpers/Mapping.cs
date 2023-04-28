using AutoMapper;
using Web_Api.Models;

namespace Web_Api.Helpers
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Employee, Employee>().ReverseMap();
        }
    }
}
