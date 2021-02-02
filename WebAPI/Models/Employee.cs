using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Converters;

namespace WebAPI.Models
{
    [JsonConverter(typeof(EmployeeJsonConverter))]
    public abstract class Employee
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        
        public int Rate { get; set; }

        [NotMapped]
        public double Salary => GetSalary();

        public string Discriminator { get; set; }

        public abstract double GetSalary();
    }
}
