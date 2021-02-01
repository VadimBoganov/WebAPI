namespace WebAPI.Models
{
    public abstract class Employee
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FullName => $"{FirstName} {SecondName}";

        public int Rate { get; set; }

        public abstract double GetSalary();
    }
}
