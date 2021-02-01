namespace WebAPI.Models
{
    public class FixedEmployee : Employee
    {
        public override double GetSalary() => Rate;
    }
}
